using JetBrains.Annotations;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using DevOffice.Secret.Models;

namespace DevOffice.Secret.Handlers
{
    [UsedImplicitly]
	
    public class EmailSettingsPartHandler : ContentHandler
    {
        public EmailSettingsPartHandler(IRepository<EmailSettingsPartRecord> repository)
        {
            Filters.Add(new ActivatingFilter<EmailSettingsPart>("Site"));
            Filters.Add(StorageFilter.For(repository));
        }
    }
}
