using DevOffice.Common.ViewModels;
using Orchard;
using System.Collections.Generic;

namespace DevOffice.Common.Services
{
    public interface IRssDataService : IDependency {
        List<Article> GetRssFeed(string rssUrl, int numberOfItems = 3);
    }

    
}
