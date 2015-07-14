using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevOffice.Secret.Models;
using Orchard.ContentManagement.Drivers;

namespace DevOffice.Secret.Drivers
{
    public class ActivityFeedAPIWidgetDriver : ContentPartDriver<ActivityFeedAPIFormWidgetPart>
    {

        protected override DriverResult Display(ActivityFeedAPIFormWidgetPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_ActivityFeedAPIFormWidget",
                () =>
                {
                    var shape = shapeHelper.Parts_ActivityFeedAPIFormWidget();
                    shape.ContentPart = part;
                    return shape;
                });
        }
    }
}