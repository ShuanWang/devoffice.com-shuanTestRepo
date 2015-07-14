using System;
using System.Data;
using DevOffice.Common.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Common.Fields;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Widgets.Models;
using Orchard.Widgets.Services;
using Provoke.Highlights.Models;
using Provoke.Highlights.Models.Widgets;
using System.Linq;

namespace Provoke.Highlights {
    public class DatabaseMigrations : DataMigrationImpl {
        public int Create()
        {
            #region Create Scenario, Task, and Step custom content parts

            SchemaBuilder.CreateTable("ScenarioRecord",
                table => table
                    .ContentPartRecord() // This will add a column 'Id', set it as the primary key and be an identity.
                    .Column<string>("Title")
                    .Column<string>("Description", column => column.Unlimited())
            );

            SchemaBuilder.CreateTable("TaskRecord",
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<int>("ScenarioRecord_Id")
                    .Column<string>("Description", column => column.Unlimited())
                    .Column<int>("SortOrder")
            );

            SchemaBuilder.CreateTable("StepRecord",
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<int>("TaskRecord_Id")
                    .Column<string>("Title")
                    .Column<string>("Description", column => column.Unlimited())
                    .Column<int>("SortOrder")
                    .Column<double>("TopPosition")
                    .Column<double>("LeftPosition")
                    .Column<string>("Anchor")
                    .Column<string>("Image")
            );

            ContentDefinitionManager.AlterPartDefinition(
                typeof(ScenarioPart).Name,
                builder => builder.Attachable()
            );

            #endregion

            #region Create Highlight content type

            ContentDefinitionManager.AlterPartDefinition("HighlightPart",
                builder => builder
                    .WithField("Version", definitionBuilder => definitionBuilder
                        .OfType("NumericField")
                        .WithDisplayName("Version")
                        .WithSetting("NumericFieldSettings.Hint", "The current version of the highlight"))
            );

            ContentDefinitionManager.AlterTypeDefinition("Highlight",
                builder => builder
                    .DisplayedAs("Highlight")
                    .WithPart("TitlePart")
                    .WithPart(typeof(ScenarioPart).Name)
                    .WithPart("HighlightsPart")
                    .WithPart("CommonPart")
                    //.WithPart("AutoroutePart", definitionBuilder => definitionBuilder
                    //    .WithSetting("AutorouteSettings.AllowCustomPattern", "true")
                    //    .WithSetting("AutorouteSettings.AutomaticAdjustmentOnEdit", "false")
                    //    .WithSetting("AutorouteSettings.PatternDefinitions", "[{Name:'Title', Pattern: '{Content.Slug}', Description: '/page-title'}]")
                    //    .WithSetting("AutorouteSettings.DefaultPatternIndex", "0"))
                    .Draftable()
                    .Creatable()
            );

            #endregion

            return 1;
        }

        public int UpdateFrom1()
        {
            #region Alter the Tasks table to include Title<string> and Duration<string> columns

            SchemaBuilder.AlterTable("TaskRecord",
                table => table
                    .AddColumn<string>("Title")
                );
            SchemaBuilder.AlterTable("TaskRecord",
                table => table
                    .AddColumn<string>("Duration")
                );

            #endregion

            return 2;
        }
        public int UpdateFrom2()
        {

            #region Make the related resources table, attach it to Highlight
            SchemaBuilder.CreateTable("RelatedResourceRecord",
               table => table
                   .Column<int>("Id", column => column.PrimaryKey().Identity())
                   .Column<int>("ScenarioRecord_Id")
                   .Column<string>("Title")
                   .Column<string>("Type")
                   .Column<string>("Url")
                   .Column<int>("SortOrder")
           );

            ContentDefinitionManager.AlterPartDefinition(
                typeof(RelatedResourcePart).Name, cfg => cfg.Attachable());

            ContentDefinitionManager.AlterTypeDefinition(
                "Highlight", cfg => cfg
                    .WithPart("RelatedResourcePart"));

            return 3;
            #endregion
        }

    }

    public class WidgetMigrations : DataMigrationImpl {
        private readonly IContentManager _contentManager;
        private readonly IWidgetsService _widgetsService;

        private LayerPart _homePageLayer;

        public LayerPart HomePageLayer {
            get { return _homePageLayer ?? (_homePageLayer = _widgetsService.GetLayers().Single(layer => layer.Name == "TheHomepage")); }
        }

        public WidgetMigrations(IContentManager contentManager, IWidgetsService widgetsService) {
            _contentManager = contentManager;
            _widgetsService = widgetsService;
        }

        public int Create() {
            #region Create Highlights Widget

            ContentDefinitionManager.AlterPartDefinition(typeof (HighlightsWidgetPart).Name,
                builder => builder.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("HighlightsWidget",
                cfg => cfg
                    .WithPart("WidgetPart")
                    .WithPart("IdentityPart")
                    .WithPart("HighlightsWidgetPart")
                    .WithPart("CommonPart")
                    .WithPart("LocalizationPart")
                    .WithSetting("Stereotype", "Widget")
                );

            ContentDefinitionManager.AlterPartDefinition(typeof(HighlightsWidgetPart).Name,
                builder => builder
                    .WithField("HighlightPicker", cfg =>
                        cfg
                        .OfType("ContentPickerField")
                        .WithDisplayName("Highlight Picker")
                        .WithSetting("ContentPickerFieldSettings.Multiple", "True")
                        .WithSetting("ContentPickerFieldSettings.ShowContentTab", "True")
                        .WithSetting("ContentPickerFieldSettings.DisplayedContentTypes", "Highlight")
                        .WithSetting("ContentPickerFieldSettings.Hint", "Choose a highlight to link to this widget"))
            );

            #endregion

            
            // This will be done manually in production, but for dev purposes, this can be uncommented to put a Highlights widget on the home page.
            //#region Place Highlights Widget on Home Page

            //WidgetPart highlightsWidget = _widgetsService.CreateWidget(HomePageLayer.Id, "HighlightsWidget", "Highlights Widget", "1", "Content");
            //highlightsWidget.RenderTitle = false;
            //highlightsWidget.Name = "HighlightsWidget";
            //_contentManager.Publish(highlightsWidget.ContentItem);

            //#endregion

            return 1;
        }
        public int UpdateFrom1()
        {

            ContentDefinitionManager.AlterPartDefinition(
                "CommonHighlightPart", builder => builder
                    .WithDescription("Fields common to all modules within a highlight content type")

                    .WithField("PageIntro", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("Page Intro")
                        .WithSetting("TextFieldSettings.Flavor", "html")
                        .WithSetting("TextFieldSettings.Hint", "Text to appear beneath the page title and above the labs"))

                    .WithField("LabIntro", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("Lab Intro")
                        .WithSetting("TextFieldSettings.Flavor", "large")
                        .WithSetting("TextFieldSettings.Hint", "Text to appear above the labs (ex. 'Work your way through the Hands on Labs')"))
                    .WithField("RelatedResourcesIntro", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("Related Resource Intro")
                        .WithSetting("TextFieldSettings.Flavor", "html")
                        .WithSetting("TextFieldSettings.Hint", "Text to appear above the list of related resources"))
            );

            ContentDefinitionManager.AlterTypeDefinition(
    "HighlightsWidget", cfg => cfg
        .WithPart("CommonHighlightPart"));

            return 2;
        }
    }
}