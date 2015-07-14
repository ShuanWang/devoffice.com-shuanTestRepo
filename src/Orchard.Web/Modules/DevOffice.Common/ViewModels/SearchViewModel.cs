using System.Collections.Generic;
using Orchard.Collections;
using Orchard.Localization;

namespace DevOffice.Common.ViewModels {
    
    public class SearchViewModel {
        public string Query { get; set; }
        
        public List<ContentTypeResults> FilteredItems { get; set; }
    }

    public class ContentTypeResults {
        public string ContentType { get; set; }
        public string ContentTypeDisplayName { get; set; }
        public dynamic ContentItems { get; set; }
        public int TotalItemCount { get; set; }
        public int StartPosition { get; set; }
        public int EndPosition { get; set; }
        public int Pages { get; set; }
        public dynamic Pager { get; set; }
    }

    public class PageOfItemsList {
        public string ContentType { get; set; }
        public dynamic Items { get; set; }
    }
}