using System;
using DevOffice.Common.Models;
using DevOffice.Common.Services;
using DevOffice.Common.ViewModels;
using Orchard.ContentManagement.Drivers;
using System.Collections.Generic;
using System.Linq;
using Orchard.Taxonomies.Services;

namespace DevOffice.Common.Drivers
{
    public class HomeArticlesWidgetDriver: ContentPartDriver<HomeArticlesPart> {
        private readonly ICommonDataService _commonDataService;
        private readonly ITaxonomyService _taxonomyService;

        public HomeArticlesWidgetDriver(ICommonDataService commonDataService, ITaxonomyService taxonomyService)
        {
            _commonDataService = commonDataService;
            _taxonomyService = taxonomyService;
        }

        protected override DriverResult Display(HomeArticlesPart part, string displayType, dynamic shapeHelper)
        {
            var articles = _commonDataService.GetArticles(1,8);
            var totalCount = _commonDataService.GetAllArticlesCount();

            return ContentShape("Parts_HomeArticlesWidget",
                () =>
                {
                    var shape = shapeHelper.Parts_HomeArticlesWidget();
                    shape.ContentPart = part;
                    shape.ViewModel = articles;
                    shape.TotalCount = totalCount;
                    return shape;
                });

        }
    }
}