using System;
using System.Collections.Generic;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
//HACK using System.Runtime.Serialization.Json;

namespace Devoffice.GettingStarted.Utils
{
    public class TokenHelper
    {
        public static Dictionary<string, string> GetAccessTokenFromAuthCode(string authCode, string redirectUri)
        {
            using (var client = new HttpClient())
            {
                var payload = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("code", authCode),
                    new KeyValuePair<string, string>("redirect_uri", redirectUri),
                    new KeyValuePair<string, string>("client_id", SettingsHelper.ClientId),
                    new KeyValuePair<string, string>("client_secret", SettingsHelper.AppKey)
                });

                var request = new HttpRequestMessage(HttpMethod.Post, Constants.TokenUri);
                request.Headers.UserAgent.Add(new ProductInfoHeaderValue("DevOfficePortal", "1.0"));
                request.Content = payload;

                var result = client.SendAsync(request).Result;
                var resultContent = result.Content.ReadAsStringAsync().Result;
                var returnVal = new Dictionary<string, string>();

                try
                {
                    dynamic tokens = JsonConvert.DeserializeObject(resultContent);

                    // Tokens may contain tokens, but may contain errors.
                    if (tokens[Constants.errorTagStr] != null)
                    {
                        returnVal[Constants.errorTagStr] = tokens[Constants.errorTagStr].Value;
                        returnVal[Constants.errorDescrptionTagStr] = tokens[Constants.errorDescrptionTagStr].Value;
                    }
                    else
                    {
                        returnVal[Constants.accessTokenTagStr] = tokens["access_token"].Value;
                        returnVal[Constants.refreshTokenTagStr] = tokens["refresh_token"].Value;
                        var userInfo = GetUserInfoFromIdToken(tokens["id_token"].Value);
                        if(!string.IsNullOrEmpty(userInfo[Constants.errorMessageTagStr]))
                        {
                            returnVal[Constants.errorTagStr] = "Error while retrieving user info";
                            returnVal[Constants.errorDescrptionTagStr] = userInfo[Constants.errorMessageTagStr];
                        }
                        returnVal[Constants.azureUserTagStr] = userInfo["name"];
                        returnVal[Constants.azureUserEmailTagStr] = userInfo["email"];
                        returnVal[Constants.azureUserTenantIdTagStr] = userInfo["tenant"];
                    }

                    return returnVal;
                }
                catch (JsonException ex)
                {
                    returnVal[Constants.errorTagStr] = "An error occurred while making request";
                    returnVal[Constants.errorDescrptionTagStr] = ex.Message;
                    return returnVal;
                }
            }
        }

        private static Dictionary<string, string> GetUserInfoFromIdToken(string idToken)
        {
            Dictionary<string, string> info = new Dictionary<string, string>() 
            {
                {"name", ""},
                {"email", ""},
                {"tenant", ""},
                {Constants.errorMessageTagStr, ""},
            };

            string encodedToken = idToken.Split('.')[1];

            string parsedToken = Base64UrlDecoder.Decode(encodedToken);

            try
            {
                dynamic token = JsonConvert.DeserializeObject(parsedToken);
                info["name"] = token["name"];
                info["email"] = token["upn"];
                info["tenant"] = token["tid"];
            }
            catch (JsonException ex)
            {
                info[Constants.errorDescrptionTagStr] = ex.Message;
            }

            return info;
        }

        public static Dictionary<string, string> GetAccessTokenFromAuthCode_discovery(string authCode, string redirectUri)
        {
            using (var client = new HttpClient())
            {
                var payload = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("code", authCode),
                    new KeyValuePair<string, string>("redirect_uri", redirectUri),
                    new KeyValuePair<string, string>("client_id", SettingsHelper.ClientId),
                    new KeyValuePair<string, string>("client_secret", SettingsHelper.AppKey),
                    new KeyValuePair<string, string>("resource", Constants.discoveryServiceResourceId)
                });

                var request = new HttpRequestMessage(HttpMethod.Post, Constants.TokenUri);
                request.Headers.UserAgent.Add(new ProductInfoHeaderValue("DevOfficePortal", "1.0"));
                request.Content = payload;

                var result = client.SendAsync(request).Result;
                var resultContent = result.Content.ReadAsStringAsync().Result;
                var returnVal = new Dictionary<string, string>();

                try
                {
                    dynamic tokens = JsonConvert.DeserializeObject(resultContent);

                    // Tokens may contain tokens, but may contain errors.
                    if (tokens[Constants.errorTagStr] != null)
                    {
                        returnVal[Constants.errorTagStr] = tokens[Constants.errorTagStr].Value;
                        returnVal[Constants.errorDescrptionTagStr] = tokens[Constants.errorDescrptionTagStr].Value;
                    }
                    else
                    {
                        returnVal[Constants.accessTokenTagStr] = tokens["access_token"].Value;
                    }

                    return returnVal;
                }
                catch (JsonException ex)
                {
                    returnVal[Constants.errorTagStr] = "An error occurred while making request";
                    returnVal[Constants.errorDescrptionTagStr] = ex.Message;
                    return returnVal;
                }
            }
        }
        public static string GetCapbilities(string accessToken)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, Constants.discoveryServiceEndPoint);
                request.Headers.UserAgent.Add(new ProductInfoHeaderValue("DevOfficePortal", "1.0"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = client.SendAsync(request).Result;
                var resultContent = result.Content.ReadAsStringAsync().Result;

                return resultContent;
            }

        }

        public static Dictionary<string, string> GetDiscoveryServiceToken(string authCode, string redirectUri,
            string tokenProviderUri, string resourceId)
        {
            using (var client = new HttpClient())
            {
                var payload = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("code", authCode),
                    new KeyValuePair<string, string>("redirect_uri", redirectUri),
                    new KeyValuePair<string, string>("client_id", SettingsHelper.ClientId),
                    new KeyValuePair<string, string>("client_secret", SettingsHelper.AppKey),
                    new KeyValuePair<string, string>("resource", resourceId)
                });

                var request = new HttpRequestMessage(HttpMethod.Post, tokenProviderUri);
                request.Headers.UserAgent.Add(new ProductInfoHeaderValue("DevOfficePortal", "1.0"));
                request.Content = payload;

                var result = client.SendAsync(request).Result;
                var resultContent = result.Content.ReadAsStringAsync().Result;
                var returnVal = new Dictionary<string, string>();

                try
                {
                    dynamic tokens = JsonConvert.DeserializeObject(resultContent);

                    // Tokens may contain tokens, but may contain errors.
                    if (tokens[Constants.errorTagStr] != null)
                    {
                        returnVal[Constants.errorTagStr] = tokens[Constants.errorTagStr].Value;
                        returnVal[Constants.errorDescrptionTagStr] = tokens[Constants.errorDescrptionTagStr].Value;
                    }
                    else
                    {
                        returnVal[Constants.accessTokenTagStr] = tokens["access_token"].Value;
                    }

                    return returnVal;
                }
                catch (JsonException ex)
                {
                    returnVal[Constants.errorTagStr] = "An error occurred while making request";
                    returnVal[Constants.errorDescrptionTagStr] = ex.Message;
                    return returnVal;
                }
            }
        }

    }
}