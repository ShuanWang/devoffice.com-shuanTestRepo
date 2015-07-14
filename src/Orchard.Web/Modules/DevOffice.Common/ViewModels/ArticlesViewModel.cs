using System.Collections.Generic;

namespace DevOffice.Common.ViewModels
{
    public class ArticlesViewModel
    {
        public string BlogPostsUrl { get; set; }
        public string ArticlesUrl { get; set; }
        public string MvpBlogsUrl { get; set; }
    }

    // TODO: Delete this class after removing IRssDataService and RssDataService
    public class Article
    {
        public string Title { get; set; }
        public string SubText { get; set; }
        public string Url { get; set; }
        public string BaseUrl { get; set; }
    }
}