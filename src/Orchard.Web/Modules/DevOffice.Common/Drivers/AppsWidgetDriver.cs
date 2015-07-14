using DevOffice.Common.Models;
using Orchard.ContentManagement.Drivers;

namespace DevOffice.Common.Drivers
{
    public class AppsWidgetDriver : ContentPartDriver<AppsWidgetPart>
    {
        protected override DriverResult Display(AppsWidgetPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_AppsWidget",
                () => shapeHelper.Partial(
                    TemplateName: "Parts/AppsWidget"
            ));
        }
    }
}