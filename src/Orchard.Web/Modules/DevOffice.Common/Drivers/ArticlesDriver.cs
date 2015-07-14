using System;
using DevOffice.Common.Models;
using DevOffice.Common.Services;
using DevOffice.Common.ViewModels;
using Orchard.ContentManagement.Drivers;
using System.Collections.Generic;
using System.Linq;
using Orchard.Taxonomies.Models;
using Orchard.Taxonomies.Services;

namespace DevOffice.Common.Drivers
{
    public class ArticlesDriver: ContentPartDriver<ArticlesPart> {
        private readonly ICommonDataService _commonDataService;
        private readonly ITaxonomyService _taxonomyService;

        public ArticlesDriver(ICommonDataService commonDataService, ITaxonomyService taxonomyService)
        {
            _commonDataService = commonDataService;
            _taxonomyService = taxonomyService;
        }

        protected override DriverResult Display(ArticlesPart part, string displayType, dynamic shapeHelper) {
            var articlesCount = _commonDataService.GetAllArticlesCount();
            var articles = _commonDataService.GetArticles();
            
            return ContentShape("Parts_NewArticlesWidget",
                () =>
                {
                    var shape = shapeHelper.Parts_NewArticlesWidget();
                    shape.ContentPart = part;
                    shape.ViewModel = articles;
                    shape.TotalCount = articlesCount;
                    return shape;
                });

        }
    }
}