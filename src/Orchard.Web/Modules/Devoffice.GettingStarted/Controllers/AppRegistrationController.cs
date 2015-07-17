using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Devoffice.GettingStarted.Models;
using Devoffice.GettingStarted.Utils;
namespace Devoffice.GettingStarted.Controllers
{
    public class AppRegistrationController : Controller
    {

        public ActionResult RegisterApp(Models.RegisterAppModel appInfo)
        {
            //For Debug Purpose only
            /*
            Dictionary<string, string> registrationInfo = new Dictionary<string, string>();
            registrationInfo.Add(Constants.clientIdTagStr, "client_id_goes_here");
            registrationInfo.Add("client_secret", "secret_goes_here");
            return Json(registrationInfo, JsonRequestBehavior.AllowGet);
            */

            if (appInfo == null)
            {
                throw new HttpException("Error, please provide app info");
            }
            if ((bool)Session[Constants.userLoggedInStr] != true)
            {
                return null;
            }
            string token = (string)Session[Constants.accessTokenTagStr];
            string tenantid = (string)Session[Constants.azureUserTenantIdTagStr];


            string appName = appInfo.appName;
            string signOnUri = appInfo.signOnUri;
            string appIdUri = appInfo.appIdUri;
            string redirectUri = appInfo.redirectUri;
            List<Utils.Scopes> appScopes = new List<Utils.Scopes>() { Utils.Scopes.UserProfileRead };
            //TBD it is not the right way to include app scope
            if (appInfo.includeCalendar)
                appScopes.Add(Utils.Scopes.CalendarsRead);
            if (appInfo.includeContacts)
                appScopes.Add(Utils.Scopes.ContactsRead);
            if (appInfo.includeMail)
                appScopes.Add(Utils.Scopes.MailRead);
            if (appInfo.includeFiles)
                appScopes.Add(Utils.Scopes.FilesRead);

            if (appInfo.includeCalendarWrite)
                appScopes.Add(Utils.Scopes.CalendarsWrite);
            if (appInfo.includeContactsWrite)
                appScopes.Add(Utils.Scopes.ContactsWrite);
            if (appInfo.includeMailWrite)
                appScopes.Add(Utils.Scopes.MailWrite);
            if (appInfo.includeFilesWrite)
                appScopes.Add(Utils.Scopes.FilesWrite);
            if (appInfo.includeMailSend)
                appScopes.Add(Utils.Scopes.MailSend);

            if (string.Equals("Web App", appInfo.appType, StringComparison.InvariantCultureIgnoreCase) == true)
            {
                var results = Utils.AppRegistration.CreateWebAppRegistration(token,
                    tenantid,
                    appName, signOnUri, appIdUri, redirectUri, false, false, appScopes);
                return Json(results, JsonRequestBehavior.AllowGet);
            }
            else if (string.Equals("Native App", appInfo.appType, StringComparison.InvariantCultureIgnoreCase) == true)
            {
                var results = Utils.AppRegistration.CreateNativeAppRegistration(token,
                    tenantid,
                    appName, redirectUri, appScopes);
                return Json(results, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var dict = new Dictionary<string, string>();
                dict.Add(Constants.errorTagStr, "Unknown app type has been passed");
                return Json(dict, JsonRequestBehavior.AllowGet);
            }
        }

    }
}