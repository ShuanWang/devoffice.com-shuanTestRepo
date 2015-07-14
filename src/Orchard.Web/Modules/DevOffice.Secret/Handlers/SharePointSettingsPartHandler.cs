using DevOffice.Secret.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace DevOffice.Secret.Handlers
{
    public class SharePointSettingsPartHandler : ContentHandler
    {
        public SharePointSettingsPartHandler(IRepository<SharePointSettingsPartRecord> repository)
        {
            Filters.Add(new ActivatingFilter<SharePointSettingsPart>("Site"));
            Filters.Add(StorageFilter.For(repository));
        }
    }
}