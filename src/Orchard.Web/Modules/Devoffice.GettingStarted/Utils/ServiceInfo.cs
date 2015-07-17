using System;

namespace Devoffice.GettingStarted.Utils
{
    public class Service
    {
        public static readonly string Contacts = "Contacts";
        public static readonly string Calendar = "Calendar";
        public static readonly string EMail = "Mail";
        public static readonly string MyFiles = "MyFiles";

        private static string [] all =  {Contacts, Calendar,EMail,MyFiles};
        public static string[] AllServices { get { return all; } }
    }

    public class ServiceInfo
    {
        public string Token { get; set; }

        public string ServiceEndPointUri { get; set; }

        public string ServiceResourceId { get; set; }
    }
}