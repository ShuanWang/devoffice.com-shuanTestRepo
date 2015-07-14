using System.Collections.Generic;

namespace DevOffice.Common.ViewModels
{
    public class CommunityViewModel
    {
        public List<CommunityItem> CommunityItems { get; set; } 
    }

    public class CommunityItem
    {
        public string Title { get; set; }
        public string SubText { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string ExternalLink { get; set; }
    }
}