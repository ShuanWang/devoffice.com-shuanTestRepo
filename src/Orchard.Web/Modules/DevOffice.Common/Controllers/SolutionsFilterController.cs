using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DevOffice.Common.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Logging;
using DevOffice.Common.Services;
using Orchard.Settings;
using Orchard.Taxonomies.Services;
using Orchard.Themes;

namespace DevOffice.Common.Controllers
{
    [ValidateInput(false), Themed]
    public class SolutionsFilterController : Controller
    {
         private readonly ISearchService _searchService;
        private readonly IContentManager _contentManager;
        private readonly ISiteService _siteService;
        private readonly ICommonDataService _commonDataService;
        private readonly ITaxonomyService _taxonomyService;


        public SolutionsFilterController(
            IOrchardServices services,
            ISearchService searchService,
            IContentManager contentManager,
            ISiteService siteService,
            IShapeFactory shapeFactory,
            ICommonDataService commonDataService,
            ITaxonomyService taxonomyService)
        {
            Services = services;
            _searchService = searchService;
            _contentManager = contentManager;
            _siteService = siteService;
            _commonDataService = commonDataService;
            _taxonomyService = taxonomyService;

            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
            Shape = shapeFactory;
        }

        private IOrchardServices Services { get; set; }
        public Localizer T { get; set; }
        public ILogger Logger { get; set; }
        dynamic Shape { get; set; }

        [HttpPost]
        public ActionResult GetSolutionsForHorizontal(string filters)
        {
            var filtersArray = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(filters) ?? new string[0];
            var solutions = _commonDataService.GetSolutions(filtersArray);

            return PartialView("Parts/SolutionsSwipeWidget", solutions); // TODO: ADD THIS FILE
        }

        [HttpPost]
        public ActionResult GetSolutionsForVertical(string filters)
        {
            var filtersArray = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(filters) ?? new string[0];
            var solutions = _commonDataService.GetSolutions(filtersArray);

            return PartialView("Parts/SolutionColumnList",solutions);
        }


       
    }
}