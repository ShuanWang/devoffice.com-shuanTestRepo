using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Devoffice.GettingStarted.Models
{
    public class RegisterAppModel
    {
        public RegisterAppModel()
        {
            this.appName = "Hello-O365";
            this.appType = "Web App";
            this.signOnUri = "http://localhost:8000";
            this.appIdUri = "http://hello-O365";
            this.redirectUri = "http://localhost:8000";
            this.appId = null;
        }

        public string appName { get; set; }
        public string appType { get; set; }
        public string signOnUri { get; set; }
        public string appIdUri { get; set; }
        public string redirectUri { get; set; }

        public bool includeCalendar { get; set; }
        public bool includeContacts { get; set; }
        public bool includeMail { get; set; }
        public bool includeFiles { get; set; }

        public bool includeCalendarWrite { get; set; }
        public bool includeContactsWrite { get; set; }
        public bool includeMailWrite { get; set; }
        public bool includeFilesWrite { get; set; }
        public bool includeMailSend { get; set; }

        public string appId { get; set; }

    }
}