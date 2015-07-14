using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevOffice.Common.ViewModels
{
    public class BlogPostsViewModel
    {
        public List<BlogPost> Articles { get; set; }
        public int AllArticlesCount { get; set; }
    }

    public class BlogPost {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public List<string> Topics { get; set; }
        public DateTime DatePublished { get; set; }
        public string PostBody { get; set; }
    }
}