using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Devoffice.GettingStarted.Models
{
    public class CodeSampleModel
    {
        public CodeSampleModel(string name, string description, string downloadLink, string platform, string app)
        {
            Name = name;
            Description = description;
            DownloadLink = downloadLink;
            Platform = platform;
            App = app;
        }
        public string Name { get; set; }

        public string Description { get; set; }

        public string DownloadLink { get; set; }

        public string Platform { get; set; }

        public string App { get; set; }
    }

    public class CodeSampleModelManager
    {
        private static CodeSampleModelManager _instance = null;

        private CodeSampleModelManager() {
            init();
        }

        public static CodeSampleModelManager Instance {
            get 
            {
                if (_instance == null)
                {
                    if (_instance == null)
                    {
                        _instance = new CodeSampleModelManager();
                    }
                }
                return _instance;
            }
        }
        private static List<CodeSampleModel> collection = new List<CodeSampleModel>();
        private static void init()
        {

            collection.Add(new CodeSampleModel(
                    /* Name->*/ "Starter iOS-Name",
                    /* Description->*/"this is  a sample code-Description",
                    /* Download link-> */"download link-http://abc.comd/def", 
                    /* Platform ->*/ Utils.Constants.Platform.ios,
                    /* App ->*/Utils.Constants.OfficeApps.word));

            collection.Add(new CodeSampleModel(
                /* Name->*/ "Starter iOS-Name",
                /* Description->*/"this is  a sample code-Description",
                /* Download link-> */"download link-http://abc.comd/def",
                /* Platform ->*/ Utils.Constants.Platform.ios,
                /* App ->*/Utils.Constants.OfficeApps.word));

            collection.Add(new CodeSampleModel(
                /* Name->*/ "Starter iOS-Name",
                /* Description->*/"this is  a sample code-Description",
                /* Download link-> */"download link-http://abc.comd/def",
                /* Platform ->*/ Utils.Constants.Platform.ios,
                /* App ->*/Utils.Constants.OfficeApps.excel));

            collection.Add(new CodeSampleModel(
                /* Name->*/ "Starter iOS-Name",
                /* Description->*/"this is  a sample code-Description",
                /* Download link-> */"download link-http://abc.comd/def",
                /* Platform ->*/ Utils.Constants.Platform.ios,
                /* App ->*/Utils.Constants.OfficeApps.ppt));

            collection.Add(new CodeSampleModel(
                /* Name->*/ "Starter iOS-Name",
                /* Description->*/"this is  a sample code-Description",
                /* Download link-> */"download link-http://abc.comd/def",
                /* Platform ->*/ Utils.Constants.Platform.android,
                /* App ->*/Utils.Constants.OfficeApps.word));

        }
        public List<CodeSampleModel> GetList(string key)
        {
            List<CodeSampleModel> result = new List<CodeSampleModel>();
            foreach (CodeSampleModel model in collection)
            {
                if (string.Equals(model.Platform, key) == true || string.Equals(model.App, key) == true)
                {
                    result.Add(model);
                }
            }
            return result;

        }
    }
}