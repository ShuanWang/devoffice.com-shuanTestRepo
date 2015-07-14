using DevOffice.Common.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace DevOffice.Common.Handlers
{
    public class ArticlesWidgetHandler: ContentHandler
    {
        public ArticlesWidgetHandler(IRepository<ArticlesWidgetRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }

}