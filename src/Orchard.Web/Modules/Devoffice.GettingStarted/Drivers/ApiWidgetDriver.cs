using Devoffice.GettingStarted.Models;
using Orchard.ContentManagement.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Devoffice.GettingStarted.Drivers
{
    public class ApiWidgetDriver : ContentPartDriver<ApiWidgetPart>
    {
        protected override DriverResult Display(ApiWidgetPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_ApiWidget",
                () => shapeHelper.Partial(
                    TemplateName: "Parts/ApiWidget"
            ));
        }
    }
}