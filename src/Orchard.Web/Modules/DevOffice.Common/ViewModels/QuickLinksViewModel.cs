using System.Collections.Generic;

namespace DevOffice.Common.ViewModels
{
    public class QuickLinksViewModel
    {
        public List<QuickLink> QuickLinks { get; set; }
    }

    public class QuickLink
    {
        public string Title { get; set; }
        public string SubText { get; set; }
        public string SmallImage { get; set; }
        public string BigImage { get; set; }
        public string ExternalLink { get; set; }
        public decimal SortOrder { get; set; }
    }
}