using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace DevOffice.Common.Models
{
    public class EventPartRecord : ContentPartRecord { }
    public class EventsPart: ContentPart<EventPartRecord> { }
}