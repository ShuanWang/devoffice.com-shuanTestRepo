using System.Net;
using System.Net.Http;
using System.Web.Http;
using DevOffice.Secret.Services;
using System.Web.Security.AntiXss;
using System;
using Orchard.ContentManagement;
using Orchard.Logging;
using Orchard.Security;
using DevOffice.Secret.Helpers;

namespace DevOffice.Secret.Controllers
{

    public class ActivityFeedAPIFormResultObject
    {
        public Boolean Success { get; set; }
        public string Error { get; set; }
    }

    public class ActivityFeedAPIFormUploadModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string PageUrl { get; set; }
        public string Scenarios { get; set; }
        public string PlatformIntegrationDescription { get; set; }
        public int MonthlyUsers { get; set; }
        public int DailyUsers { get; set; }
        public int Current365Customers { get; set; }
        public int Future365Customers { get; set; }
        public string IsvPartner { get; set; }
    }

    public class ActivityFeedAPIFormController : ApiController
    {
        private readonly IContentManager _contentManager;
        private readonly IEncryptionService _encryptionService;
        public ILogger Logger { get; set; }
        public ActivityFeedAPIFormController(IContentManager contentManager, IEncryptionService encryptionService)
        {
            _contentManager = contentManager;
            _encryptionService = encryptionService;
        }

        [HttpPost]
        [ActionName("Post")]
        public HttpResponseMessage Post(ActivityFeedAPIFormUploadModel data)
        {
            var cleanData = new ActivityFeedAPIFormUploadModel();
            foreach (var propertyName in data.GetType().GetProperties())
            {
                var element = data.GetType().GetProperty(propertyName.Name).GetValue(data, null);
                dynamic cleanValue = null;

                // validate any strings that may come in.
                // Any property value of the data object that doesn't match the property type of the APIRequestUploadModel
                // will be "kicked out," meaning C# will ignore the value passed and will set it to some valid
                // default value in APIRequestUploadModel. If a property like MonthlyUsers (int) is sent as a string
                // when C# converts the data into the UploadModel, the MonthlyUsers will default to the value 0.
                // Therefore, we really only need to check Strings, since those can be used to pass malicious values.
                if (element != null && propertyName.PropertyType.Name == "String")
                {
                    cleanValue = AntiXssEncoder.HtmlEncode(element.ToString(), true);
                }
                else
                {
                    // we are only worried about Strings, so any null or non-String value we can safely pass on.
                    cleanValue = element;
                }

                // if element is null, obviously no validation needed.
                cleanData
                    .GetType()
                    .GetProperty(propertyName.Name)
                    .SetValue(cleanData, cleanValue);
            }

            var result = new ActivityFeedAPIFormResultObject();

            try {

                var config = new ProxyConfiguration(_contentManager, _encryptionService, ProxySettingTypes.APISubmission);

                var proxy = new RESTProxy(config);

                var newItemId = proxy.Send(new JsonDTOApiSubmission
                {
                    Title = string.Concat(cleanData.CompanyName, "_", DateTime.Now.ToString("yyyyMMdd")),
                    SubmissionFirstName = cleanData.FirstName,
                    SubmissionLastName = cleanData.LastName,
                    SubmissionEmail = cleanData.Email,
                    SubmissionPhone = cleanData.Phone,
                    SubmissionCompanyName = cleanData.CompanyName,
                    SubmissionCompanyAddress = cleanData.CompanyAddress,
                    SubmissionCountry = cleanData.Country,
                    SubmissionState = cleanData.State,
                    SubmissionCity = cleanData.City,
                    SubmissionPostCode = cleanData.PostCode,
                    ProductPageUrl = new UrlDTO { Url = cleanData.PageUrl, Description = "" },
                    PlatformIntegrationDescription = data.PlatformIntegrationDescription,
                    SubmissionScenario = data.Scenarios,
                    MonthlyUsers = cleanData.MonthlyUsers,
                    DailyUsers = cleanData.DailyUsers,
                    Current365Customers = cleanData.Current365Customers,
                    Future365Customers = cleanData.Future365Customers,
                    IsvPartner = cleanData.IsvPartner
                });

                result.Success = true;

                try
                {
                    //Nested try because an exception from sending mail could be anything
                    //and we don't want to tell the user we failed if sending the email fails
                    //NOTE Email will not be sent if no From address
                    new MailHelper(_contentManager, _encryptionService).SendActivityFeedApiEmail(cleanData.Email, cleanData.FirstName);
                }
                catch(Exception exception) 
                {
                    Logger.Error(exception, string.Format("Error sending activity feed api email to {0}", cleanData.Email));
                }

            }
            catch (Exception exception)
            {
                result.Error = exception.Message;
            }

            var response = Request.CreateResponse(HttpStatusCode.OK, result, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            return response;
        }
    }
}