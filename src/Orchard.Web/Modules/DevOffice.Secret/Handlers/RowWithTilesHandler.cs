using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevOffice.Secret.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace DevOffice.Secret.Handlers
{
    public class RowWithTilesHandler : ContentHandler
    {
        public RowWithTilesHandler(IRepository<RowWithTilesRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}