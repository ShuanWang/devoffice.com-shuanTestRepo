using System;
using System.Collections.Generic;
using DevOffice.Common.Models;

namespace DevOffice.Common.ViewModels
{
    public class TrainingViewModel
    {
        public string Type { get; set; }
        public List<TaxonomyTrainingItem> TaxonomyTrainingItems { get; set; }
        public List<string> TaxonomyNames { get; set; }
        public List<int> TopViewed { get; set; }
        public List<Training> AllTrainingItems { get; set; }
    }

    public class TaxonomyTrainingItem
    {
        public List<Training> TrainingItems { get; set; }
        public string Title { get; set; }
        public string SafeTitle { get; set; }
    }

    public class Training
    {
        public string Title { get; set; }
        public string SubText { get; set; }
        public string Image { get; set; }
        public string Location { get; set; }
        public List<int> TrainingTypes { get; set; }
        public string Month { get; set; }
        public string Date { get; set; }
        public string Year { get; set; }
        public DateTime FullStartDate { get; set; }
        public string ExternalLink { get; set; }
        public DateTime DatePublished { get; set; }
        public IList<RelatedLink> Links { get; set; }
        public Dictionary<int, string> TermsTagged { get; set; }
        public List<string> TermsTaggedList { get; set; }
        public int Id { get; set; }
        public string PermalinkTag { get; set; }
        public int ViewCount { get; set; }
        public int ViewCount30Days { get; set; }
    }

    public class RelatedLink
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public int SortOrder { get; set; }
    }
}