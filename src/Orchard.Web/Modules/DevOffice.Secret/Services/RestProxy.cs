using DevOffice.Secret.Models;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using Orchard.ContentManagement;
using Orchard.Security;

namespace DevOffice.Secret.Services
{
    public enum ProxySettingTypes {
        Default,
        APISubmission
    }

    /// <summary>
    /// Defines settings and credentials required to access the external SP 2013 Online instance, based on config keys.
    /// </summary>
    /// <remarks>
    /// Any key being accessed via DefaultProxyConfiguration needs to be defined in the consuming application.
    /// </remarks>
    public class ProxyConfiguration
    {
        public string ContainingWebUrl { get; private set; }
        public string TargetListName { get; private set; }
        public string TargetListItemMetaType { get; private set; }

        private readonly IContentManager _contentManager;
        private readonly IEncryptionService _encryptionService;

        private SharePointSettingsPart SharePointSettingsPart { get; set; }

        public ProxyConfiguration(IContentManager contentManager, IEncryptionService encryptionService, ProxySettingTypes settingsType) {
            _contentManager = contentManager;
            _encryptionService = encryptionService;
            SharePointSettingsPart = GetSettings();

            //Set the url and list values based on proxy type
            if (settingsType == ProxySettingTypes.Default) {
                ContainingWebUrl = SharePointSettingsPart.ContainingWebUrl;
                TargetListName = SharePointSettingsPart.TargetListName;
                TargetListItemMetaType = SharePointSettingsPart.TargetListItemMetaType;
            }
            else if (settingsType == ProxySettingTypes.APISubmission) {
                ContainingWebUrl = SharePointSettingsPart.ApiSubmission_ContainingWebUrl;
                TargetListName = SharePointSettingsPart.ApiSubmission_TargetListName;
                TargetListItemMetaType = SharePointSettingsPart.ApiSubmission_TargetListItemMetaType;
            }
        }

        private SharePointSettingsPart GetSettings()
        {
            SharePointSettingsPart settings = _contentManager.Query<SharePointSettingsPart, SharePointSettingsPartRecord>().List().FirstOrDefault();
            if (settings != null)
            {
                return settings;
            }
            settings = new SharePointSettingsPart();
            settings.Record = new SharePointSettingsPartRecord();
            return settings;
        }

        // authentication
        public SharePointOnlineCredentials ProxyCredentials
        {
            get
            {
                //var un = ConfigurationManager.AppSettings["UserName"];
                var un = SharePointSettingsPart.Username;

                //var pw = ConfigurationManager.AppSettings["Password"];
                var encryptedPw = SharePointSettingsPart.Password;
                var pw = Encoding.UTF8.GetString(_encryptionService.Decode(Convert.FromBase64String(encryptedPw)));

                var securePassword = new SecureString();

                pw.ToCharArray().ToList().ForEach(securePassword.AppendChar);

                var credentials = new SharePointOnlineCredentials(un, securePassword);

                return credentials;
            }
        }
    }

    // service
    public class RESTProxy
    {
        private readonly ProxyConfiguration _config;
        private readonly string _formDigest;

        public RESTProxy(ProxyConfiguration config)
        {
            _config = config;
            _formDigest = RequestFormDigest();
        }

        /// <summary>
        /// Uploads the JSON payload to the site defined by the IProxyConfiguration.
        /// </summary>
        /// <param name="data">The JSON payload for serialisation.</param>
        /// <returns>The integer ID of the item created.</returns>
        public int Send(IJsonDTO json)
        {
            using (var client = new WebClient())
            {
                // add the meta 'type' value (required by the REST services) then serialize data for transmission.
                json.SetMetaData(new Dictionary<string, string>() 
                {
                    { "type", _config.TargetListItemMetaType }
                });

                var oDataJson = JsonConvert.SerializeObject(json);

                client.Headers.Add(HttpRequestHeader.ContentType, "application/json;odata=verbose");
                client.Headers.Add(HttpRequestHeader.Accept, "application/json;odata=verbose");

                // auth & digest
                client.Headers.Add("X-FORMS_BASED_AUTH_ACCEPTED", "f");
                client.Headers.Add("X-RequestDigest", _formDigest);
                client.Credentials = _config.ProxyCredentials;

                var createUrl = string.Format("{0}/_api/web/lists/GetByTitle('{1}')/items",
                    _config.ContainingWebUrl,
                    _config.TargetListName);

                // upload
                var rawUploadResponse = client.UploadString(createUrl, WebRequestMethods.Http.Post, oDataJson);
                var uploadResponseToken = JToken.Parse(rawUploadResponse);

                // here, use fiddler to inspect the returned JSON token if you need to surface more properties.
                var sId = uploadResponseToken["d"]["ID"].ToString();

                int id;
                var success = Int32.TryParse(sId, out id);

                return id;
            }
        }

