using Devoffice.GettingStarted.Utils;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

using Devoffice.GettingStarted.Models;

namespace Devoffice.GettingStarted.Utils
{
    public class RequestHelper
    {
        public HttpResponseMessage PutRequest(string url, string authToken)
        {
            using(var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
                var response = client.SendAsync(request).Result;
                return response;
            }
        }
    }
}