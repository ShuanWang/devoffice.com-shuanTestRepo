using System;
using System.Collections.Generic;
using DevOffice.Common.ViewModels;
using Orchard;
using Orchard.Collections;
using Orchard.Indexing;

namespace DevOffice.Common.Services {
    public interface ISearchService : IDependency {
        List<PageOfItemsList> Query<T>(string query, int skip, int? take, bool filterCulture, string index, string[] searchFields, Func<ISearchHit, T> shapeResult, List<string> contentType);
    }
}