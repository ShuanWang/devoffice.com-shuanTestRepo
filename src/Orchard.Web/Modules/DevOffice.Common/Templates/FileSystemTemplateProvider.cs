using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Orchard;

namespace DevOffice.Common.Templates
{
    public interface IHtmlTemplateProvider
    {
        string GetTemplateContent(string templateName);
    }

    public class FileSystemTemplateProvider : IHtmlTemplateProvider
    {
        private const string _templateDirectory = "~/Modules/DevOffice.Common/Templates/{0}.html";

        public string GetTemplateContent(string templateName)
        {
            var path = string.Format(_templateDirectory, templateName);
            var mappedPath = HttpContext.Current.Server.MapPath(path);
            return File.ReadAllText(mappedPath);
        }
    }
}