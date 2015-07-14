using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sunkist.FeaturedItemSlider.ViewModels;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.MediaLibrary.Fields;
using Orchard.MediaLibrary.Models;
using Sunkist.FeaturedItemSlider.Models;

namespace Sunkist.FeaturedItemSlider.Drivers {
    public class FeaturedItemSliderWidgetPartDriver : ContentPartDriver<FeaturedItemSliderWidgetPart> {
        private readonly IContentManager _contentManager;

        public FeaturedItemSliderWidgetPartDriver(IContentManager contentManager) {
            _contentManager = contentManager;
        }



        protected override DriverResult Display(FeaturedItemSliderWidgetPart part, string displayType, dynamic shapeHelper) {
            int slideNumber = 0;

            var featuredItems = _contentManager.Query<FeaturedItemPart, FeaturedItemPartRecord>("FeaturedItem")
                .Where(fip => fip.GroupName == part.GroupName)
                .OrderBy(fi => fi.SlideOrder)
                .List()
                .Select(fi => new FeaturedItemViewModel {
                    Headline = fi.Headline,
                    SubHeadline = fi.SubHeadline,
                    LinkUrl = fi.LinkUrl,
                    SeparateLink = fi.SeparateLink,
                    LinkText = fi.LinkText,
                    ImagePath = getImagePath(fi, "Picture"),
                    DescriptionImagePaths = ((MediaLibraryPickerField) fi.Fields.Single(f => f.Name == "DescriptionImages")).MediaParts == null ? new List<string>() : ExtractUrlsFromMediaPickerField(((MediaLibraryPickerField) fi.Fields.Single(f => f.Name == "DescriptionImages")).MediaParts),
                    FeaturedImagePath = getImagePath(fi, "FeaturedImage"),
                    SlideNumber = ++slideNumber,
                    ImageLinks = GetMediaUrls((dynamic)fi.Fields.Single(f => f.Name == "DescriptionImages"))

                }).ToList();

            var group = _contentManager.Query<FeaturedItemGroupPart, FeaturedItemGroupPartRecord>("FeaturedItemGroup")
                .Where(fig => fig.Name == part.GroupName)
                .List()
                .SingleOrDefault();

            return ContentShape("Parts_FeaturedItems",
                () => shapeHelper.Parts_FeaturedItems(FeaturedItems: featuredItems, ContentPart: part, Group: group));
        }

        private string getImagePath(FeaturedItemPart fi, string fieldName) {

            // ((MediaLibraryPickerField) fi.Fields.Single(f => f.Name == "Picture")).MediaParts == null ? "" : ((MediaLibraryPickerField) fi.Fields.Single(f => f.Name == "Picture")).MediaParts.First().MediaUrl

            var x = String.Empty;
            if (((MediaLibraryPickerField) fi.Fields.Single(f => f.Name == fieldName)).MediaParts != null &&
                ((MediaLibraryPickerField) fi.Fields.Single(f => f.Name == fieldName)).MediaParts.Any() &&
                ((MediaLibraryPickerField) fi.Fields.Single(f => f.Name == fieldName)).MediaParts.First() != null) {
                    x = ((MediaLibraryPickerField)fi.Fields.Single(f => f.Name == fieldName)).MediaParts.First().MediaUrl;
            }
            return x;
        }

        private List<string> ExtractUrlsFromMediaPickerField(IEnumerable<MediaPart> mediaParts) {
            List<string> paths = new List<string>();
            foreach (dynamic resource in mediaParts) {
                paths.Add(resource.MediaUrl);
            }
            return paths;
        }
        private List<ImageLinksViewModel.ImageLinks> GetMediaUrls(dynamic mediaField)
        {
            var icons = new List<ImageLinksViewModel.ImageLinks>();
            if (mediaField.Ids.Length > 0)
            {
                foreach (MediaPart mediaPart in mediaField.MediaParts)
                {
                    icons.Add(new ImageLinksViewModel.ImageLinks{Image = mediaPart.MediaUrl, ImageUrl = mediaPart.Caption});
                }
            }
            return icons;
        }

        protected override DriverResult Editor(FeaturedItemSliderWidgetPart part, dynamic shapeHelper) {
            var groups = _contentManager.Query<FeaturedItemGroupPart, FeaturedItemGroupPartRecord>("FeaturedItemGroup")
                .List().Select(fig => fig.Name).ToList();

            var viewModel = new FeaturedItemSliderWidgetEditViewModel {GroupNames = groups, SelectedGroup = part.GroupName};
            return ContentShape("Parts_FeaturedItemSliderWidget_Edit",
                () => shapeHelper.EditorTemplate(TemplateName: "Parts.FeaturedItemSliderWidget.Edit", Model: viewModel));
        }

        protected override DriverResult Editor(FeaturedItemSliderWidgetPart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, "", null, null);
            return Editor(part, shapeHelper);
        }

        protected override void Exporting(FeaturedItemSliderWidgetPart part, ExportContentContext context) {
            context.Element(part.PartDefinition.Name).SetAttributeValue("GroupName", part.GroupName);
        }

        protected override void Importing(FeaturedItemSliderWidgetPart part, ImportContentContext context) {
            part.GroupName = context.Attribute(part.PartDefinition.Name, "GroupName");
        }
    }
}