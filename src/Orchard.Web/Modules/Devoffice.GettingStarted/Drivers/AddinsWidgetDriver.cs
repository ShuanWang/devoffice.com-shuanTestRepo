using Devoffice.GettingStarted.Models;
using Orchard.ContentManagement.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Devoffice.GettingStarted.Drivers
{
    public class AddinsWidgetDriver : ContentPartDriver<AddinsWidgetPart>
    {
        protected override DriverResult Display(AddinsWidgetPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_AddinsWidget",
                () => shapeHelper.Partial(
                    TemplateName: "Parts/AddinsWidget"
            ));
        }
    }
}