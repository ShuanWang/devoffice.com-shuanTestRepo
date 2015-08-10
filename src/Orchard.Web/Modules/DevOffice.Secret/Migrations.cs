using System.Linq;
using System.Web.UI;
using DevOffice.Secret.Models;
using NHibernate.Linq;
using Orchard.Alias;
using Orchard.Autoroute.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Common.Fields;
using Orchard.Core.Contents.Extensions;
using Orchard.Core.Navigation.Services;
using Orchard.Core.Title.Models;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;
using Orchard.MediaLibrary.Fields;
using Orchard.Widgets.Services;
using Orchard.Autoroute.Services;

namespace DevOffice.Secret {
    [OrchardFeature("DevOffice.Secret")]
    public class Migrations : DataMigrationImpl {
        private readonly IContentManager _contentManager;
        private readonly IMenuService _menuService;
        private readonly IAliasService _aliasService;
        private readonly IAutorouteService _autorouteService;
        private readonly IWidgetsService _widgetsService;

        public Migrations(
            IContentManager contentManager,
            IMenuService menuService,
            IAliasService aliasService,
            IAutorouteService autorouteService,
            IWidgetsService widgetsService) {
            _contentManager = contentManager;
            _menuService = menuService;
            _aliasService = aliasService;
            _autorouteService = autorouteService;
            _widgetsService = widgetsService;
        }


