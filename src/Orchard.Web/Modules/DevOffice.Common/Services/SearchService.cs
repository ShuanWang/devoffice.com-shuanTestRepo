using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevOffice.Common.ViewModels;
using Orchard;
using Orchard.Collections;
using Orchard.Indexing;
using Orchard.Localization;
using Orchard.Localization.Services;

namespace DevOffice.Common.Services
{
    public class SearchService : ISearchService
    {
        private readonly IIndexManager _indexManager;
        private readonly ICultureManager _cultureManager;

        public SearchService(IOrchardServices services, IIndexManager indexManager, ICultureManager cultureManager)
        {
            Services = services;
            _indexManager = indexManager;
            _cultureManager = cultureManager;
            T = NullLocalizer.Instance;
        }

        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }

        ISearchBuilder Search(string index, string contentType)
        {
            return _indexManager.HasIndexProvider()
                ? _indexManager.GetSearchIndexProvider().CreateSearchBuilder(index).WithField("type", contentType).ExactMatch().AsFilter()
                : new NullSearchBuilder();
        }

        List<PageOfItemsList> ISearchService.Query<T>(string query, int page, int? pageSize, bool filterCulture, string index, string[] searchFields, Func<ISearchHit, T> shapeResult, List<string> contentTypes)
        {

            var pageOfItemsList = new List<PageOfItemsList>();

            if (string.IsNullOrWhiteSpace(query))
                return pageOfItemsList;

            foreach (var contentType in contentTypes)
            {

                var searchBuilder = Search(index, contentType).Parse(searchFields, query);

                var totalCount = searchBuilder.Count();

                if (pageSize != null)
                    searchBuilder = searchBuilder
                        .Slice((page > 0 ? page - 1 : 0) * (int)pageSize, (int)pageSize);

                var searchResults = searchBuilder.Search();
                
                var pageOfItems = new PageOfItems<T>(searchResults.Select(shapeResult))
                {
                    PageNumber = page,
                    PageSize = pageSize != null ? (int)pageSize : totalCount,
                    TotalItemCount = totalCount
                };

                pageOfItemsList.Add(new PageOfItemsList {ContentType = contentType, Items = pageOfItems});

            }

            return pageOfItemsList;
        }
    }
}