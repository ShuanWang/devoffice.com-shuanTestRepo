using DevOffice.Common.Models;
using Orchard.ContentManagement.Drivers;

namespace DevOffice.Common.Drivers
{
    public class FeedbackWidgetDriver : ContentPartDriver<FeedbackWidgetPart>
    {
        protected override DriverResult Display(FeedbackWidgetPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_FeedbackWidget",
                () => shapeHelper.Partial(
                    TemplateName: "Parts/FeedbackWidget"
            ));
        }
    }
}