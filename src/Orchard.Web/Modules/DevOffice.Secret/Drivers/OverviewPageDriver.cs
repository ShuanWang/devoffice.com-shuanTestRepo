using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevOffice.Secret.Models;
using DevOffice.Secret.ViewModels;
using Orchard.ContentManagement.Drivers;

namespace DevOffice.Secret.Drivers
{
    public class OverviewPageDriver : ContentPartDriver<OverviewPagePart>
    {
        protected override DriverResult Display(OverviewPagePart part, string displayType, dynamic shapeHelper)
        {
            dynamic item = part.ContentItem;
            var model = new OverviewPageViewModel
            {
                IntroText = item.OverviewPagePart.IntroText.Value,
                BannerImage = (item.OverviewPagePart.BannerImage.MediaParts.Count > 0 ? item.OverviewPagePart.BannerImage.MediaParts[0].MediaUrl : ""),
                Title = item.TitlePart.Title,

                Row1Title = item.OverviewPagePart.Row1Title.Value,
                Row1Body = item.OverviewPagePart.Row1Body.Value,
                Row1ExternalLink = item.OverviewPagePart.Row1ExternalLink.Value,
                Row1LinkText = item.OverviewPagePart.Row1LinkText.Value,

                Row1TileImage = (item.OverviewPagePart.Row1TileImage.MediaParts.Count > 0 ? item.OverviewPagePart.Row1TileImage.MediaParts[0].MediaUrl : ""),
                Row1TileTitle = item.OverviewPagePart.Row1TileTitle.Value,
                Row1TileBody = item.OverviewPagePart.Row1TileBody.Value,
                Row1TileExternalLink = item.OverviewPagePart.Row1TileExternalLink.Value,
                Row1TileLinkText = item.OverviewPagePart.Row1TileLinkText.Value,

                Row1Tile2Image = (item.OverviewPagePart.Row1Tile2Image.MediaParts.Count > 0 ? item.OverviewPagePart.Row1Tile2Image.MediaParts[0].MediaUrl : ""),
                Row1Tile2Title = item.OverviewPagePart.Row1Tile2Title.Value,
                Row1Tile2Body = item.OverviewPagePart.Row1Tile2Body.Value,
                Row1Tile2ExternalLink = item.OverviewPagePart.Row1Tile2ExternalLink.Value,
                Row1Tile2LinkText = item.OverviewPagePart.Row1Tile2LinkText.Value,

                Row2Title = item.OverviewPagePart.Row2Title.Value,
                Row2Body = item.OverviewPagePart.Row2Body.Value,
                Row2ExternalLink = item.OverviewPagePart.Row2ExternalLink.Value,
                Row2LinkText = item.OverviewPagePart.Row2LinkText.Value,

                Row2TileImage = (item.OverviewPagePart.Row2TileImage.MediaParts.Count > 0 ? item.OverviewPagePart.Row2TileImage.MediaParts[0].MediaUrl : ""),
                Row2TileTitle = item.OverviewPagePart.Row2TileTitle.Value,
                Row2TileBody = item.OverviewPagePart.Row2TileBody.Value,
                Row2TileExternalLink = item.OverviewPagePart.Row2TileExternalLink.Value,
                Row2TileLinkText = item.OverviewPagePart.Row2TileLinkText.Value,

                Row2Tile2Image = (item.OverviewPagePart.Row2Tile2Image.MediaParts.Count > 0 ? item.OverviewPagePart.Row2Tile2Image.MediaParts[0].MediaUrl : ""),
                Row2Tile2Title = item.OverviewPagePart.Row2Tile2Title.Value,
                Row2Tile2Body = item.OverviewPagePart.Row2Tile2Body.Value,
                Row2Tile2ExternalLink = item.OverviewPagePart.Row2Tile2ExternalLink.Value,
                Row2Tile2LinkText = item.OverviewPagePart.Row2Tile2LinkText.Value

            };
            return ContentShape("Parts_OverviewPage",
                () => shapeHelper.Parts_OverviewPage(ViewModel: model));

        }
    }
}