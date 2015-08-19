//----------------------------------------------------------------------------------------------
//    Copyright 2014 Microsoft Corporation
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//----------------------------------------------------------------------------------------------

using System;
using System.Configuration;

namespace Devoffice.GettingStarted.Utils
{
    public class SettingsHelper
    {
#if DEBUG
        private static string _clientId = "da35b9a6-bbd1-4ad4-b07c-2950efa25a1e";
        private static string _appKey = "MHA9YluTuhaqslTFnTCyEFw7OYKIaEdtbzwX9hzvd4c=";
#else
        private static string _clientId = "d9c2a6e1-fd01-4da5-b9fe-be93ca502042";
        private static string _appKey = "GJZOs7JVI0HvkhOLfNdhdyOuMUzCGcJ6m1KrEnrDyII=";
#endif
        private static string _authorizationUri = "https://login.microsoftonline.com/common/oauth2/authorize";

        private static string _graphResourceId = "https://graph.windows.net";
        private static string _discoverySvcResourceId = "https://api.office.com/discovery/";
        private static string _discoverySvcEndpointUri = "https://api.office.com/discovery/v1.0/me/";

        public static string ClientId
        {
            get
            {
                return _clientId;
            }
        }

        public static string AppKey
        {
            get
            {
                return _appKey;
            }
        }

        public static string AuthorizationUri
        {
            get
            {
                return _authorizationUri;
            }
        }


        public static string AADGraphResourceId
        {
            get
            {
                return _graphResourceId;
            }
        }

        public static string DiscoveryServiceResourceId
        {
            get
            {
                return _discoverySvcResourceId;
            }
        }

        public static Uri DiscoveryServiceEndpointUri
        {
            get
            {
                return new Uri(_discoverySvcEndpointUri);
            }
        }
    }
}
