using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devoffice.GettingStarted.Utils
{
    public enum Resources
    {
        Graph,
        Exchange,
        SharePoint
    };

    public enum Scopes
    {
        UserProfileRead,
        MailRead,
        MailWrite,
        MailSend,
        CalendarsRead,
        CalendarsWrite,
        ContactsRead,
        ContactsWrite,
        FilesRead,
        FilesWrite
    };

    public static class AppScopes
    {
        private static Scopes firstExchangeScope = Scopes.MailRead;
        private static Scopes firstSharePointScope = Scopes.FilesRead;

        private static Dictionary<string, Scopes> scopeFromName = new Dictionary<string, Scopes>()
        {
            { "UserProfile.Read", Scopes.UserProfileRead },
            { "Mail.Read", Scopes.MailRead },
            { "Mail.Write", Scopes.MailWrite },
            { "Mail.Send", Scopes.MailSend },
            { "Calendars.Read", Scopes.CalendarsRead },
            { "Calendars.Write", Scopes.CalendarsWrite },
            { "Contacts.Read", Scopes.ContactsRead },
            { "Contacts.Write", Scopes.ContactsWrite },
            { "Files.Read", Scopes.FilesRead },
            { "Files.Write", Scopes.FilesWrite },
        };
        
        private static Dictionary<Scopes, string> scopeIds = new Dictionary<Scopes, string>()
        {
            { Scopes.UserProfileRead, "311a71cc-e848-46a1-bdf8-97ff7156d8e6" },
            { Scopes.MailRead, "185758ba-798d-4b72-9e54-429a413a2510" },
            { Scopes.MailWrite, "75767999-c7a8-481e-a6b4-19458e0b30a5" },
            { Scopes.MailSend, "5eb43c10-865a-4259-960a-83946678f8dd" },
            { Scopes.CalendarsRead, "5b9be81f-2977-4d27-8faf-bb43af8fc705" },
            { Scopes.CalendarsWrite, "765f423e-b55d-412e-97e3-13a800c3a537" },
            { Scopes.ContactsRead, "181aac24-028a-486e-a649-b3742c74ec71" },
            { Scopes.ContactsWrite, "32253599-e142-4cf0-810d-4827eedd1cfa" },
            { Scopes.FilesRead, "dd2c8d78-58e1-46d7-82dd-34d411282686" },
            { Scopes.FilesWrite, "2cfdc887-d7b4-4798-9b33-3d98d6b95dd2" },
        };

        private static Dictionary<Resources, string> resourceIds = new Dictionary<Resources, string>()
        {
            { Resources.Graph, "00000002-0000-0000-c000-000000000000" },
            { Resources.Exchange, "00000002-0000-0ff1-ce00-000000000000" },
            { Resources.SharePoint, "00000003-0000-0ff1-ce00-000000000000" },
        };

        public static Resources GetScopeResource(Scopes scope)
        {
            if (scope < firstExchangeScope)
                return Resources.Graph;
            else if (scope < firstSharePointScope)
                return Resources.Exchange;
            return Resources.SharePoint;
        }

        public static List<Scopes> GetApplicableScopes(string[] scopeNames)
        {
            List<Scopes> applicableScopes = new List<Scopes>() { Scopes.UserProfileRead };
            foreach (string name in scopeNames)
            {
                if (!applicableScopes.Contains(scopeFromName[name]))
                    applicableScopes.Add(scopeFromName[name]);
            }

            // Remove redundant scopes
            if (applicableScopes.Contains(Scopes.MailWrite))
                applicableScopes.Remove(Scopes.MailRead);

            if (applicableScopes.Contains(Scopes.CalendarsWrite))
                applicableScopes.Remove(Scopes.CalendarsRead);

            if (applicableScopes.Contains(Scopes.ContactsWrite))
                applicableScopes.Remove(Scopes.ContactsRead);

            return applicableScopes;
        }

        public static string GetScopeId(Scopes scope)
        {
            return scopeIds[scope];
        }

        public static string GetResourceId(Resources resource)
        {
            return resourceIds[resource];
        }
    }
}
