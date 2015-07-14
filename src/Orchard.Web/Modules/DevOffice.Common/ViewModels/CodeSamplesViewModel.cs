using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevOffice.Common.Models;

namespace DevOffice.Common.ViewModels
{
    public class CodeSamplesViewModel
    {
        public List<string> Types { get; set; }
        public List<string> Platforms { get; set; }
        public List<string> Languages { get; set; }
        public List<string> Services { get; set; }
        public List<string> SourceReps { get; set; }
        public List<string> Products { get; set; }
        public List<string> SelectedTypes { get; set; }
        public List<string> SelectedPlatforms { get; set; }
        public List<string> SelectedLanguages { get; set; }
        public List<string> SelectedServices { get; set; }
        public List<string> SelectedSourceReps { get; set; }
        public List<string> SelectedProducts { get; set; }
        public List<CodeSample> CodeSamples { get; set; }
        public List<int> TopViewed { get; set; }
    }

    public class CodeSample
    {
        public string Title { get; set; }
        public string SubText { get; set; }
        public string Image { get; set; }
        public string Location { get; set; }
        public List<string> TermTypes { get; set; }
        public DateTime FullStartDate { get; set; }
        public string ExternalLink { get; set; }
        public string GitHubLink { get; set; }
        public DateTime DatePublished { get; set; }
        public IList<RelatedLink> Links { get; set; }
        public string PermalinkTag { get; set; }
        public int Id { get; set; }
        public int ViewCount { get; set; }
    }

    public class GitHubViewModel
    {
        public string HTMLData { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Icon { get; set; }
        public string ExternalLink { get; set; }
        public int ContentItemId { get; set; }
    }
}