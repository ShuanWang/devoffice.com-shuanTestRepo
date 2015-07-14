using System.Data;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using DevOffice.Secret.Helpers;
using DevOffice.Secret.Services;
using System.Web.Security.AntiXss;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using Orchard.ContentManagement;
using Orchard.Logging;
using Orchard.Security;

namespace DevOffice.Secret.Controllers
{

    public class ResultObject {
        public Boolean Success { get; set; }
        public string Error { get; set; }
    }

    public class UploadModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
        public string Industry { get; set; }
        public string CompanyAddress { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public int Zip { get; set; }
        public string ProductUrl { get; set; }
        public string IntegrationType { get; set; }
        public string Scenarios { get; set; }
        public string Description { get; set; }
        public int MonthlyUsers { get; set; }
        public int DailyUsers { get; set; }
        public int ViewSessions { get; set; }
        public int EditSessions { get; set; }
        public int MsWord { get; set; }
        public int MsPpt { get; set; }
        public int MsXl { get; set; }
        public int NorthAmerica { get; set; }
        public int SouthAmerica { get; set; }
        public int Europe { get; set; }
        public int Asia { get; set; }
        public int Africa { get; set; }
        public int Australia { get; set; }
        public string Distribution { get; set; }
        public ImageDataDTO[] EncodedImageData { get; set; }
    }

    public class SecretFormController : ApiController
    {
        private readonly IContentManager _contentManager;
        private readonly IEncryptionService _encryptionService;
        public ILogger Logger { get; set; }

        public SecretFormController(IContentManager contentManager, IEncryptionService encryptionService) {
            _contentManager = contentManager;
            _encryptionService = encryptionService;
        }

        [HttpPost]
        [ActionName("Post")]
        public HttpResponseMessage Post(UploadModel data) {

            var cleanData = new UploadModel();
            foreach (var propertyName in data.GetType().GetProperties()) {
                var element = data.GetType().GetProperty(propertyName.Name).GetValue(data, null);
                dynamic cleanValue = null;

                // validate any strings that may come in.
                // Any property value of the data object that doesn't match the property type of the UploadModel
                // will be "kicked out," meaning C# will ignore the value passed and will set it to some valid
                // default value in UploadModel. If a property like MonthlyUsers (int) is sent as a string
                // when C# converts the data into the UploadModel, the MonthlyUsers will default to the value 0.
                // Therefore, we really only need to check Strings, since those can be used to pass malicious values.
                if (element != null && propertyName.PropertyType.Name == "String") {                  
                    cleanValue = AntiXssEncoder.HtmlEncode(element.ToString(), true);
                }
                else {
                    // we are only worried about Strings, so any null or non-String value we can safely pass on.
                    cleanValue = element;
                }

                // if element is null, obviously no validation needed.
                cleanData
                    .GetType()
                    .GetProperty(propertyName.Name)
                    .SetValue(cleanData, cleanValue);
            }

            var result = new ResultObject();

            try {
                var proxy = new RESTProxy(new ProxyConfiguration(_contentManager, _encryptionService, ProxySettingTypes.Default));

                var newItemId = proxy.Send(new JsonDTO
                {
                    Title = string.Concat(cleanData.Company, "_", DateTime.Now.ToString("yyyyMMdd")),
                    PartnerFirstName = cleanData.FirstName,
                    PartnerLastName = cleanData.LastName,
                    PartnerEmail = cleanData.Email,
                    PartnerPhone = cleanData.Phone,
                    PartnerCompanyName = cleanData.Company,
                    PartnerIndustry = cleanData.Industry,
                    PartnerCompanyAddress = cleanData.CompanyAddress,
                    PartnerCountry = cleanData.Country,
                    PartnerStateProvince = cleanData.State,
                    PartnerCity = cleanData.City,
                    PartnerZip = cleanData.Zip,
                    ProductUrl = new UrlDTO { Url = cleanData.ProductUrl, Description = "" },
                    IntegrationType = cleanData.IntegrationType,
                    PartnershipScenarios = data.Scenarios, // not encoded
                    PartnershipDescription = data.Description, // not encoded
                    MonthlyUsers = cleanData.MonthlyUsers,
                    DailyUsers = cleanData.DailyUsers,
                    ViewSessions = cleanData.ViewSessions,
                    EditSessions = cleanData.EditSessions,
                    TotalWordDocuments = cleanData.MsWord,
                    TotalPPTDocuments = cleanData.MsPpt,
                    TotalExcelDocuments = cleanData.MsXl,
                    NorthAmerica = cleanData.NorthAmerica,
                    SouthAmerica = cleanData.SouthAmerica,
                    Europe = cleanData.Europe,
                    Asia = cleanData.Asia,
                    Africa = cleanData.Africa,
                    Australia = cleanData.Australia,
                    DatacentreDistribution = data.Distribution // not encoded
                });


                var newAttachmentIds = proxy.Attach(cleanData.EncodedImageData, newItemId);

                result.Success = true;

                try
                {
                    //Nested try because an exception from sending mail could be anything
                    //and we don't want to tell the user we failed if sending the email fails
                    new MailHelper(_contentManager, _encryptionService).SendCloudStorageEmail(cleanData.Email, cleanData.FirstName);
                }
                catch (Exception exception)
                {
                    Logger.Error(exception, string.Format("Error sending cloud storage email to {0}", cleanData.Email));
                }
            }
            catch (Exception exception) {
                result.Error = exception.Message;
            }

            var response = Request.CreateResponse(HttpStatusCode.OK, result, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            return response;
        }
    }
}