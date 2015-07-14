using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevOffice.Secret.Models;
using DevOffice.Secret.ViewModels;
using Orchard.ContentManagement.Drivers;

namespace DevOffice.Secret.Drivers
{
    public class PartnerReqsPageDriver : ContentPartDriver<PartnerReqsPagePart>
    {

        protected override DriverResult Display(PartnerReqsPagePart part, string displayType, dynamic shapeHelper) {
            dynamic item = part.ContentItem;
            var model = new PartnerReqsPageViewModel {
                ExternalLink = item.PartnerReqsPagePart.ExternalLink.Value,
                BannerImage = (item.PartnerReqsPagePart.BannerImage.MediaParts.Count > 0 ? item.PartnerReqsPagePart.BannerImage.MediaParts[0].MediaUrl : ""),
                LinkText = item.PartnerReqsPagePart.LinkText.Value,
                Body = item.PartnerReqsPagePart.Body.Value,
                Subtitle = item.PartnerReqsPagePart.Subtitle.Value,
                Image = (item.PartnerReqsPagePart.Image.MediaParts.Count > 0 ? item.PartnerReqsPagePart.Image.MediaParts[0].MediaUrl : ""),
                Title = item.TitlePart.Title
 
            };
            return ContentShape("Parts_PartnerReqsPage",
                () => shapeHelper.Parts_PartnerReqsPage(ViewModel: model));

        }
    }
}