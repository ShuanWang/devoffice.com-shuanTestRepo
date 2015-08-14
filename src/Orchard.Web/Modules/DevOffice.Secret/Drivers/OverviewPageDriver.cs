using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevOffice.Secret.Models;
using DevOffice.Secret.Services;
using DevOffice.Secret.ViewModels;
using Orchard.ContentManagement.Drivers;

namespace DevOffice.Secret.Drivers
{
    public class OverviewPageDriver : ContentPartDriver<OverviewPagePart>
    {
        private readonly ISecretServices _secretServices;

          public OverviewPageDriver( ISecretServices secretServices)
        {
            _secretServices = secretServices;
        }

        protected override DriverResult Display(OverviewPagePart part, string displayType, dynamic shapeHelper)
        {
            dynamic item = part.ContentItem;
            var model = new OverviewPageViewModel
            {
                IntroText = item.OverviewPagePart.IntroText.Value,
                BannerImage = (item.OverviewPagePart.BannerImage.MediaParts.Count > 0 ? item.OverviewPagePart.BannerImage.MediaParts[0].MediaUrl : ""),
                Title = item.TitlePart.Title,
                Rows = _secretServices.BuildEditorViewModel(item.RowWithTilesPart).Rows
             

            };
            return ContentShape("Parts_OverviewPage",
                () => shapeHelper.Parts_OverviewPage(ViewModel: model));

        }
    }
}