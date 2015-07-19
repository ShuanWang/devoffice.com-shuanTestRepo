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
            #region For Debug Purpose only
                        /*
                        Dictionary<string, string> registrationInfo = new Dictionary<string, string>();
                        registrationInfo.Add(Constants.clientIdTagStr, "client_id_goes_here");
                        registrationInfo.Add("client_secret", "secret_goes_here");
                        return Json(registrationInfo, JsonRequestBehavior.AllowGet);
                        */
            #endregion

            #region Do error check
            Dictionary<string, string> errorInfo = new Dictionary<string, string>();

            if (appInfo == null)
            {
                errorInfo.Add(Utils.Constants.errorMessageTagStr, "App information can not be null");
                return Json(errorInfo, JsonRequestBehavior.AllowGet);
            }
            if (Session[Constants.userLoggedInStr] == null || (bool)Session[Constants.userLoggedInStr] != true)
            {
                errorInfo.Add(Utils.Constants.errorMessageTagStr, "Your session has been expired, please sign out and sign in again");
                return Json(errorInfo, JsonRequestBehavior.AllowGet);
            }
            #endregion

            #region Reading variables
            Dictionary<string, string> results = null;
            string token = (string)Session[Constants.accessTokenTagStr];
            string tenantid = (string)Session[Constants.azureUserTenantIdTagStr];


            string appName = appInfo.appName;
            string signOnUri = appInfo.signOnUri;
            string appIdUri = appInfo.appIdUri;
            // append a guid in appid uri
            if(appIdUri[appIdUri.Length-1] != '/')
            {
                appIdUri += "/";
            }
            appIdUri += Guid.NewGuid().ToString();
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

            #endregion

            #region Web App registration
            if (string.Equals("Web App", appInfo.appType, StringComparison.InvariantCultureIgnoreCase) == true)
            {
                if (string.IsNullOrEmpty(appInfo.appId))
                { /* Create new app*/
                    results = Utils.AppRegistration.CreateWebAppRegistration(token,
                                    tenantid,
                                    appName, signOnUri, appIdUri, redirectUri, false, false, appScopes);
                }
                else { /* update the existing app*/
                    results = Utils.AppRegistration.UpdateWebAppRegistration(token,
                                    tenantid,
                                    appName, signOnUri, appIdUri, redirectUri, false, false, appScopes, appInfo.appId);
                }

                return Json(results, JsonRequestBehavior.AllowGet);
            }
            #endregion

            #region Native App registration
            else if (string.Equals("Native App", appInfo.appType, StringComparison.InvariantCultureIgnoreCase) == true)
            {
                if (string.IsNullOrEmpty(appInfo.appId))
                { /* Create new app*/
                    results = Utils.AppRegistration.CreateNativeAppRegistration(token,
                        tenantid, appName, redirectUri, appScopes);
                }
                else
                { /* update the existing app*/
                    results = Utils.AppRegistration.UpdateNativeAppRegistration(token,
                                    tenantid, appName, redirectUri, appScopes, appInfo.appId);
                }
                return Json(results, JsonRequestBehavior.AllowGet);
            }
            #endregion

            #region Unknown Type
            else
            {
                var dict = new Dictionary<string, string>();
                dict.Add(Constants.errorTagStr, "Unknown app type has been passed");
                return Json(dict, JsonRequestBehavior.AllowGet);
            }
            #endregion

        }


    }
}