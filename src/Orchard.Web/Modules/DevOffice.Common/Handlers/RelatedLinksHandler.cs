using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevOffice.Common.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace DevOffice.Common.Handlers
{
    public class RelatedLinksHandler: ContentHandler
    {
        public RelatedLinksHandler(IRepository<RelatedLinksRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}