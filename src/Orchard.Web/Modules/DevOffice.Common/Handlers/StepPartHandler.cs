using DevOffice.Common.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace DevOffice.Common.Handlers
{
    public class StepPartHandler : ContentHandler
    {
        public StepPartHandler(IRepository<StepPartRecord> repository) 
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}