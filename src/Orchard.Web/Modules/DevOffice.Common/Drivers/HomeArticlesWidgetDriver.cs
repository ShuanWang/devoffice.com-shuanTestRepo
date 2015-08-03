using System;
using DevOffice.Common.Models;
using DevOffice.Common.Services;
using DevOffice.Common.ViewModels;
using Orchard.Blogs.Services;
using Orchard.ContentManagement.Drivers;
using System.Collections.Generic;
using System.Linq;
using Orchard.Taxonomies.Services;
using Orchard.Core.Common.Models;
using Orchard.Blogs.Models;
using Orchard.ContentManagement;

namespace DevOffice.Common.Drivers
{
    public class HomeArticlesWidgetDriver: ContentPartDriver<HomeArticlesPart> {
        private readonly IBlogService _blogService;
        private readonly ICommonDataService _commonDataService;
        private readonly ITaxonomyService _taxonomyService;
        private readonly IContentManager _contentManager;

        public HomeArticlesWidgetDriver(ICommonDataService commonDataService, ITaxonomyService taxonomyService, IBlogService blogService, IContentManager contentManager)
        {
            _commonDataService = commonDataService;
            _taxonomyService = taxonomyService;
            _blogService = blogService;
            _contentManager = contentManager;
        }

        protected override DriverResult Display(HomeArticlesPart part, string displayType, dynamic shapeHelper)
        {
            var blogPosts = _contentManager.Query(VersionOptions.Published, "BlogPost")
                    .Join<CommonPartRecord>().Where(cr => cr.Container.Id == 4672)
                    .OrderByDescending(cr => cr.CreatedUtc)
                    .Slice(0, 8)
                    .Select(ci => ci.As<BlogPostPart>());

            var blogPostsItems = new List<BlogPost>();
            foreach (var post in blogPosts)
            {
                dynamic item = post.ContentItem;
                blogPostsItems.Add(new BlogPost()
                {
                    Title = post.Title,
                    Link = item.AutoroutePart.DisplayAlias


                });
            }

            //var articles = _commonDataService.GetArticles(1,8);
            //var totalCount = _commonDataService.GetAllArticlesCount();

            return ContentShape("Parts_HomeArticlesWidget",
                () =>
                {
                    var shape = shapeHelper.Parts_HomeArticlesWidget();
                    shape.ContentPart = part;
                    shape.ViewModel = blogPostsItems;
                    shape.TotalCount = 0;
                    return shape;
                });

        }
    }
}