using System.Data;
using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;
using Sunkist.FeaturedItemSlider.Models;

namespace Sunkist.FeaturedItemSlider {
    public class Migrations : DataMigrationImpl {

        public int Create() {
            SchemaBuilder.CreateTable("FeaturedItemGroupPartRecord", builder => builder
                .ContentPartRecord()
                .Column<string>("Name", col => col.WithLength(100))
                .Column<int>("GroupWidth")
                .Column<int>("GroupHeight")
                .Column<bool>("IncludeImages", col => col.WithDefault(true))
                .Column<string>("ImageStyle", col => col.WithDefault(ImageStyle.Inline.ToString()))
                .Column<int>("ImageWidth")
                .Column<int>("ImageHeight")
                .Column<int>("HeadlineOffsetTop")
                .Column<int>("HeadlineOffsetLeft")
                .Column<string>("BackgroundColor")
                .Column<string>("ForegroundColor")
                .Column<int>("SlideSpeed", cfg => cfg.WithDefault(300))
                .Column<int>("SlidePause", cfg => cfg.WithDefault(6000))
                .Column<bool>("ShowSlideNumbers", col => col.WithDefault(false))
                .Column<bool>("ShowPager", col => col.WithDefault(true))
                .Column<string>("TransitionEffect", col => col.WithDefault("scrollLeft"))
                );

            ContentDefinitionManager.AlterTypeDefinition("FeaturedItemGroup", builder => builder
                .DisplayedAs("Featured Item Group")
                .WithPart("FeaturedItemGroupPart")
                .WithPart("CommonPart")
                .WithPart("IdentityPart")
                );

            SchemaBuilder.CreateTable("FeaturedItemPartRecord", builder => builder
                    .ContentPartRecord()
                    .Column<string>("Headline", col => col.Unlimited())
                    .Column<string>("SubHeadline", col => col.Unlimited())
                    .Column<string>("LinkUrl", col => col.Unlimited())
                    .Column<bool>("SeparateLink", col => col.WithDefault(false))
                    .Column<string>("LinkText", col => col.Unlimited())
                    .Column<string>("GroupName", col => col.WithLength(100))
                    .Column<int>("SlideOrder", col => col.WithDefault(0))
                );

            ContentDefinitionManager.AlterTypeDefinition("FeaturedItem", builder => builder
                    .DisplayedAs("Featured Item")
                    .WithPart("FeaturedItemPart")
                    .WithPart("CommonPart")
                    .WithPart("IdentityPart")
                );

            ContentDefinitionManager.AlterPartDefinition("FeaturedItemPart", builder => builder
                    .WithField("Picture", cfg => cfg
                        .OfType("MediaLibraryPickerField")
                        .WithSetting("MediaLibraryPickerFieldSettings.DisplayedContentTypes", "Image")
                        .WithSetting("MediaLibraryPickerFieldSettings.Multiple", "False"))

                );

            SchemaBuilder.CreateTable("FeaturedItemSliderWidgetPartRecord", builder => builder
                .ContentPartRecord()
                .Column<string>("GroupName", col => col.WithLength(100)));

            ContentDefinitionManager.AlterTypeDefinition("FeaturedItemSliderWidget", builder => builder
                    .WithPart("FeaturedItemSliderWidgetPart")
                    .WithPart("CommonPart")
                    .WithPart("WidgetPart")
                    .WithPart("IdentityPart")
                    .WithSetting("Stereotype", "Widget")
                );

            return 1;
        }

        public int UpdateFrom1() {
          ContentDefinitionManager.AlterPartDefinition("FeaturedItemPart", builder => builder
                   .WithField("DescriptionImages", cfg => cfg
                        .OfType("MediaLibraryPickerField")
                        .WithDisplayName("Description Images")
                        .WithSetting("MediaLibraryPickerFieldSettings.DisplayedContentTypes", "Image")
                        .WithSetting("MediaLibraryPickerFieldSettings.Multiple", "True")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "Use up to three images for best front-end look. If both text and images are provided, only images are displayed by default."))
                        );
            return 2;
        }

        public int UpdateFrom2() {
            ContentDefinitionManager.AlterPartDefinition("FeaturedItemPart", builder => builder
                .WithField("FeaturedImage", cfg => cfg
                    .OfType("MediaLibraryPickerField")
                    .WithDisplayName("Featured Image")
                    .WithSetting("MediaLibraryPickerFieldSettings.DisplayedContentTypes", "Image")
                    .WithSetting("MediaLibraryPickerFieldSettings.Multiple", "False")
                    .WithSetting("MediaLibraryPickerFieldSettings.Hint", "Add an image to be featured next to the description. This image should be exactly 140 x 140."))
                    );
            return 3;
        }
    }
}