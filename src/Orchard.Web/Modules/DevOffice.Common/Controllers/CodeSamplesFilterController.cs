using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using DevOffice.Common.Services;
using DevOffice.Common.ViewModels;
using HtmlAgilityPack;
using Orchard.ContentManagement;
using Orchard.Taxonomies.Services;
using Orchard.Themes;
using Orchard.Utility.Extensions;
using GitHubViewModel = DevOffice.Common.ViewModels.GitHubViewModel;
using Orchard.Logging;

namespace DevOffice.Common.Controllers
{
    [ValidateInput(false), Themed]
    public class CodeSamplesFilterController: Controller
    {
        private readonly ICommonDataService _commonDataService;
        private readonly ITaxonomyService _taxonomyService;
        private readonly IContentManager _contentManager;
        public ILogger Logger { get; set; }

        public CodeSamplesFilterController(ICommonDataService commonDataService, ITaxonomyService taxonomyService, IContentManager contentManager)
        {
            _commonDataService = commonDataService;
            _taxonomyService = taxonomyService;
            _contentManager = contentManager;
            Logger = NullLogger.Instance;
        }

        public ActionResult GitHubCodeSamplesDetailPage(int id) {
            dynamic codeSample = _contentManager.Get(id);
            
            string externalUrl = codeSample.CodeSamplePart.ExternalLink.Value;
            
             var model = new GitHubViewModel {
                Title = codeSample.TitlePart.Title,
                Icon = GetMediaUrl(codeSample.CodeSamplePart.Image),
                Content = codeSample.CodeSamplePart.SubText.Value,
                HTMLData = GetGitHubMarkDown(externalUrl, "code-sample"),
                ExternalLink = externalUrl,
                ContentItemId = codeSample.CodeSamplePart.Id
            };

             return View("CodeSamplesFilter/GitHubCodeSamplesDetailPage", model);
        }

        public ActionResult GitHubPatternsAndPracticesDetailPage(int id) {
            dynamic codeSample = _contentManager.Get(id);

            string externalUrl = codeSample.PatternsAndPracticesPart.ExternalLink.Value;
            
             var model = new GitHubViewModel {
                Title = codeSample.TitlePart.Title,
                Icon = GetMediaUrl(codeSample.PatternsAndPracticesPart.Icon),
                Content = codeSample.PatternsAndPracticesPart.SubText.Value,
                HTMLData = GetGitHubMarkDown(externalUrl, "patterns-and-practices"),
                ExternalLink = externalUrl,
                ContentItemId = codeSample.PatternsAndPracticesPart.Id
            };

             return View("CodeSamplesFilter/GitHubPatternsAndPracticesDetailPage", model);
        }

        public ActionResult GitHubHandsOnLabsDetailPage(int id) {
            string externalUrl = "";
            dynamic trainingItem = _contentManager.Get(id);
            var relatedLinks = trainingItem.RelatedLinksPart.Links;
            foreach (var link in relatedLinks) {
                if (link.Type == "handsOnLab") {
                    externalUrl = link.Url;
                }
            }

            var model = new GitHubViewModel
            {
                Title = trainingItem.TitlePart.Title,
                Icon = GetMediaUrl(trainingItem.TrainingPart.Image),
                Content = trainingItem.TrainingPart.SubText.Value,
                HTMLData = GetGitHubMarkDown(externalUrl, "training"),
                ExternalLink = externalUrl,
                ContentItemId = trainingItem.TrainingPart.Id
            };

            return View("CodeSamplesFilter/GitHubHandsOnLabDetailPage", model);
        }
      

