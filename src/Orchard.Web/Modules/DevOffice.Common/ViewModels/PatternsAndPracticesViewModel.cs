using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevOffice.Common.Models;

namespace DevOffice.Common.ViewModels
{
    public class PatternsAndPracticesViewModel
    {
        public List<string> Types { get; set; }
        public List<PatternsAndPractice> PatternsAndPractices { get; set; }
        public List<string> SelectedTypes { get; set; }
        public List<int> TopViewed { get; set; }
        public List<string> Platforms { get; set; }
        public List<string> Languages { get; set; }
        public List<string> Services { get; set; }
        public List<string> SourceReps { get; set; }
        public List<string> Products { get; set; }
        public List<string> Themes { get; set; }
        public List<string> SecondaryTypes { get; set; }
    }

    public class PatternsAndPractice
    {
        public string Title { get; set; }
        public string SubText { get; set; }
        public string Image { get; set; }
        public int Ordering { get; set; }
        public string ExternalLink { get; set; }
        public string Link { get; set; }
        public IEnumerable<string> PatternsAndPracticesTypes { get; set; }
        public string PermalinkTag { get; set; }
        public int Id { get; set; }
        public int ViewCount { get; set; }
        public int ViewCount30Days { get; set; }
        public DateTime UpdatedDate { get; set; }
        public IList<RelatedLink> Links { get; set; }

    }
}