using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security.AntiXss;
using System.Web.UI.WebControls.WebParts;
using DevOffice.Common.ViewModels;
using Orchard;
using Orchard.Collections;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.Indexing;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Search.Models;
using DevOffice.Common.Services;
using Orchard.Settings;
using Orchard.Themes;
using Orchard.UI.Navigation;
using Orchard.UI.Notify;
using SearchViewModel = DevOffice.Common.ViewModels.SearchViewModel;

namespace DevOffice.Common.Controllers
{

    [ValidateInput(false), Themed]
    public class SearchController : Controller
    {
        private readonly ISearchService _searchService;
        private readonly IContentManager _contentManager;
        private readonly ISiteService _siteService;

        public SearchController(
            IOrchardServices services,
            ISearchService searchService,
            IContentManager contentManager,
            ISiteService siteService,
            IShapeFactory shapeFactory)
        {
            Services = services;
            _searchService = searchService;
            _contentManager = contentManager;
            _siteService = siteService;

            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
            Shape = shapeFactory;
        }

        private IOrchardServices Services { get; set; }
        public Localizer T { get; set; }
        public ILogger Logger { get; set; }
        dynamic Shape { get; set; }

        public ActionResult Index(PagerParameters pagerParameters, string q = "") {

            var cleanQ = AntiXssEncoder.HtmlEncode(q, true); // ensure the search query is properly encoded to prevent XSS

            var searchSettingPart = Services.WorkContext.CurrentSite.As<SearchSettingsPart>();

            if (String.IsNullOrEmpty(searchSettingPart.SearchIndex))
            {
                Services.Notifier.Error(T("Please define a default search index"));
                return HttpNotFound();
            }

            List<string> contentTypes = new List<string>() {
                "Event", "Community", "CodeSample", 
                "PatternsAndPractices", "Podcast", "Resource", 
                "Solution", "StepPage", "Training", "VideoItem", "Article"
            };

            var results = new List<ContentTypeResults>();
            results = GetSearchResults(cleanQ, pagerParameters, searchSettingPart, contentTypes);
           
            var searchViewModel = new SearchViewModel
            {
                Query = cleanQ,
                FilteredItems = results
            };

            //todo: deal with page requests beyond result count
            return View(searchViewModel);
        }

        [HttpPost]
        public ActionResult ListMore(string q, int? currentPage, string currentContentType)
        {
            PagerParameters pagerParameters = new PagerParameters() { Page = currentPage };

            var searchSettingPart = Services.WorkContext.CurrentSite.As<SearchSettingsPart>();

            if (String.IsNullOrEmpty(searchSettingPart.SearchIndex))
            {
                Services.Notifier.Error(T("Please define a default search index"));
                return HttpNotFound();
            }

            var results = new List<ContentTypeResults>();
            List<string> contentTypes = new List<string>() { currentContentType };

            results = GetSearchResults(AntiXssEncoder.HtmlEncode(q, true), pagerParameters, searchSettingPart, contentTypes);
            //todo: deal with page requests beyond result count

            //return View(results);
            return PartialView("ListMore", results);
        }

        private List<ContentTypeResults> GetSearchResults(string q, PagerParameters pagerParameters, SearchSettingsPart searchSettingPart, List<string> contentTypes) {

            var cleanQ = AntiXssEncoder.HtmlEncode(q, true); // ensure the search query is properly encoded to prevent XSS

            var results = new List<ContentTypeResults>();
            var searchHitsDictionary = new List<PageOfItemsList>();

            var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);

            searchHitsDictionary = _searchService.Query(q, pager.Page, pager.PageSize,
                Services.WorkContext.CurrentSite.As<SearchSettingsPart>().FilterCulture,
                searchSettingPart.SearchIndex,
                searchSettingPart.SearchedFields,
                searchHit => searchHit,
                contentTypes);

            try
            {
                foreach (var searchHits in searchHitsDictionary)
                {
                    var list = Shape.List();

                    IPageOfItems<ISearchHit> items = searchHits.Items;
                    var foundIds = items.Select(searchHit => searchHit.ContentItemId).ToList();
                    var foundItems = _contentManager.GetMany<IContent>(foundIds, VersionOptions.Published, new QueryHints()).ToList();

                    if (foundItems != null && foundItems.Any())
                    {
                        // ignore search results which content item has been removed or unpublished
                        foreach (var item in foundItems)
                        {
                            list.Add(_contentManager.BuildDisplay(item, "Summary"));
                        }

                        var contentTypeDisplayName = foundItems[0].ContentItem.TypeDefinition.DisplayName;

                        results.Add(new ContentTypeResults()
                        {
                            ContentType = searchHits.ContentType,
                            ContentTypeDisplayName = contentTypeDisplayName,
                            TotalItemCount = items.TotalItemCount,
                            StartPosition = (pager.Page - 1) * pager.PageSize + 1,
                            EndPosition = pager.Page * pager.PageSize > items.TotalItemCount ? items.TotalItemCount : pager.Page * pager.PageSize,
                            Pages = (items.TotalItemCount / pager.PageSize) + (items.TotalItemCount % pager.PageSize == 0 ? 0 : 1),
                            ContentItems = list,
                            Pager = pager
                        });
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error(T("Invalid search query: {0}", exception.Message).Text);
                Services.Notifier.Error(T("Invalid search query: {0}", exception.Message));
            }

            return results;
        }
    }
}