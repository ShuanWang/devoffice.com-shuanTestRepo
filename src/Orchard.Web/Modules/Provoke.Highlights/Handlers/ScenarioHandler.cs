using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Provoke.Highlights.Models;

namespace Provoke.Highlights.Handlers
{
    public class ScenarioHandler : ContentHandler
    {
        public ScenarioHandler(IRepository<ScenarioRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}