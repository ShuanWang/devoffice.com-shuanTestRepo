using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevOffice.Secret.Models;
using Orchard.ContentManagement.Drivers;

namespace DevOffice.Secret.Drivers
{
    public class SecretFormWidgetDriver : ContentPartDriver<SecretFormWidgetPart> {

        protected override DriverResult Display(SecretFormWidgetPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_SecretFormWidget",
                () =>
                {
                    var shape = shapeHelper.Parts_SecretFormWidget();
                    shape.ContentPart = part;
                    return shape;
                });
        }
    
    }
}