using Devoffice.GettingStarted.Models;
using Devoffice.GettingStarted.Utils;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net.Http;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//HACK using System.Runtime.Serialization.Json;
namespace Devoffice.GettingStarted.Controllers
{
    [HandleError]
    public class ProxyController : Controller
    {
        private ServiceInfo GetServiceInfo(string service)
        {
            ServiceInfo si = new ServiceInfo();
            if (string.IsNullOrEmpty(service))
            {
                return null;
            }

            // retrieve the cache capabilities, if it is not present, make a request
            string capablities = (string)Session[Constants.capabilitiesTagStr];
            if(string.IsNullOrEmpty(capablities)) {
                capablities = TokenHelper.GetCapbilities((string)Session[Constants.discoveryServiceTokenTagStr]);
                if(string.IsNullOrEmpty(capablities)) {
                    return null;
                }
                Session[Constants.capabilitiesTagStr] = capablities;
            }

            // now get the service endpoint
            dynamic capablitiesJObject = (JObject.Parse(capablities))["value"];
            //dynamic valT = jbj["value"];
            for (int i = 0; i < capablitiesJObject.Count; ++i)
            {
                if (capablitiesJObject[i].capability == service)
                {
                    si.ServiceEndPointUri = capablitiesJObject[i].serviceEndpointUri;
                    si.ServiceResourceId = capablitiesJObject[i].serviceResourceId;
                        break;
                    }
            }
            if (string.IsNullOrEmpty(si.ServiceEndPointUri) || string.IsNullOrEmpty(si.ServiceResourceId))
            {
                return null;
            }
            // now get the token
            string token = (string)Session[si.ServiceEndPointUri];
            if(string.IsNullOrEmpty(token)) {
                var tokens = TokenHelper.GetDiscoveryServiceToken(
                    (string)Session[Constants.authCodeTagStr],
                    Url.Action("GetToken", "Account", null, Request.Url.Scheme),
                    Constants.TokenUri,
                    si.ServiceResourceId);
                if(tokens.ContainsKey(Constants.accessTokenTagStr))
                {
                    token = tokens[Constants.accessTokenTagStr];
                    Session[si.ServiceEndPointUri] = token;
                }
                else
                {
                    return null;
                }
            }
            si.Token = token;
            return si;
        }
        /// <summary>
        /// Main method to call Office 365 APIs
        /// </summary>
        /// <param name="service">type of the service to execute</param>
        /// <param name="postUrl">post part of the url that need to be appended to service endpoint</param>
        /// <returns>result in JSON format</returns>
        private async Task<ContentResult> CallService(string service, string postUrl)
        {
            Utils.RequestHelper helper = new Utils.RequestHelper();
            Utils.ServiceInfo si = this.GetServiceInfo(service);
            if (si == null || si.Token == null || si.ServiceResourceId == null)
            {
                return Content("Sorry!!! We couldn't fetch the token or service endpoint from Azure.");
            }
            string url = si.ServiceEndPointUri + postUrl;
            HttpResponseMessage msg = helper.PutRequest(url, si.Token);
            var resMsg = await msg.Content.ReadAsStringAsync();
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(resMsg, Newtonsoft.Json.Formatting.Indented);
            return Content(resMsg, "application/json");
        }

        public Task<ContentResult> Contacts()
        {
            return this.CallService(Utils.Service.Contacts, "/me/Contacts");
        }

        public Task<ContentResult> EMail(string id)
        {
            string postUrl = string.Format("/me/folders/{0}/messages", id);

            return this.CallService(Utils.Service.EMail, postUrl);
        }

        public Task<ContentResult> Events()
        {
            return this.CallService(Utils.Service.Calendar, "/me/events");
        }

        public Task<ContentResult> UserCalendar(string id)
        {
            return this.CallService(Utils.Service.Calendar, "/me/calendargroups");
        }

        public Task<ContentResult> ListFolderContents(string id)
        {
            string postUrl = string.Format("/files/getByPath('{0}')/children", id);
            return this.CallService(Utils.Service.MyFiles, postUrl);
        }


        public Task<ContentResult> FileProperties(string id)
        {
            string postUrl = string.Format("/files/{0}", id);
            return this.CallService(Utils.Service.MyFiles, postUrl);
        }

        public ActionResult EndPoints()
        {
            //debug only
            // get token for discovery service
            IDictionary<string, string> dict = new Dictionary<string, string>();
            string authCode = (string)Session[Constants.authCodeTagStr];
            if (string.IsNullOrEmpty(authCode))
            {
                dict.Add(Constants.errorTagStr, "An error has been encountered while accessing discovery service, Please login and then try again");
                return Json(dict, JsonRequestBehavior.AllowGet);
            }
            var results = TokenHelper.GetAccessTokenFromAuthCode_discovery(authCode, Url.Action("GetToken", "Account", null, Request.Url.Scheme));
            if (results.ContainsKey(Constants.accessTokenTagStr))
            {
                Session[Constants.discoveryServiceTokenTagStr] = results[Constants.accessTokenTagStr];
            }
            else
            {
                dict.Add(Constants.errorTagStr, "Sorry!!!, We are unable to get access token for the discovery service, sign-in again to continue");
                return Json(dict, JsonRequestBehavior.AllowGet);
            }
            string resultContent = TokenHelper.GetCapbilities((string)Session[Constants.discoveryServiceTokenTagStr]);
            if (string.IsNullOrEmpty(resultContent))
            {
                dict.Add(Constants.errorTagStr, "Sorry!!!, We are unable to get access token for the discovery capabilities, please sign out and sign-in again to continue");
                return Json(dict, JsonRequestBehavior.AllowGet);
            }
            if (resultContent.Contains("System.UnauthorizedAccessException"))
            {
                dict.Add(Constants.errorTagStr, "Sorry!!!, you dont have access to the resource.");
                return Json(dict, JsonRequestBehavior.AllowGet);
            }
            Session[Constants.capabilitiesTagStr] = resultContent;
            try
            {
                dynamic capabilitiesJobject = (JObject.Parse(resultContent))["value"];
                //dynamic valT = jbj["value"];
                for (int i = 0; i < capabilitiesJobject.Count; ++i)
                {
                    string capability = capabilitiesJobject[i].capability;
                    string serviceEndpointUri = capabilitiesJobject[i].serviceEndpointUri;
                        dict.Add(capability, serviceEndpointUri);
                }
                return Json(dict, JsonRequestBehavior.AllowGet);
            }
            catch (JsonException ex)
            {
                dict.Add(Constants.errorTagStr, ex.Message);
                return Json(dict, JsonRequestBehavior.AllowGet);
            }
        }
    }
}