        private string GetGitHubMarkDown(string externalUrl, string itemType)
        {
            var htmlData = "";

                //Additional code to deal with existing GitHub URL's in the content. Ex: https://github.com/OfficeDev/Learning-Path-Manager-Code-Sample
                externalUrl = externalUrl.Trim();
                var gitHubUrlTokens = externalUrl.Replace("https://", "").Split('/');

            var userAgent = gitHubUrlTokens[1];

                var repoUrl = "";
                if (gitHubUrlTokens.Length > 1)
                {
                    repoUrl = string.Join("/", gitHubUrlTokens.Skip(1));
                }

                //GitHub url to request readme.md file content
                var githubUrl = "";
                if (itemType == "code-sample" ) { githubUrl = externalUrl.Replace("https://github.com/", "https://api.github.com/repos/"); }
                if (itemType == "patterns-and-practices") {
                    githubUrl = externalUrl.Replace("https://github.com/", "https://api.github.com/repos/");

                    if (externalUrl.Contains("tree/master"))
                    {
                        githubUrl = githubUrl.Replace("tree/master", "contents");
                        githubUrl = CheckUrlHasReadMe(githubUrl);
                    }
                    else
                    {
                        githubUrl = githubUrl.Replace("blob/master", "contents");
                        githubUrl = CheckUrlHasReadMe(githubUrl);
                    }
                
                }
             
                if (itemType == "training") { githubUrl = externalUrl.Replace("https://github.com/", "https://api.github.com/repos/").Replace("blob/master", "contents").Replace("blob/",""); }

                

                if (githubUrl.EndsWith("/")) {
                    githubUrl = githubUrl.TrimEnd('/');
                }

                //Adding auth tokens so there is no restriction on number of requests - https://developer.github.com/v3/#authentication
                const string tokens = "?client_id=46382d277233909e5d58&client_secret=2ccbfd543f6a5d4428ca0681d485a7115eda95c1";
                var urlLowercase = "";
                var urlUppercase = "";

                if (itemType == "code-sample")
                {
                    urlLowercase = githubUrl + "/readme" + tokens;
                }
                else
                {
                    urlLowercase = githubUrl + tokens;
                    urlUppercase = githubUrl + tokens;
                }

                var readmeUrls = new List<string> { urlLowercase, urlUppercase };
                //var readmeUrls = new List<string> { githubUrl + "readme" };

                var html = "";
                foreach (var readmeUrl in readmeUrls)
                {

                    try
                    {
                        var client = new TimeoutWebClient { Encoding = Encoding.UTF8, Timeout = 6000}; 
                        client.Headers[HttpRequestHeader.Accept] = "application/vnd.github.VERSION.html";
                        client.Headers[HttpRequestHeader.UserAgent] = userAgent;
                        html = client.DownloadString(readmeUrl);
                        client.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex, "An unexpected error occurred at GetGithubMarkdown");
                    }

                }
                if (string.IsNullOrEmpty(html))
                    Response.Redirect(externalUrl);

                htmlData = FixRelativePaths(html, repoUrl, itemType);


            return htmlData;

        }

        private static string CheckUrlHasReadMe(string githubUrl)
        {
            if (githubUrl.EndsWith("/"))
            {
                githubUrl = githubUrl.TrimEnd('/');
            }
            if (!githubUrl.ToLower().EndsWith("readme.md"))
            {
                githubUrl = githubUrl + "/readme.md";
            }
            return githubUrl;
        }


        private string FixRelativePaths(string html, string repoUrl, string itemType) {
            var htmlDoc = new HtmlDocument();
            string readmeContent = html;
            try
            {
                htmlDoc.LoadHtml(html);
                foreach (var srcNode in htmlDoc.DocumentNode.SelectNodes("//img/@src")) {
                    if (string.IsNullOrEmpty(srcNode.Attributes["src"].Value)) {
                        continue;
                    }
                    var hrefPath = srcNode.Attributes["src"];
                    if (hrefPath.Value.StartsWith("http") || hrefPath.Value.StartsWith("https")){
                        continue;
                    }

                    var replaceString = string.Format("{0}/{1}/master/", "https://raw.githubusercontent.com", repoUrl);
                    if (itemType == "training") {
                        replaceString = replaceString.Replace("/Lab.md", "").Replace("/blob", "");
                        if (replaceString.EndsWith("master/")) { replaceString = replaceString.Substring(0, replaceString.Length - 7); }
                    }
                    var absoluteSource = replaceString + srcNode.Attributes["src"].Value;
                    srcNode.SetAttributeValue("src", absoluteSource);
                }
             
                foreach (var link in htmlDoc.DocumentNode.SelectNodes("//a[@href]")) {
                    if (string.IsNullOrEmpty(link.Attributes["href"].Value)) {
                        continue;
                    }
                    var hrefPath = link.Attributes["href"];
                    if (hrefPath.Value.StartsWith("http") || hrefPath.Value.StartsWith("https") || hrefPath.Value.StartsWith("#"))
                    {
                        continue;
                    }

                    if (link.SelectNodes("img") != null) {
                       var node = link.SelectSingleNode("//img/@src");
                        link.SetAttributeValue("href", HtmlNode.CreateNode(link.InnerHtml).Attributes["src"].Value);
                       
                        //link.ParentNode.ReplaceChild(newNode,link);
                        //link.SetAttributeValue("href", "");
                        continue;
                    }
                    var replaceUrlString = repoUrl.Contains("blob/master") ? string.Format("{0}/{1}/", "https://github.com", repoUrl.Replace("/Lab.md", string.Empty)) : string.Format("{0}/{1}/blob/master/", "https://github.com", repoUrl); 
                    var absoluteUrl = replaceUrlString + link.Attributes["href"].Value;
                    link.SetAttributeValue("href", absoluteUrl);
                }
                readmeContent = htmlDoc.DocumentNode.InnerHtml;
            }
            catch (Exception ex)
            {

            }
            return readmeContent;
        }

        private string GetMediaUrl(dynamic mediaField) {
            var icon = "";
            if (mediaField.Ids.Length > 0)
            {
                icon = mediaField.MediaParts[0].MediaUrl;
            }
            return icon;
        }

       
     
    }
}