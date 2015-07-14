using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace DevOffice.Common.Models
{
    public class ArticlesWidgetRecord : ContentPartRecord 
    {
        public virtual string BlogPostsRssUrl { get; set; }
        public virtual string ArticlesRssUrl { get; set; }
        public virtual string MvpBlogsRssUrl { get; set; }
    }

    public class ArticlesWidgetPart : ContentPart<ArticlesWidgetRecord>
    {
        public string BlogPostsRssUrl
        {
            get { return Record.BlogPostsRssUrl; }
            set { Record.BlogPostsRssUrl = value; } 
            
        }

        public string ArticlesRssUrl
        {
            get { return Record.ArticlesRssUrl; }
            set { Record.ArticlesRssUrl = value; } 
        }

        public string MvpBlogsRssUrl
        {
            get { return Record.MvpBlogsRssUrl;  }
            set { Record.MvpBlogsRssUrl = value; }
        }
    }
}