using Devoffice.GettingStarted.Utils;
using System.Web;
using System.Web.Mvc;
using System;

namespace Devoffice.GettingStarted.Controllers
{
    [HandleError]
    public class AccountController : Controller
    {
        #region private methods

        private Uri GetAuthorizationUri(string resource, string state, string redirectUri)
        {
            UriBuilder authorizeRequest = new UriBuilder(Utils.SettingsHelper.AuthorizationUri);

            authorizeRequest.Query = "response_type=code" +
                "&client_id=" + SettingsHelper.ClientId+
                "&redirect_uri=" + redirectUri +
                "&resource=" + HttpUtility.UrlEncode(resource) +
                "&state=" + state +
                "&prompt=login";

            return authorizeRequest.Uri;
        }

        private string AppendQueryString(string queryString, string appendValue)
        {
            return string.IsNullOrEmpty(queryString) ? appendValue : queryString + "&" + appendValue;
        }

        private void ClearSession()
        {
            try
            {
                Session.Remove(Constants.accessTokenTagStr);
                Session.Remove(Constants.refreshTokenTagStr);
                Session.Remove(Constants.authCodeTagStr);
                Session.Remove(Constants.azureUserTagStr);
                Session.Remove(Constants.azureUserEmailTagStr);
                Session.Remove(Constants.azureUserTenantIdTagStr);
                Session.Remove(Constants.discoveryServiceTokenTagStr);
                Session.Remove(Constants.azureUserTagStr);
                Session.Remove(Constants.stateTagStr);
                Session.Remove(Constants.userLoggedInStr);
                Session.Remove(Constants.platformNameTagStr);
                Session.Remove(Constants.appNameTagStr);
            }
            catch (Exception)
            {
                // we dont  need to d do anything if deleting session fails
            }
        }

        public Uri GetLogoutUri(string redirectUri)
        {
            UriBuilder logoutUri = new UriBuilder(Constants.LogoutUri);

            logoutUri.Query = "post_logout_redirect_uri=" + HttpUtility.UrlEncode(redirectUri);

            return logoutUri.Uri;
        }
        #endregion

        public ActionResult SignIn()
        {
            string state = Guid.NewGuid().ToString();
            Session[Utils.Constants.stateTagStr] = state;

            UriBuilder redirectUri = new UriBuilder(Url.Action("GetToken", "Account", null, "https"));
            Uri authUri = this.GetAuthorizationUri("https://graph.windows.net/", state, redirectUri.Uri.ToString());

            RedirectResult redirect = new RedirectResult(authUri.ToString());

            return redirect;
        }

        public ActionResult SignOut()
        {
            this.ClearSession();

            //TBD: once we log out we need to fallback to http protocol
            UriBuilder returnUri = new UriBuilder(Request.Url.Scheme, Request.Url.Host, Request.Url.Port,
                "/Getting-Started/office365Apis");
            if (Request.Cookies["current-card"] != null)
            {
                string returnUrl = returnUri.ToString();
                returnUrl += ("#" + Request.Cookies["current-card"].Value);
                return new RedirectResult(returnUrl);
            }
            return new RedirectResult(returnUri.ToString());
        }
        public ActionResult GetToken()
        {
            string authCode = Request.Params["code"];
            string state = Request.Params["state"];
            Session[Constants.authCodeTagStr] = authCode;

            string queryString = string.Empty;
            if (string.IsNullOrEmpty(authCode))
            {
                // Check for error
                string errorMsg = Request.Params["error_description"];
                if (string.IsNullOrEmpty(errorMsg))
                {
                    errorMsg = "Azure did not send an authorization code. Please try to sign in again.";
                }

                queryString = AppendQueryString(queryString, "error_msg=" + HttpUtility.UrlEncode(errorMsg));
            }
            else
            {
                if (state != (string)Session[Utils.Constants.stateTagStr])
                {
                    queryString = AppendQueryString(queryString, "error_msg=" + HttpUtility.UrlEncode("The session state returned by Azure was not the expected value."));
                }
                else
                {
                    var loginResults = TokenHelper.GetAccessTokenFromAuthCode(authCode, Url.Action("GetToken", "Account", routeValues: new { area = "Devoffice.GettingStarted" }, protocol:Request.Url.Scheme));

                    if (loginResults.ContainsKey(Constants.accessTokenTagStr))
                    {
                        Session[Constants.userLoggedInStr] = true;
                        // save all the data in session
                        Session[Utils.Constants.accessTokenTagStr] = loginResults[Utils.Constants.accessTokenTagStr];
                        Session[Utils.Constants.refreshTokenTagStr] = loginResults[Utils.Constants.refreshTokenTagStr];
                        Session[Utils.Constants.azureUserTagStr] = loginResults[Utils.Constants.azureUserTagStr];
                        Session[Utils.Constants.azureUserEmailTagStr] = loginResults[Utils.Constants.azureUserEmailTagStr];
                        Session[Utils.Constants.azureUserTenantIdTagStr] = loginResults[Utils.Constants.azureUserTenantIdTagStr];
                    }
                    else if (loginResults.ContainsKey("error"))
                    {
                        queryString = AppendQueryString(queryString, "error_msg=" + HttpUtility.UrlEncode(loginResults["error_description"]));
                    }
                    else
                    {
                        queryString = AppendQueryString(queryString, "error_msg=" + HttpUtility.UrlEncode("Something went wrong logging on. Please try again."));
                    }

                }
            }

            UriBuilder returnUri = new UriBuilder(Request.Url.Scheme, Request.Url.Host, Request.Url.Port,
                "/Getting-Started/office365Apis");
            if (string.IsNullOrEmpty(queryString))
            {
                if (Request.Cookies["current-card"] != null)
                {
                    string returnUrl = returnUri.ToString();
                    returnUrl += ("#" + Request.Cookies["current-card"].Value);
                    return new RedirectResult(returnUrl);
                }
            }
            
            returnUri.Query = queryString;

            return new RedirectResult(returnUri.ToString());
        }

        public bool IsAuthenticated()
        {
            if (Session[Constants.userLoggedInStr] != null)
            {
                bool login = (bool)Session[Constants.userLoggedInStr];
                return (login == true);
            }
            return false;
        }
    }
}