        public int Create() {
            #region Create Secret Form Widget

            //Create Secret Form Layer
            var secretFormPageLayer = _widgetsService.CreateLayer("Cloud Storage Form Page", "Widgets on this layer are displayed on the Cloud Storage Form page", "url('~/programs/officecloudstorageform')");

            //Create Secret Form Page
            var secretFormPage = _contentManager.Create("Page");
            secretFormPage.As<TitlePart>().Title = "Registration Form";

            var secretFormPageArp = secretFormPage.As<AutoroutePart>();
            secretFormPageArp.DisplayAlias = "programs/officecloudstorageform";
            _autorouteService.GenerateAlias(secretFormPageArp);
            _autorouteService.PublishAlias(secretFormPageArp);
            _contentManager.Publish(secretFormPage);

            //Create SecretFormWidget part and type
            ContentDefinitionManager.AlterPartDefinition(
                "SecretFormWidgetPart",
                builder => builder.Attachable());
            ContentDefinitionManager.AlterTypeDefinition(
                "SecretFormWidget",
                cfg => cfg
                    .WithPart("SecretFormWidgetPart")
                    .WithPart("CommonPart")
                    .WithPart("WidgetPart")
                    .WithPart("LocalizationPart")
                    .WithSetting("Stereotype", "Widget"));

            var secretFormWidget = _widgetsService.CreateWidget(secretFormPageLayer.Id, "SecretFormWidget", "Cloud Storage Form Widget", "1", "Content");
            secretFormWidget.RenderTitle = false;
            _contentManager.Publish(secretFormWidget.ContentItem);

            #endregion

            #region Create Partners Requirements page

            //Create Partners Requirements content part
            ContentDefinitionManager.AlterPartDefinition(
                "PartnerReqsPagePart", builder => builder
                    .WithField("BannerImage", cfg => cfg
                        .OfType(typeof (MediaLibraryPickerField).Name)
                        .WithDisplayName("Banner Image")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif"))
                    .WithField("Subtitle", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Subtitle")
                        .WithSetting("TextFieldSettings.Flavor", "large")
                        .WithSetting("TextFieldSettings.Hint", "Title to be displayed beneath the banner."))
                    .WithField("Body", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithSetting("TextFieldSettings.Flavor", "html")
                        .WithDisplayName("Body"))
                    .WithField("ExternalLink", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("External Link")
                        .WithSetting("TextFieldSettings.Hint", "Url for the link at bottom of page."))
                    .WithField("LinkText", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Link Text")
                        .WithSetting("TextFieldSettings.Hint", "Text to be displayed in link at bottom of page."))
                    .WithField("Image", cfg => cfg
                        .OfType(typeof (MediaLibraryPickerField).Name)
                        .WithDisplayName("Image")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "Image to be displayed on right side of screen."))
                );

            //Create Partners Requirements content item type
            ContentDefinitionManager.AlterTypeDefinition(
                "PartnerReqsPage", cfg => cfg
                    .DisplayedAs("Simple Body with Image Page")
                    .WithPart("TitlePart")
                    .WithPart("PartnerReqsPagePart")
                    .WithPart("CommonPart")
                    .WithPart("MetaPart")
                    .WithPart("OpenGraphMetaTags")
                    .WithPart("SummaryCardsMetaTags")
                    .Draftable()
                    .Creatable()
                    .WithPart("AutoroutePart", builder => builder
                        .WithSetting("AutorouteSettings.AllowCustomPattern", "true")
                        .WithSetting("AutorouteSettings.AutomaticAdjustmentOnEdit", "false")
                        .WithSetting("AutorouteSettings.PatternDefinitions", "[{Name:'Title', Pattern: '{Content.Slug}', Description: '/page-title'}]")
                        .WithSetting("AutorouteSettings.DefaultPatternIndex", "0")));

            #endregion

            #region Create Overview page

            //Create Overview content part
            ContentDefinitionManager.AlterPartDefinition(
                "OverviewPagePart", builder => builder
                    .WithField("BannerImage", cfg => cfg
                        .OfType(typeof (MediaLibraryPickerField).Name)
                        .WithDisplayName("Banner Image")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif"))
                    .WithField("IntroText", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Intro Text")
                        .WithSetting("TextFieldSettings.Flavor", "large")
                        .WithSetting("TextFieldSettings.Hint", "Text displayed beneath the banner, but above the rows."))

                    .WithField("Row1Title", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Row 1 Title")
                        .WithSetting("TextFieldSettings.Flavor", "large"))
                    .WithField("Row1Body", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Row 1 Body")
                        .WithSetting("TextFieldSettings.Flavor", "textarea"))
                    .WithField("Row1ExternalLink", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Row 1 External Link"))
                    .WithField("Row1LinkText", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Row 1 Link Text")
                        .WithSetting("TextFieldSettings.Hint", "Text to be displayed in link."))

                    .WithField("Row1TileImage", cfg => cfg
                        .OfType(typeof (MediaLibraryPickerField).Name)
                        .WithDisplayName("Row 1 Tile Image")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "The image to be displayed in the top left corner of the tile."))
                    .WithField("Row1TileTitle", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Row 1 Tile Title")
                        .WithSetting("TextFieldSettings.Hint", "A title for the tile to the right of the body.")
                        .WithSetting("TextFieldSettings.Flavor", "large"))
                    .WithField("Row1TileBody", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithSetting("TextFieldSettings.Flavor", "textarea")
                        .WithDisplayName("Row 1 Tile Body"))
                    .WithField("Row1TileExternalLink", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Row 1 External Link"))
                    .WithField("Row1TileLinkText", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Row 1 Tile Link Text"))

                    .WithField("Row1Tile2Image", cfg => cfg
                        .OfType(typeof (MediaLibraryPickerField).Name)
                        .WithDisplayName("Row 1 Tile 2 Image")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "The image to be displayed in the top left corner of the tile."))
                    .WithField("Row1Tile2Title", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Row 1 Tile 2 Title")
                        .WithSetting("TextFieldSettings.Hint", "A title for the tile to the right of the body.")
                        .WithSetting("TextFieldSettings.Flavor", "large"))
                    .WithField("Row1Tile2Body", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithSetting("TextFieldSettings.Flavor", "textarea")
                        .WithDisplayName("Row 1 Tile 2 Body"))
                    .WithField("Row1Tile2ExternalLink", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Row 1 Tile 2 External Link"))
                    .WithField("Row1Tile2LinkText", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Row 1 Tile 2 Link Text"))

                    .WithField("Row2Title", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Row 2 Title")
                        .WithSetting("TextFieldSettings.Flavor", "large"))
                    .WithField("Row2Body", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithSetting("TextFieldSettings.Flavor", "textarea")
                        .WithDisplayName("Row 2 Body"))
                    .WithField("Row2ExternalLink", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Row 2 External Link"))
                    .WithField("Row2LinkText", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Row 2 Link Text")
                        .WithSetting("TextFieldSettings.Hint", "Text to be displayed in link."))

                    .WithField("Row2TileImage", cfg => cfg
                        .OfType(typeof (MediaLibraryPickerField).Name)
                        .WithDisplayName("Row 2 Tile Image")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "The image to be displayed in the top left corner of the tile."))
                    .WithField("Row2TileTitle", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Row 2 Tile Title")
                        .WithSetting("TextFieldSettings.Hint", "A title for the tile to the right of the body.")
                        .WithSetting("TextFieldSettings.Flavor", "large"))
                    .WithField("Row2TileBody", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Row 2 Tile Body")
                        .WithSetting("TextFieldSettings.Flavor", "textarea"))
                    .WithField("Row2TileExternalLink", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Row 2 Tile External Link"))
                    .WithField("Row2TileLinkText", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Row 2 Tile Link Text"))
                    .WithField("Row2Tile2Image", cfg => cfg
                        .OfType(typeof (MediaLibraryPickerField).Name)
                        .WithDisplayName("Row 2 Tile 2 Image")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "The image to be displayed in the top left corner of the tile."))

                    .WithField("Row2Tile2Title", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Row 2 Tile 2 Title")
                        .WithSetting("TextFieldSettings.Hint", "A title for the tile to the right of the body.")
                        .WithSetting("TextFieldSettings.Flavor", "large"))
                    .WithField("Row2Tile2Body", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Row 2 Tile 2 Body")
                        .WithSetting("TextFieldSettings.Flavor", "textarea"))
                    .WithField("Row2Tile2ExternalLink", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Row 2 Tile 2 External Link"))
                    .WithField("Row2Tile2LinkText", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Row 2 Tile 2 Link Text"))



                );

            //Create Overview content item type
            ContentDefinitionManager.AlterTypeDefinition(
                "OverviewPage", cfg => cfg
                    .DisplayedAs("Rows with Tiles Page")
                    .WithPart("TitlePart")
                    .WithPart("OverviewPagePart")
                    .WithPart("CommonPart")
                    .WithPart("MetaPart")
                    .WithPart("OpenGraphMetaTags")
                    .WithPart("SummaryCardsMetaTags")
                    .Draftable()
                    .Creatable()
                    .WithPart("AutoroutePart", builder => builder
                        .WithSetting("AutorouteSettings.AllowCustomPattern", "true")
                        .WithSetting("AutorouteSettings.AutomaticAdjustmentOnEdit", "false")
                        .WithSetting("AutorouteSettings.PatternDefinitions", "[{Name:'Title', Pattern: '{Content.Slug}', Description: '/page-title'}]")
                        .WithSetting("AutorouteSettings.DefaultPatternIndex", "0")));

            #endregion

            return 1;
        }

        public int UpdateFrom1() {

            SchemaBuilder.CreateTable("SharePointSettingsPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<string>("Username")
                    .Column<string>("Password")
                    .Column<string>("ContainingWebUrl")
                    .Column<string>("TargetListName")
                    .Column<string>("TargetListItemMetaType")
                );

            return 2;
        }

        public int UpdateFrom2() {
            ContentDefinitionManager.AlterPartDefinition(
                "OverviewPagePart", builder => builder

                    .WithField("IntroText", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Intro Text")
                        .WithSetting("TextFieldSettings.Flavor", "textarea")
                        .WithSetting("TextFieldSettings.Hint", "Text displayed beneath the banner, but above the rows.")));
            return 3;
        }


        public int UpdateFrom3() {

            SchemaBuilder.AlterTable("SharePointSettingsPartRecord", table => {
                table.AddColumn<string>("ApiSubmission_ContainingWebUrl");
                table.AddColumn<string>("ApiSubmission_TargetListName");
                table.AddColumn<string>("ApiSubmission_TargetListItemMetaType");
            });

            #region Create Activity Feed API Form Widget

            //Activity Feed API Form Layer
            var activityFeedAPIFormLayer = _widgetsService.CreateLayer("Activity Feed API Form Page", "Widgets on this layer are displayed on the Activity Feed API Form page", "url('~/programs/managementactivityapiform')");

            //Activity Feed API Form Page
            var activityFeedAPIFormPage = _contentManager.Create("Page");
            activityFeedAPIFormPage.As<TitlePart>().Title = "Office 365 Management Activity API Preview Program";

            var secretFormPageAutoPart = activityFeedAPIFormPage.As<AutoroutePart>();
            secretFormPageAutoPart.DisplayAlias = "/programs/managementactivityapiform";
            _autorouteService.GenerateAlias(secretFormPageAutoPart);
            _autorouteService.PublishAlias(secretFormPageAutoPart);
            _contentManager.Publish(activityFeedAPIFormPage);

            ContentDefinitionManager.AlterPartDefinition(typeof (ActivityFeedAPIFormWidgetPart).Name, builder => builder.Attachable());
            ContentDefinitionManager.AlterTypeDefinition(
                "ActivityFeedAPIFormWidget",
                cfg => cfg
                    .WithPart(typeof (ActivityFeedAPIFormWidgetPart).Name)
                    .WithPart("CommonPart")
                    .WithPart("WidgetPart")
                    .WithPart("LocalizationPart")
                    .WithSetting("Stereotype", "Widget"));

            var activityFeedAPIFormWidget = _widgetsService.CreateWidget(activityFeedAPIFormLayer.Id, "ActivityFeedAPIFormWidget", "Activity Feed API Form Widget", "1", "Content");
            activityFeedAPIFormWidget.RenderTitle = false;
            _contentManager.Publish(activityFeedAPIFormWidget.ContentItem);

            #endregion

            return 4;
        }

        public int UpdateFrom4() {

            SchemaBuilder.CreateTable(typeof (EmailSettingsPartRecord).Name, table => table
                .ContentPartRecord()
                .Column<string>("SendGridAccountName")
                .Column<string>("SendGridAccountPassword")
                .Column<string>("ActivityFeedApiFromEmailAddress")
                .Column<string>("ActivityFeedApiFromEmailTitle")
                .Column<string>("ActivityFeedApiEmailSubject")
                .Column<string>("ActivityFeedApiEmailText", column => column.WithLength(10000))
                .Column<string>("ActivityFeedApiEmailHtml", column => column.WithLength(10000))
                .Column<string>("CloudStorageFromEmailAddress")
                .Column<string>("CloudStorageFromEmailTitle")
                .Column<string>("CloudStorageEmailSubject")
                .Column<string>("CloudStorageEmailText", column => column.WithLength(10000))
                .Column<string>("CloudStorageEmailHtml", column => column.WithLength(10000)));

            return 5;
        }




        public int UpdateFrom5() {

            #region Creating custom part - RowWithTiles

            SchemaBuilder.CreateTable("RowWithTilesRecord", table => table
                .ContentPartRecord()
                .Column<string>("TilesJson", column => column.Unlimited())
                );

            ContentDefinitionManager.AlterPartDefinition(
                typeof(RowWithTilesPart).Name, cfg => cfg.Attachable());

            #endregion

            ContentDefinitionManager.AlterTypeDefinition(
               "OverviewPage", cfg => cfg
                   .WithPart("RowWithTilesPart"));

            return 6;

        }

        public int UpdateFrom6()
        {

            SchemaBuilder.CreateTable("SingleRowWithTilesRecord", table => table
                .ContentPartRecord()
                  .Column<int>("RowWithTilesRecord_Id")
                  .Column<string>("Title")
                  .Column<string>("Body")
                  .Column<string>("External_Link")
                  .Column<int>("SortOrder")
                  .Column<string>("Tile_1_Title")
                  .Column<string>("Tile_1_External_Link")
                  .Column<string>("Tile_1_Thumbnail")
                  .Column<string>("Tile_2_Title")
                  .Column<string>("Tile_2_External_Link")
                  .Column<string>("Tile_2_Thumbnail"));


            return 7;
        }

        public int UpdateFrom7() {
            ContentDefinitionManager.AlterPartDefinition(
              typeof(RowWithTilesPart).Name, cfg => cfg.Attachable());
          

            ContentDefinitionManager.AlterTypeDefinition(
              "OverviewPage", cfg => cfg
                  .WithPart("RowWithTilesPart"));
            return 8;
        }
    }
}