        public IList<string> Attach(ImageDataDTO[] imageData, int parentListItemId)
        {
            var urls = new List<string>();

            Func<ImageDataDTO, string> attach = img =>
            {
                using (var client = new WebClient())
                {
                    // auth & digest
                    client.Headers.Add("X-FORMS_BASED_AUTH_ACCEPTED", "f");
                    client.Headers.Add("X-RequestDigest", _formDigest);
                    client.Credentials = _config.ProxyCredentials;

                    client.Headers.Add(HttpRequestHeader.Accept, "application/json;odata=verbose");

                    var attachUrl = string.Format("{0}/_api/web/Lists/GetByTitle('{1}')/items({2})/AttachmentFiles/add(FileName='{3}')",
                        _config.ContainingWebUrl,
                        _config.TargetListName,
                        parentListItemId,
                        img.Filename);

                    var parts = img.ImageData.Split(new [] {"base64,"}, StringSplitOptions.None);
                    if(parts!=null && parts.Length>1)
                    {
                        img.ImageData = parts[1];
                    }
                    byte[] binaryData = System.Convert.FromBase64String(img.ImageData);

                    // here the response is JSON, but the web client method turns this to a byte[] for you. yay
                    var response = Encoding.UTF8.GetString(client.UploadData(attachUrl, "POST", binaryData));

                    var uploadResponseToken = JToken.Parse(response);

                    // here, use fiddler to inspect the returned JSON token if you need to surface more properties.
                    // the returned properties __metadata.id is actually a fully qualified URL to download the item
                    var url = uploadResponseToken["d"]["__metadata"]["id"].ToString();

                    return url;
                }
            };

            foreach (var image in imageData)
            {
                urls.Add(attach(image));
            }

            return urls;
        }

        /// <summary>
        /// Retreives a form digest value for use with subsequent POST requests. This is needed for SharePoint to validate incoming requests as coming
        /// from real authenticated users.
        /// </summary>
        /// <returns></returns>
        private string RequestFormDigest()
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("X-FORMS_BASED_AUTH_ACCEPTED", "f");
                client.Credentials = _config.ProxyCredentials;

                client.Headers.Add(HttpRequestHeader.Accept, "application/json;odata=verbose");

                var endpointUri = new Uri(_config.ContainingWebUrl + "/_api/contextinfo");
                var result = client.UploadString(endpointUri, WebRequestMethods.Http.Post);

                var t = JToken.Parse(result);
                return t["d"]["GetContextWebInformation"]["FormDigestValue"].ToString();
            }
        }
    }



    public interface IJsonDTO 
    {
        void SetMetaData(Dictionary<string, string> metaData);
    }

    // models

    /// <summary>
    /// For transmission of data to SP 2013 Online via REST.
    /// </summary>
    /// <remarks>
    /// By keeping this separate from the upload model, we can use simple serialisation to generate the REST payload. Be warned 
    /// that SP is expecting property names to match column internal names.
    /// </remarks>
    public class JsonDTO : IJsonDTO
    {
        // Ignore me, for the RestProxy only
        public Dictionary<string, string> __metadata { get; set; }
        public string Title { get; set; }
        public string PartnerFirstName { get; set; }
        public string PartnerLastName { get; set; }
        public string PartnerEmail { get; set; }
        public string PartnerPhone { get; set; }
        public string PartnerCompanyName { get; set; }
        public string PartnerIndustry { get; set; }
        public string PartnerCompanyAddress { get; set; }
        public string PartnerCountry { get; set; }
        public string PartnerStateProvince { get; set; }
        public string PartnerCity { get; set; }
        public int PartnerZip { get; set; }
        public UrlDTO ProductUrl { get; set; }
        public string IntegrationType { get; set; }
        public string PartnershipScenarios { get; set; }
        public string PartnershipDescription { get; set; }
        public int MonthlyUsers { get; set; }
        public int DailyUsers { get; set; }
        public int ViewSessions { get; set; }
        public int EditSessions { get; set; }
        public int TotalWordDocuments { get; set; }
        public int TotalPPTDocuments { get; set; }
        public int TotalExcelDocuments { get; set; }
        public int NorthAmerica { get; set; }
        public int SouthAmerica { get; set; }
        public int Europe { get; set; }
        public int Asia { get; set; }
        public int Africa { get; set; }
        public int Australia { get; set; }
        public string DatacentreDistribution { get; set; }
        public void SetMetaData(Dictionary<string, string> metaData) {
            __metadata = metaData;
        }
    }

    public class JsonDTOApiSubmission : IJsonDTO
    {
        // Ignore me, for the RestProxy only
        public Dictionary<string, string> __metadata { get; set; }
        public string Title { get; set; }
        public string SubmissionFirstName { get; set; }
        public string SubmissionLastName { get; set; }
        public string SubmissionEmail { get; set; }
        public string SubmissionPhone { get; set; }
        public string SubmissionCompanyName { get; set; }
        public string SubmissionCompanyAddress { get; set; }
        public string SubmissionCountry { get; set; }
        public string SubmissionState { get; set; }
        public string SubmissionCity { get; set; }
        public string SubmissionPostCode { get; set; }
        public UrlDTO ProductPageUrl { get; set; }
        public string SubmissionScenario { get; set; }
        public string PlatformIntegrationDescription { get; set; }
        public int MonthlyUsers { get; set; }
        public int DailyUsers { get; set; }
        public int Current365Customers { get; set; }
        public int Future365Customers { get; set; }
        public string IsvPartner { get; set; }
        public void SetMetaData(Dictionary<string, string> metaData)
        {
            __metadata = metaData;
        }
    }

    public class UrlDTO
    {
        public string Url { get; set; }
        public string Description { get; set; }
    }

    public class ImageDataDTO
    {
        public string Filename { get; set; }
        public string ImageData { get; set; }

        public ImageDataDTO(string fileName, string base64EncodedData)
        {
            Filename = fileName;
            ImageData = base64EncodedData;
        }
    }
}
