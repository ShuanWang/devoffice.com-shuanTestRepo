using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevOffice.Common.Models;
using DevOffice.Common.ViewModels;
using Orchard.ContentManagement.Drivers;

namespace DevOffice.Common.Drivers
{
    public class BannerWidgetDriver : ContentPartDriver<BannerWidgetPart>
    {
        protected override DriverResult Display(BannerWidgetPart part, string displayType, dynamic shapeHelper)
        {
            dynamic item = (dynamic)part.ContentItem;

            var model = new BannerViewModel
            {
                BodyText = item.BannerWidgetPart.BodyText.Value,
                ExternalLink = item.BannerWidgetPart.ExternalLink.Value,
                ExternalLinkText = item.BannerWidgetPart.LinkText.Value,
                LinkBackgroundColor = item.BannerWidgetPart.LinkBackgroundColor.Value,
                LinkTextColor = item.BannerWidgetPart.LinkTextColor.Value
            };

            return ContentShape("Parts_BannerWidget",
                 () =>
                 {
                     var shape = shapeHelper.Parts_BannerWidget();
                     shape.ContentPart = part;
                     shape.ViewModel = model;

                     return shape;
                 });
        }
    }
}