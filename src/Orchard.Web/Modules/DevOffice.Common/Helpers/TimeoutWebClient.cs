using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace DevOffice.Common.Services
{
    public class TimeoutWebClient : WebClient
    {
        public int Timeout { get; set; }

        public TimeoutWebClient()
        {
            Timeout = 60000;
        }

        public TimeoutWebClient(int timeout)
        {
            Timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            request.Timeout = Timeout;
            return request;
        }
    }
}