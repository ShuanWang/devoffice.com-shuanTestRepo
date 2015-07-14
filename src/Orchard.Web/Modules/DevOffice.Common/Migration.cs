using System.CodeDom;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using DevOffice.Common.Models;
using DevOffice.Common.Templates;
using DevOffice.Common.ViewModels;
using Orchard;
using Orchard.Autoroute.Models;
using Orchard.Autoroute.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Common.Fields;
using Orchard.Core.Common.Models;
using Orchard.Core.Contents.Extensions;
using Orchard.Core.Navigation.Models;
using Orchard.Core.Navigation.Services;
using Orchard.Core.Title.Models;
using Orchard.Data;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;
using Orchard.Fields.Fields;
using Orchard.Indexing;
using Orchard.Indexing.Services;
using Orchard.MediaLibrary.Fields;
using Orchard.Projections.Models;
using Orchard.Search.Models;
using Orchard.Taxonomies.Fields;
using Orchard.Taxonomies.Models;
using Orchard.Taxonomies.Services;
using Orchard.Widgets.Services;
using Sunkist.FeaturedItemSlider.Models;
using System;
using System.Data;
using System.Linq;

namespace DevOffice.Common
{
    [OrchardFeature("DevOffice.Common")]
    public class Migrations : DataMigrationImpl {
        private readonly IAutorouteService _autorouteService;
        private readonly IMenuService _menuService;
        private readonly IContentManager _contentManager;
        private readonly IWidgetsService _widgetsService;
        private readonly IHtmlTemplateProvider _templateProvider;
        private readonly IRepository<QueryPartRecord> _queryRepository;
        private readonly ITaxonomyService _taxonomyService;

        public Migrations(
            IAutorouteService autorouteService,
            IMenuService menuService,
            IRepository<QueryPartRecord> queryRepository,
            IContentManager contentManager,
            IWidgetsService widgetService,
            ITaxonomyService taxonomyService
            ) {
            _autorouteService = autorouteService;
            _menuService = menuService;
            _contentManager = contentManager;
            _widgetsService = widgetService;
            _queryRepository = queryRepository;
            _taxonomyService = taxonomyService;
            _templateProvider = new FileSystemTemplateProvider();
        }


        public int Create() {
            #region Create Global Navigation and Menu

            var defaultLayer = _widgetsService.GetLayers().FirstOrDefault(l => l.Name == "Default");

            var navGlobal = _menuService.Create("Global Navigation");

            var menuWidgetGlobal = _widgetsService.CreateWidget(defaultLayer.Id, "MenuWidget", "Global Navigation", "5", "Navigation");
            menuWidgetGlobal.RenderTitle = false;
            menuWidgetGlobal.As<MenuWidgetPart>().MenuContentItemId = navGlobal.ContentItem.Id;
            menuWidgetGlobal.As<MenuWidgetPart>().Levels = 0;
            _contentManager.Publish(menuWidgetGlobal.ContentItem);

            #endregion

            #region Create Pages and Navigation Items

            //Create HOMEPAGE
            var pageHome = _contentManager.Create("Page");
            pageHome.As<TitlePart>().Title = "Homepage";
            pageHome.As<BodyPart>().Text = String.Empty;
            _contentManager.Publish(pageHome);

            //Update alias/url to be root
            var pageHomeArp = pageHome.As<AutoroutePart>();
            pageHomeArp.DisplayAlias = String.Empty;
            _autorouteService.GenerateAlias(pageHomeArp);
            _autorouteService.PublishAlias(pageHomeArp);

            //Create OPPORTUNITY Page
            var pageOpportunity = _contentManager.Create("StepPage");
            pageOpportunity.As<TitlePart>().Title = "Opportunity";
            pageOpportunity.As<BodyPart>().Text = String.Empty;

            var pageWhyArp = pageOpportunity.As<AutoroutePart>();
            pageWhyArp.DisplayAlias = "opportunity";
            _autorouteService.GenerateAlias(pageWhyArp);
            _autorouteService.PublishAlias(pageWhyArp);

            var pageWhyMenu = pageOpportunity.As<MenuPart>();
            pageWhyMenu.Menu = navGlobal;
            pageWhyMenu.MenuText = pageOpportunity.As<TitlePart>().Title;
            pageWhyMenu.MenuPosition = "1";

            //Create BUILD Page
            var pageBuild = _contentManager.Create("StepPage");
            pageBuild.As<TitlePart>().Title = "Build";
            pageBuild.As<BodyPart>().Text = String.Empty;

            var pageHowArp = pageBuild.As<AutoroutePart>();
            pageHowArp.DisplayAlias = "build";
            _autorouteService.GenerateAlias(pageHowArp);
            _autorouteService.PublishAlias(pageHowArp);

            var pageHowMenu = pageBuild.As<MenuPart>();
            pageHowMenu.Menu = navGlobal;
            pageHowMenu.MenuText = pageBuild.As<TitlePart>().Title;
            pageHowMenu.MenuPosition = "2";

            //Create TRANSFORM Page
            var pageTransform = _contentManager.Create("StepPage");
            pageTransform.As<TitlePart>().Title = "Transform";
            pageTransform.As<BodyPart>().Text = String.Empty;

            var pageMigrateArp = pageTransform.As<AutoroutePart>();
            pageMigrateArp.DisplayAlias = "transform";
            _autorouteService.GenerateAlias(pageMigrateArp);
            _autorouteService.PublishAlias(pageMigrateArp);

            var pageMigrateMenu = pageTransform.As<MenuPart>();
            pageMigrateMenu.Menu = navGlobal;
            pageMigrateMenu.MenuText = pageTransform.As<TitlePart>().Title;
            pageMigrateMenu.MenuPosition = "3";

            //Create TRAINING Page
            var pageTraining = _contentManager.Create("Page");
            pageTraining.As<TitlePart>().Title = "Training";
            pageTraining.As<BodyPart>().Text = String.Empty;

            var pageTrainingArp = pageTraining.As<AutoroutePart>();
            pageTrainingArp.DisplayAlias = "training";
            _autorouteService.GenerateAlias(pageTrainingArp);
            _autorouteService.PublishAlias(pageTrainingArp);

            var pageTrainingMenu = pageTraining.As<MenuPart>();
            pageTrainingMenu.Menu = navGlobal;
            pageTrainingMenu.MenuText = pageTraining.As<TitlePart>().Title;
            pageTrainingMenu.MenuPosition = "4";

            //Create EVENTS Page
            var pageEvents = _contentManager.Create("Page");
            pageEvents.As<TitlePart>().Title = "Events";
            pageEvents.As<BodyPart>().Text = String.Empty;

            var pageEventsArp = pageEvents.As<AutoroutePart>();
            pageEventsArp.DisplayAlias = "events";
            _autorouteService.GenerateAlias(pageEventsArp);
            _autorouteService.PublishAlias(pageEventsArp);

            var pageEventsMenu = pageEvents.As<MenuPart>();
            pageEventsMenu.Menu = navGlobal;
            pageEventsMenu.MenuText = pageEvents.As<TitlePart>().Title;
            pageEventsMenu.MenuPosition = "6";

            _contentManager.Publish(pageOpportunity);
            _contentManager.Publish(pageBuild);
            _contentManager.Publish(pageTransform);
            _contentManager.Publish(pageTraining);
            _contentManager.Publish(pageEvents);

            #endregion

            #region Create Layers for each Page

            var opportunityLayer = _widgetsService.CreateLayer("Opportunity", "The widgets in this layer are displayed on the opportunity page.", "url '~/opportunity'");
            var buildLayer = _widgetsService.CreateLayer("Build", "The widgets in this layer are displayed on the build page.", "url '~/build'");
            var transformLayer = _widgetsService.CreateLayer("Transform", "The widgets in this layer are displayed on the transforms page.", "url '~/transform'");
            var trainingLayer = _widgetsService.CreateLayer("Training", "The widgets in this layer are displayed on the training page.", "url '~/training'");
            var eventsLayer = _widgetsService.CreateLayer("Events", "The widgets in this layer are displayed on the events page.", "url '~/events'");
            var notHomepage = _widgetsService.CreateLayer("Not Homepage", "The widgets in this layer are displayed everywhere except the homepage", "not url '~/'");
            var homepageLayer = _widgetsService.GetLayers().FirstOrDefault(l => l.Name == "TheHomepage");

            #endregion

            #region Create QuickLinks Content Type

            ContentDefinitionManager.AlterPartDefinition(
                "QuickLinksPart", builder => builder
                    .WithDescription("Quick links")

                    .WithField("ExternalLink", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("External Link")
                        .WithSetting("TextFieldSettings.Flavor", "wide")
                        .WithSetting("TextFieldSettings.Hint", "Enter the link. Ex. https://www.microsoft.com"))

                    .WithField("SubText", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Sub Text")
                        .WithSetting("TextFieldSettings.Flavor", "textarea")
                        .WithSetting("TextFieldSettings.Hint", "Enter sub text."))

                    .WithField("SmallImage", cfg => cfg
                        .OfType(typeof (MediaLibraryPickerField).Name)
                        .WithDisplayName("Small Image")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "The selected image will be used in the quicklinks in the header."))

                    .WithField("BigImage", cfg => cfg
                        .OfType(typeof (MediaLibraryPickerField).Name)
                        .WithDisplayName("Big Image")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "The selected image will be used in the quicklinks in the home page."))

                    .WithField("SortOrder", cfg => cfg
                        .OfType(typeof (NumericField).Name)
                        .WithDisplayName("Sort Order")
                        .WithSetting("NumericFieldSettings.Hint", "Enter a number to determine the order in which this item is displayed on the widgets."))

                );

            ContentDefinitionManager.AlterTypeDefinition(
                "QuickLinks", cfg => cfg
                    .DisplayedAs("Quick Links Item")
                    .WithPart("TitlePart")
                    .WithPart("QuickLinksPart")
                    .WithPart("CommonPart")
                    .Draftable()
                    .Creatable()
                );

            #endregion

            #region Create Event Content Type

            ContentDefinitionManager.AlterPartDefinition(
                "EventPart", builder => builder
                    .WithDescription("A single event")

                    .WithField("SubText", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Sub Text")
                        .WithSetting("TextFieldSettings.Flavor", "textarea")
                        .WithSetting("TextFieldSettings.Hint", "Enter sub text."))

                    .WithField("StartDate", cfg => cfg
                        .OfType("DateTimeField")
                        .WithDisplayName("Start Date")
                        .WithSetting("DateTimeFieldSettings.Display", "DateOnly")
                        .WithSetting("DateTimeFieldSettings.Required", "true"))

                    .WithField("EndDate", cfg => cfg
                        .OfType("DateTimeField")
                        .WithDisplayName("End Date")
                        .WithSetting("DateTimeFieldSettings.Display", "DateOnly"))

                    .WithField("ExternalLink", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("External Link")
                        .WithSetting("TextFieldSettings.Hint", "Enter the link to the external resource. Ex. https://www.microsoft.com"))

                    .WithField("Location", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Location")
                        .WithSetting("TextFieldSettings.Hint", "Enter the location of the event."))

                    .WithField("Image", cfg => cfg
                        .OfType(typeof (MediaLibraryPickerField).Name)
                        .WithDisplayName("Image")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "This will be the event thumbnail."))

                    .WithField("EventType", cfg => cfg
                        .OfType(typeof (EnumerationField).Name)
                        .WithDisplayName("Event Type")
                        .WithSetting("EnumerationFieldSettings.Options",
                            string.Join(System.Environment.NewLine,
                                new[] {"Microsoft Events", "3rd Party Events", "Recent Events"}))
                        .WithSetting("EnumerationField.Hint", "Select event type"))
                );

            ContentDefinitionManager.AlterTypeDefinition(
                "Event", cfg => cfg
                    .DisplayedAs("Event")
                    .WithPart("TitlePart")
                    .WithPart("EventPart")
                    .WithPart("CommonPart")
                    .Draftable()
                    .Creatable()
                );

            #endregion

            #region Create Community Content Type

            ContentDefinitionManager.AlterPartDefinition(
                "CommunityPart", builder => builder
                    .WithDescription("Additional resources from the community.")

                    .WithField("SubText", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Sub Text")
                        .WithSetting("TextFieldSettings.Flavor", "textarea")
                        .WithSetting("TextFieldSettings.Hint", "Enter sub text."))


                    .WithField("ItemDescription", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Item Description")
                        .WithSetting("TextFieldSettings.Flavor", "textarea")
                        .WithSetting("TextFieldSettings.Hint", "Enter description text."))

                    .WithField("ExternalLink", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("External Link")
                        .WithSetting("TextFieldSettings.Hint", "Enter the link to the external resource. Ex. https://www.microsoft.com"))

                    .WithField("Image", cfg => cfg
                        .OfType(typeof (MediaLibraryPickerField).Name)
                        .WithDisplayName("Image")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "The selected image will be used in conjuction with the text of the community resource description."))

                );

            ContentDefinitionManager.AlterTypeDefinition(
                "Community", cfg => cfg
                    .DisplayedAs("Community Item")
                    .WithPart("TitlePart")
                    .WithPart("CommunityPart")
                    .WithPart("CommonPart")
                    .Draftable()
                    .Creatable()
                );

            #endregion

            #region Create Training Content Type

            ContentDefinitionManager.AlterPartDefinition(
                "TrainingPart", builder => builder
                    .WithDescription("A single training")

                    .WithField("SubText", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Sub Text")
                        .WithSetting("TextFieldSettings.Flavor", "textarea")
                        .WithSetting("TextFieldSettings.Hint", "Enter sub text."))

                    .WithField("StartDate", cfg => cfg
                        .OfType("DateTimeField")
                        .WithDisplayName("Start Date")
                        .WithSetting("DateTimeFieldSettings.Display", "DateOnly"))

                    .WithField("EndDate", cfg => cfg
                        .OfType("DateTimeField")
                        .WithDisplayName("End Date")
                        .WithSetting("DateTimeFieldSettings.Display", "DateOnly"))

                    .WithField("ExternalLink", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("External Link")
                        .WithSetting("TextFieldSettings.Hint", "Enter the link to the external resource. Ex. https://www.microsoft.com"))

                    .WithField("Location", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Location")
                        .WithSetting("TextFieldSettings.Hint", "Enter the location of the training."))

                    .WithField("Image", cfg => cfg
                        .OfType(typeof (MediaLibraryPickerField).Name)
                        .WithDisplayName("Image")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "This will be the training thumbnail."))

                    .WithField("TrainingType", cfg => cfg
                        .OfType(typeof (EnumerationField).Name)
                        .WithDisplayName("Training Type")
                        .WithSetting("EnumerationFieldSettings.Options",
                            string.Join(System.Environment.NewLine,
                                new[] {"Platform Overview", "Delving Deeper", "Advanced Topics"}))
                        .WithSetting("EnumerationField.Hint", "Select training type"))
                );

            ContentDefinitionManager.AlterTypeDefinition(
                "Training", cfg => cfg
                    .DisplayedAs("Training")
                    .WithPart("TitlePart")
                    .WithPart("TrainingPart")
                    .WithPart("CommonPart")
                    .Draftable()
                    .Creatable()
                );

            #endregion

            #region Create Footer Widget

            var widgetFooter = _widgetsService.CreateWidget(defaultLayer.Id, "HtmlWidget", "Footer", "1", "Footer");
            widgetFooter.RenderTitle = false;
            widgetFooter.As<BodyPart>().Text = _templateProvider.GetTemplateContent("Footer");
            _contentManager.Publish(widgetFooter.ContentItem);

            #endregion

            #region Update Page Content Type

            ContentDefinitionManager.AlterPartDefinition(
                "PagePart", builder => builder
                    .WithDescription("Page")

                    .WithField("BannerImage", cfg => cfg
                        .OfType(typeof (MediaLibraryPickerField).Name)
                        .WithDisplayName("Banner Image")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "The selected image will be displayed as the page banner below the navigation.")
                    )
                );

            ContentDefinitionManager.AlterTypeDefinition(
                "Page", cfg => cfg
                    .WithPart("PagePart")
                );

            #endregion

            #region Create Feedback Record Table

            SchemaBuilder.CreateTable("FeedbackInformationRecord", table => table
                .Column("Id", DbType.Int32, column => column.PrimaryKey().Identity())
                .Column("IsHelpful", DbType.Boolean)
                .Column("FeedbackContent", DbType.String)
                .Column("DateCreated", DbType.DateTime)
                );

            #endregion

            #region Create Custom Widgets (Apps, Articles, Feedback)

            //Apps
            ContentDefinitionManager.AlterPartDefinition(
                typeof (AppsWidgetPart).Name, cfg => cfg.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("AppsWidget", cfg => cfg
                .WithPart("WidgetPart")
                .WithPart(typeof (AppsWidgetPart).Name)
                .WithPart("CommonPart")
                .WithSetting("Stereotype", "Widget"));

            //Articles
            ContentDefinitionManager.AlterPartDefinition(
                typeof (ArticlesWidgetPart).Name, cfg => cfg.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("ArticlesWidget", cfg => cfg
                .WithPart("WidgetPart")
                .WithPart(typeof (ArticlesWidgetPart).Name)
                .WithPart("CommonPart")
                .WithSetting("Stereotype", "Widget"));

            //Feedback
            ContentDefinitionManager.AlterPartDefinition(
                typeof (FeedbackWidgetPart).Name, cfg => cfg.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("FeedbackWidget", cfg => cfg
                .WithPart("WidgetPart")
                .WithPart(typeof (FeedbackWidgetPart).Name)
                .WithPart("CommonPart")
                .WithSetting("Stereotype", "Widget"));

            #endregion

            #region Create Home Articles Widget

            // Creating table MapRecord
            SchemaBuilder.CreateTable("ArticlesWidgetRecord", table => table
                .ContentPartRecord()
                .Column("BlogPostsRssUrl", DbType.String)
                .Column("ArticlesRssUrl", DbType.String)
                .Column("MvpBlogsRssUrl", DbType.String)
                );

            ContentDefinitionManager.AlterPartDefinition(
                typeof (ArticlesWidgetPart).Name, cfg => cfg.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("ArticlesWidget", cfg => cfg
                .WithPart("ArticlesWidgetPart")
                .WithPart("WidgetPart")
                .WithPart("CommonPart")
                .WithSetting("Stereotype", "Widget"));

            var articlesWidget = _widgetsService.CreateWidget(homepageLayer.Id, "ArticlesWidget", "Home Articles", "5", "AfterContent");
            articlesWidget.RenderTitle = false;
            articlesWidget.Name = "home-articles";
            _contentManager.Publish(articlesWidget.ContentItem);

            #endregion

            #region Create Home Events Widget

            ContentDefinitionManager.AlterPartDefinition(
                typeof (HomeEventsPart).Name, cfg => cfg.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("HomeEventsWidget",
                cfg => cfg
                    .WithPart("WidgetPart")
                    .WithPart("HomeEventsPart")
                    .WithPart("CommonPart")
                    .WithSetting("Stereotype", "Widget")
                );

            var homeEventsWidget = _widgetsService.CreateWidget(homepageLayer.Id, "HomeEventsWidget", "Home Events", "7", "AfterContent");
            homeEventsWidget.RenderTitle = false;
            homeEventsWidget.Name = "home-events";
            _contentManager.Publish(homeEventsWidget.ContentItem);

            #endregion

            #region Create Feedback Widget

            var feedbackWidget = _widgetsService.CreateWidget(notHomepage.Id, "FeedbackWidget", "Feedback", "5", "AfterMain");
            feedbackWidget.RenderTitle = false;
            feedbackWidget.Name = "feedback";
            _contentManager.Publish(feedbackWidget.ContentItem);

            #endregion

            #region Create Quick Links Header Widget

            ContentDefinitionManager.AlterPartDefinition(
                typeof (HeaderQuickLinksPart).Name, cfg => cfg.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("HeaderQuickLinksWidget",
                cfg => cfg
                    .WithPart("WidgetPart")
                    .WithPart("HeaderQuickLinksPart")
                    .WithPart("CommonPart")
                    .WithSetting("Stereotype", "Widget")
                );

            var quickLinksHeaderWidget = _widgetsService.CreateWidget(defaultLayer.Id, "HeaderQuickLinksWidget", "Header Quick Links", "10", "Header");
            quickLinksHeaderWidget.RenderTitle = false;
            quickLinksHeaderWidget.Name = "header-quick-links";
            _contentManager.Publish(quickLinksHeaderWidget.ContentItem);

            #endregion

            #region Create Quick Links Home Widget

            ContentDefinitionManager.AlterPartDefinition(
                typeof (HomeQuickLinksPart).Name, cfg => cfg.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("HomeQuickLinksWidget",
                cfg => cfg
                    .WithPart("WidgetPart")
                    .WithPart("HomeQuickLinksPart")
                    .WithPart("CommonPart")
                    .WithSetting("Stereotype", "Widget")
                );

            var quickLinksHomeWidget = _widgetsService.CreateWidget(homepageLayer.Id, "HomeQuickLinksWidget", "Home Quick Links", "5", "BeforeContent");
            quickLinksHomeWidget.RenderTitle = false;
            quickLinksHomeWidget.Name = "home-quick-links";
            _contentManager.Publish(quickLinksHomeWidget.ContentItem);

            #endregion

            #region Create Feature Slider Widget - HOME PAGE

            // Create "Featured Item Group".
            var featuresGroup = _contentManager.Create("FeaturedItemGroup");
            featuresGroup.As<FeaturedItemGroupPart>().Name = "Home Landing Page";
            featuresGroup.As<FeaturedItemGroupPart>().GroupWidth = 0;
            featuresGroup.As<FeaturedItemGroupPart>().GroupHeight = 0;
            featuresGroup.As<FeaturedItemGroupPart>().IncludeImages = true;
            featuresGroup.As<FeaturedItemGroupPart>().ImageWidth = 0;
            featuresGroup.As<FeaturedItemGroupPart>().ImageHeight = 0;
            featuresGroup.As<FeaturedItemGroupPart>().BackgroundColor = "eb3b00";
            featuresGroup.As<FeaturedItemGroupPart>().ForegroundColor = "ffffff";
            featuresGroup.As<FeaturedItemGroupPart>().SlideSpeed = 700;
            featuresGroup.As<FeaturedItemGroupPart>().SlidePause = 7000;
            featuresGroup.As<FeaturedItemGroupPart>().ShowPager = true;
            featuresGroup.As<FeaturedItemGroupPart>().TransitionEffect = "blindX";
            featuresGroup.As<FeaturedItemGroupPart>().ImageStyle = ImageStyle.Background;
            _contentManager.Publish(featuresGroup);

            // Add "Featured Item Slider Widget" to "Homepage" layer.
            var featuredItemSliderWidget = _widgetsService.CreateWidget(homepageLayer.Id, "FeaturedItemSliderWidget", "Home Landing Page", "1", "Featured");
            featuredItemSliderWidget.RenderTitle = false;
            featuredItemSliderWidget.Name = "home-featured-item-slider";
            featuredItemSliderWidget.As<FeaturedItemSliderWidgetPart>().GroupName = "Home Landing Page";
            _contentManager.Publish(featuredItemSliderWidget.ContentItem);

            #endregion

            #region Create Feature Slider Widget - OPPORTUNITY

            // Create "Featured Item Group".
            var featuresGroupOpportunity = _contentManager.Create("FeaturedItemGroup");
            featuresGroupOpportunity.As<FeaturedItemGroupPart>().Name = "Opportunity Page";
            featuresGroupOpportunity.As<FeaturedItemGroupPart>().GroupWidth = 0;
            featuresGroupOpportunity.As<FeaturedItemGroupPart>().GroupHeight = 0;
            featuresGroupOpportunity.As<FeaturedItemGroupPart>().IncludeImages = true;
            featuresGroupOpportunity.As<FeaturedItemGroupPart>().ImageWidth = 0;
            featuresGroupOpportunity.As<FeaturedItemGroupPart>().ImageHeight = 0;
            featuresGroupOpportunity.As<FeaturedItemGroupPart>().BackgroundColor = "eb3b00";
            featuresGroupOpportunity.As<FeaturedItemGroupPart>().ForegroundColor = "ffffff";
            featuresGroupOpportunity.As<FeaturedItemGroupPart>().SlideSpeed = 700;
            featuresGroupOpportunity.As<FeaturedItemGroupPart>().SlidePause = 7000;
            featuresGroupOpportunity.As<FeaturedItemGroupPart>().ShowPager = true;
            featuresGroupOpportunity.As<FeaturedItemGroupPart>().TransitionEffect = "blindX";
            featuresGroupOpportunity.As<FeaturedItemGroupPart>().ImageStyle = ImageStyle.Background;
            _contentManager.Publish(featuresGroupOpportunity);

            // Add "Featured Item Slider Widget" to "Opportunity" layer.
            var featuredItemSliderWidgetOpportunity = _widgetsService.CreateWidget(opportunityLayer.Id, "FeaturedItemSliderWidget", "Opportunity Page", "1", "Featured");
            featuredItemSliderWidgetOpportunity.RenderTitle = false;
            featuredItemSliderWidgetOpportunity.Name = "opportunity-featured-item-slider";
            featuredItemSliderWidgetOpportunity.As<FeaturedItemSliderWidgetPart>().GroupName = "Opportunity Page";
            _contentManager.Publish(featuredItemSliderWidgetOpportunity.ContentItem);

            #endregion

            #region Create Feature Slider Widget - BUILD

            // Create "Featured Item Group".
            var featuresGroupBuild = _contentManager.Create("FeaturedItemGroup");
            featuresGroupBuild.As<FeaturedItemGroupPart>().Name = "Build Page";
            featuresGroupBuild.As<FeaturedItemGroupPart>().GroupWidth = 0;
            featuresGroupBuild.As<FeaturedItemGroupPart>().GroupHeight = 0;
            featuresGroupBuild.As<FeaturedItemGroupPart>().IncludeImages = true;
            featuresGroupBuild.As<FeaturedItemGroupPart>().ImageWidth = 0;
            featuresGroupBuild.As<FeaturedItemGroupPart>().ImageHeight = 0;
            featuresGroupBuild.As<FeaturedItemGroupPart>().BackgroundColor = "eb3b00";
            featuresGroupBuild.As<FeaturedItemGroupPart>().ForegroundColor = "ffffff";
            featuresGroupBuild.As<FeaturedItemGroupPart>().SlideSpeed = 700;
            featuresGroupBuild.As<FeaturedItemGroupPart>().SlidePause = 7000;
            featuresGroupBuild.As<FeaturedItemGroupPart>().ShowPager = true;
            featuresGroupBuild.As<FeaturedItemGroupPart>().TransitionEffect = "blindX";
            featuresGroupBuild.As<FeaturedItemGroupPart>().ImageStyle = ImageStyle.Background;
            _contentManager.Publish(featuresGroupBuild);

            // Add "Featured Item Slider Widget" to "Build" layer.
            var featuredItemSliderWidgetBuild = _widgetsService.CreateWidget(buildLayer.Id, "FeaturedItemSliderWidget", "Build Page", "1", "Featured");
            featuredItemSliderWidgetBuild.RenderTitle = false;
            featuredItemSliderWidgetBuild.Name = "build-featured-item-slider";
            featuredItemSliderWidgetBuild.As<FeaturedItemSliderWidgetPart>().GroupName = "Build Page";
            _contentManager.Publish(featuredItemSliderWidgetBuild.ContentItem);

            #endregion

            #region Create Feature Slider Widget - TRANSFORM

            // Create "Featured Item Group".
            var featuresGroupTransform = _contentManager.Create("FeaturedItemGroup");
            featuresGroupTransform.As<FeaturedItemGroupPart>().Name = "Transform Page";
            featuresGroupTransform.As<FeaturedItemGroupPart>().GroupWidth = 0;
            featuresGroupTransform.As<FeaturedItemGroupPart>().GroupHeight = 0;
            featuresGroupTransform.As<FeaturedItemGroupPart>().IncludeImages = true;
            featuresGroupTransform.As<FeaturedItemGroupPart>().ImageWidth = 0;
            featuresGroupTransform.As<FeaturedItemGroupPart>().ImageHeight = 0;
            featuresGroupTransform.As<FeaturedItemGroupPart>().BackgroundColor = "eb3b00";
            featuresGroupTransform.As<FeaturedItemGroupPart>().ForegroundColor = "ffffff";
            featuresGroupTransform.As<FeaturedItemGroupPart>().SlideSpeed = 700;
            featuresGroupTransform.As<FeaturedItemGroupPart>().SlidePause = 7000;
            featuresGroupTransform.As<FeaturedItemGroupPart>().ShowPager = true;
            featuresGroupTransform.As<FeaturedItemGroupPart>().TransitionEffect = "blindX";
            featuresGroupTransform.As<FeaturedItemGroupPart>().ImageStyle = ImageStyle.Background;
            _contentManager.Publish(featuresGroupTransform);

            // Add "Featured Item Slider Widget" to "Build" layer.
            var featuredItemSliderWidgetTransform = _widgetsService.CreateWidget(transformLayer.Id, "FeaturedItemSliderWidget", "Transform Page", "1", "Featured");
            featuredItemSliderWidgetTransform.RenderTitle = false;
            featuredItemSliderWidgetTransform.Name = "transform-featured-item-slider";
            featuredItemSliderWidgetTransform.As<FeaturedItemSliderWidgetPart>().GroupName = "Transform Page";
            _contentManager.Publish(featuredItemSliderWidgetTransform.ContentItem);

            #endregion

            #region Create Home Community Widget

            ContentDefinitionManager.AlterPartDefinition(
                typeof (HomeCommunityWidgetPart).Name, cfg => cfg.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("HomeCommunityWidget",
                cfg => cfg
                    .WithPart("WidgetPart")
                    .WithPart("HomeCommunityWidgetPart")
                    .WithPart("CommonPart")
                    .WithSetting("Stereotype", "Widget")
                );

            var homeCommunityWidget = _widgetsService.CreateWidget(homepageLayer.Id, "HomeCommunityWidget", "Home Community", "6", "AfterContent");
            homeCommunityWidget.RenderTitle = false;
            homeCommunityWidget.Name = "home-community";
            _contentManager.Publish(homeCommunityWidget.ContentItem);

            #endregion

            #region Create Training Widget

            ContentDefinitionManager.AlterPartDefinition(
                typeof (TrainingWidgetPart).Name, cfg => cfg.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("TrainingWidget",
                cfg => cfg
                    .WithPart("WidgetPart")
                    .WithPart("TrainingWidgetPart")
                    .WithPart("CommonPart")
                    .WithSetting("Stereotype", "Widget")
                );

            var trainingWidget = _widgetsService.CreateWidget(trainingLayer.Id, "TrainingWidget", "Training", "1", "AfterContent");
            trainingWidget.RenderTitle = false;
            trainingWidget.Name = "training";
            _contentManager.Publish(trainingWidget.ContentItem);

            #endregion

            #region Create Events Widget

            ContentDefinitionManager.AlterPartDefinition(
                typeof (EventsPart).Name, cfg => cfg.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("EventsWidget",
                cfg => cfg
                    .WithPart("WidgetPart")
                    .WithPart("EventsPart")
                    .WithPart("CommonPart")
                    .WithSetting("Stereotype", "Widget")
                );

            var eventsWidget = _widgetsService.CreateWidget(eventsLayer.Id, "EventsWidget", "Events", "1", "AfterContent");
            eventsWidget.RenderTitle = false;
            eventsWidget.Name = "events";
            _contentManager.Publish(eventsWidget.ContentItem);

            #endregion

            #region Create Step Part and related assets

            //Create table for StepInformationRecord
            SchemaBuilder.CreateTable(typeof (StepInformationRecord).Name, table => table
                .Column("Id", DbType.Int32, column => column.PrimaryKey().Identity())
                .Column("StepPartId", DbType.Int32)
                .Column("Title", DbType.String, column => column.Unlimited())
                .Column("Description", DbType.String, column => column.Unlimited())
                .Column("LeftImage", DbType.String)
                .Column("RightImage", DbType.String)
                .Column("CreateDate", DbType.DateTime)
                .Column("IsDeleted", DbType.Boolean)
                .Column("Position", DbType.Int32)
                );

            //Create table for SubstepInformationRecord
            SchemaBuilder.CreateTable(typeof (SubstepInformationRecord).Name, table => table
                .Column("Id", DbType.Int32, column => column.PrimaryKey().Identity())
                .Column(String.Format("{0}_Id", typeof (StepInformationRecord).Name), DbType.Int32)
                .Column("Title", DbType.String, column => column.Unlimited())
                .Column("Description", DbType.String, column => column.Unlimited())
                .Column("CreateDate", DbType.DateTime)
                .Column("IsDeleted", DbType.Boolean)
                .Column("Position", DbType.Int32)
                );

            // Creating table for StepPartRecord
            SchemaBuilder.CreateTable(typeof (StepPartRecord).Name, table => table
                .ContentPartRecord()
                );

            ContentDefinitionManager.AlterPartDefinition(typeof (StepPart).Name,
                builder => builder.Attachable());

            #endregion

            #region Attach Step Part to Step Page and update Autoroute Part

            ContentDefinitionManager.AlterTypeDefinition("StepPage",
                builder => builder
                    .WithPart(typeof (StepPart).Name)
                    .WithPart(typeof (AutoroutePart).Name, cfg => cfg
                        .WithSetting("AutorouteSettings.AllowCustomPattern", "true")
                        .WithSetting("AutorouteSettings.AutomaticAdjustmentOnEdit", "false")
                        .WithSetting("AutorouteSettings.PatternDefinitions", "[{Name:'Title', Pattern: '{Content.Slug}', Description: 'steppage-title'}]")
                        .WithSetting("AutorouteSettings.DefaultPatternIndex", "0"))
                );

            #endregion

            return 1;
        }

        public int UpdateFrom1() {
            #region Fix for importing / exporting custom content types - Attaching IdentityPart to types that do not have Autoroute part / friendly URL's

            ContentDefinitionManager.AlterTypeDefinition(
                "QuickLinks", cfg => cfg
                    .WithPart("IdentityPart"));

            ContentDefinitionManager.AlterTypeDefinition(
                "Event", cfg => cfg
                    .WithPart("IdentityPart"));

            //ContentDefinitionManager.AlterTypeDefinition(
            //    "Community", cfg => cfg
            //        .WithPart("IdentityPart"));

            ContentDefinitionManager.AlterTypeDefinition(
                "Training", cfg => cfg
                    .WithPart("IdentityPart"));

            #endregion

            return 2;
        }


        public int UpdateFrom2() {

            #region Updating hints for images

            ContentDefinitionManager.AlterPartDefinition(
                "QuickLinksPart", builder => builder
                    .WithField("SmallImage", cfg => cfg
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "The selected image will be used in the quicklinks in the header. Suggested image size is 24px X 24px (Width X Height)"))
                    .WithField("BigImage", cfg => cfg
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "The selected image will be used in the quicklinks in the home page. Suggested image size is 43px X 43px (Width X Height)"))
                );

            ContentDefinitionManager.AlterPartDefinition(
                "EventPart", builder => builder
                    .RemoveField("EventType")
                );

            ContentDefinitionManager.AlterPartDefinition(
                "TrainingPart", builder => builder
                    .WithField("Image", cfg => cfg
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "This will be the event thumbnail. Suggested image size is 100px X 100px (Width X Height)"))
                    .RemoveField("TrainingType")
                );

            ContentDefinitionManager.AlterPartDefinition(
                "CommunityPart", builder => builder
                    .WithField("Image", cfg => cfg
                        .OfType(typeof (MediaLibraryPickerField).Name)
                        .WithDisplayName("Image")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "The selected image will be used in conjuction with the text of the community resource description. Suggested max image size is 420px X 86px (Width X Height)"))

                );

            #endregion

            #region Converting Enumeration to Taxonomy fields

            var eventTypeTaxonomy = _contentManager.New("Taxonomy").As<TaxonomyPart>();
            eventTypeTaxonomy.Name = "Event Type";
            eventTypeTaxonomy.ContentItem.As<TitlePart>().Title = "Event Type";
            _taxonomyService.CreateTermContentType(eventTypeTaxonomy);
            _contentManager.Create(eventTypeTaxonomy, VersionOptions.Published);

            CreateTerm(eventTypeTaxonomy, "Upcoming Microsoft Events", 1);
            CreateTerm(eventTypeTaxonomy, "Upcoming Third Party Events", 2);
            CreateTerm(eventTypeTaxonomy, "Recent Events", 3);

            var trainingTypeTaxonomy = _contentManager.New("Taxonomy").As<TaxonomyPart>();
            trainingTypeTaxonomy.Name = "Training Type";
            trainingTypeTaxonomy.ContentItem.As<TitlePart>().Title = "Training Type";
            _taxonomyService.CreateTermContentType(trainingTypeTaxonomy);
            _contentManager.Create(trainingTypeTaxonomy, VersionOptions.Published);

            CreateTerm(trainingTypeTaxonomy, "Platform Overview", 1);
            CreateTerm(trainingTypeTaxonomy, "Dive Deeper", 2);
            CreateTerm(trainingTypeTaxonomy, "Advanced Topics", 3);

            ContentDefinitionManager.AlterPartDefinition(
                "EventPart", builder => builder
                    .WithField("EventType", cfg => cfg
                        .OfType("TaxonomyField")
                        .WithDisplayName("Event Type")
                        .WithSetting("TaxonomyFieldSettings.Taxonomy", eventTypeTaxonomy.Name))
                );

            ContentDefinitionManager.AlterPartDefinition(
                "TrainingPart", builder => builder
                    .WithField("TrainingType", cfg => cfg
                        .OfType("TaxonomyField")
                        .WithDisplayName("Training Type")
                        .WithSetting("TaxonomyFieldSettings.Taxonomy", trainingTypeTaxonomy.Name))
                );

            #endregion

            return 3;
        }

        public int UpdateFrom3() {
            #region Deleting and re-creating Event Type and Training Type taxonomy fields

            ContentDefinitionManager.AlterPartDefinition(
                "EventPart", builder => builder
                    .RemoveField("EventType")
                );

            ContentDefinitionManager.AlterPartDefinition(
                "TrainingPart", builder => builder
                    .RemoveField("TrainingType")
                );

            var eventType = _taxonomyService.GetTaxonomyByName("Event Type");
            if (eventType != null) {
                _taxonomyService.DeleteTaxonomy(eventType.As<TaxonomyPart>());
            }
            var eventTypeTaxonomy = _contentManager.New("Taxonomy").As<TaxonomyPart>();
            eventTypeTaxonomy.Name = "Event Type";
            eventTypeTaxonomy.ContentItem.As<TitlePart>().Title = "Event Type";
            _taxonomyService.CreateTermContentType(eventTypeTaxonomy);
            _contentManager.Create(eventTypeTaxonomy, VersionOptions.Published);

            CreateTerm(eventTypeTaxonomy, "Upcoming Microsoft Events", 1);
            CreateTerm(eventTypeTaxonomy, "Upcoming Third Party Events", 2);
            CreateTerm(eventTypeTaxonomy, "Recent Events", 3);

            var trainingType = _taxonomyService.GetTaxonomyByName("Training Type");
            if (trainingType != null) {
                _taxonomyService.DeleteTaxonomy(trainingType.As<TaxonomyPart>());
            }
            var trainingTypeTaxonomy = _contentManager.New("Taxonomy").As<TaxonomyPart>();
            trainingTypeTaxonomy.Name = "Training Type";
            trainingTypeTaxonomy.ContentItem.As<TitlePart>().Title = "Training Type";
            _taxonomyService.CreateTermContentType(trainingTypeTaxonomy);
            _contentManager.Create(trainingTypeTaxonomy, VersionOptions.Published);

            CreateTerm(trainingTypeTaxonomy, "Platform Overview", 1);
            CreateTerm(trainingTypeTaxonomy, "Dive Deeper", 2);
            CreateTerm(trainingTypeTaxonomy, "Advanced Topics", 3);

            ContentDefinitionManager.AlterPartDefinition(
                "EventPart", builder => builder
                    .WithField("ExternalLink", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("External Link")
                        .WithSetting("TextFieldSettings.Flavor", "large"))

                    .WithField("EventType", cfg => cfg
                        .OfType("TaxonomyField")
                        .WithDisplayName("Event Type")
                        .WithSetting("TaxonomyFieldSettings.Taxonomy", eventTypeTaxonomy.Name))
                );

            ContentDefinitionManager.AlterPartDefinition(
                "TrainingPart", builder => builder
                    .WithField("ExternalLink", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("External Link")
                        .WithSetting("TextFieldSettings.Flavor", "large"))

                    .WithField("TrainingType", cfg => cfg
                        .OfType("TaxonomyField")
                        .WithDisplayName("Training Type")
                        .WithSetting("TaxonomyFieldSettings.Taxonomy", trainingTypeTaxonomy.Name))
                );

            #endregion

            #region Create new pages - Videos, Code Samples, Podcasts and Resources

            var navGlobal = _menuService.GetMenu("Global Navigation");

            //Create Videos Page
            var pageVideos = _contentManager.Create("Page");
            pageVideos.As<TitlePart>().Title = "Videos";
            pageVideos.As<BodyPart>().Text = String.Empty;

            var pageVideosArp = pageVideos.As<AutoroutePart>();
            pageVideosArp.DisplayAlias = "videos";
            _autorouteService.GenerateAlias(pageVideosArp);
            _autorouteService.PublishAlias(pageVideosArp);

            //Create Code Samples Page
            var pageCodeSamples = _contentManager.Create("Page");
            pageCodeSamples.As<TitlePart>().Title = "Code Samples";
            pageCodeSamples.As<BodyPart>().Text = String.Empty;

            var pageCodeSamplesArp = pageCodeSamples.As<AutoroutePart>();
            pageCodeSamplesArp.DisplayAlias = "code-samples";
            _autorouteService.GenerateAlias(pageCodeSamplesArp);
            _autorouteService.PublishAlias(pageCodeSamplesArp);

            //Create Podcasts Page
            var pagePodcasts = _contentManager.Create("Page");
            pagePodcasts.As<TitlePart>().Title = "Podcasts";
            pagePodcasts.As<BodyPart>().Text = String.Empty;

            var pagePodcastsArp = pagePodcasts.As<AutoroutePart>();
            pagePodcastsArp.DisplayAlias = "podcasts";
            _autorouteService.GenerateAlias(pagePodcastsArp);
            _autorouteService.PublishAlias(pagePodcastsArp);

            //Create Resources Page
            var pageResources = _contentManager.Create("Page");
            pageResources.As<TitlePart>().Title = "Resources";
            pageResources.As<BodyPart>().Text = String.Empty;

            var pageResourcesArp = pageResources.As<AutoroutePart>();
            pageResourcesArp.DisplayAlias = "resources";
            _autorouteService.GenerateAlias(pageResourcesArp);
            _autorouteService.PublishAlias(pageResourcesArp);

            var pageResourcesMenu = pageResources.As<MenuPart>();
            pageResourcesMenu.Menu = navGlobal;
            pageResourcesMenu.MenuText = pageResources.As<TitlePart>().Title;
            pageResourcesMenu.MenuPosition = "6";

            _contentManager.Publish(pageVideos);
            _contentManager.Publish(pageCodeSamples);
            _contentManager.Publish(pagePodcasts);
            _contentManager.Publish(pageResources);

            #endregion

            #region Creating 6 new Taxonomies for Code Samples page

            //creating code sample type Taxonomy
            var codeSampleType = _taxonomyService.GetTaxonomyByName("Code Sample Type");
            if (codeSampleType != null) {
                _taxonomyService.DeleteTaxonomy(codeSampleType.As<TaxonomyPart>());
            }

            var codeSampleTypeTaxonomy = _contentManager.New("Taxonomy").As<TaxonomyPart>();
            codeSampleTypeTaxonomy.Name = "Code Sample Type";
            codeSampleTypeTaxonomy.ContentItem.As<TitlePart>().Title = "Code Sample Type";
            _taxonomyService.CreateTermContentType(codeSampleTypeTaxonomy);
            _contentManager.Create(codeSampleTypeTaxonomy, VersionOptions.Published);

            CreateTerm(codeSampleTypeTaxonomy, "App for SharePoint", 1);
            CreateTerm(codeSampleTypeTaxonomy, "App for Office", 2);
            CreateTerm(codeSampleTypeTaxonomy, "Office 365 API", 3);

            //creating code sample platform Taxonomy
            var codeSamplePlatform = _taxonomyService.GetTaxonomyByName("Code Sample Platform");
            if (codeSamplePlatform != null) {
                _taxonomyService.DeleteTaxonomy(codeSamplePlatform.As<TaxonomyPart>());
            }

            var codeSamplePlatformTaxonomy = _contentManager.New("Taxonomy").As<TaxonomyPart>();
            codeSamplePlatformTaxonomy.Name = "Code Sample Platform";
            codeSamplePlatformTaxonomy.ContentItem.As<TitlePart>().Title = "Code Sample Platform";
            _taxonomyService.CreateTermContentType(codeSamplePlatformTaxonomy);
            _contentManager.Create(codeSamplePlatformTaxonomy, VersionOptions.Published);

            CreateTerm(codeSamplePlatformTaxonomy, "Azure", 1);
            CreateTerm(codeSamplePlatformTaxonomy, "Windows 8", 2);
            CreateTerm(codeSamplePlatformTaxonomy, "Windows Phone 8.1", 3);
            CreateTerm(codeSamplePlatformTaxonomy, "Android", 4);
            CreateTerm(codeSamplePlatformTaxonomy, "iOS", 5);
            CreateTerm(codeSamplePlatformTaxonomy, "Cordova", 6);
            CreateTerm(codeSamplePlatformTaxonomy, "Xamarin", 7);

            //creating code sample language Taxonomy
            var codeSampleLanguage = _taxonomyService.GetTaxonomyByName("Code Sample Language");
            if (codeSampleLanguage != null) {
                _taxonomyService.DeleteTaxonomy(codeSampleLanguage.As<TaxonomyPart>());
            }

            var codeSampleLanguageTaxonomy = _contentManager.New("Taxonomy").As<TaxonomyPart>();
            codeSampleLanguageTaxonomy.Name = "Code Sample Language";
            codeSampleLanguageTaxonomy.ContentItem.As<TitlePart>().Title = "Code Sample Language";
            _taxonomyService.CreateTermContentType(codeSampleLanguageTaxonomy);
            _contentManager.Create(codeSampleLanguageTaxonomy, VersionOptions.Published);

            CreateTerm(codeSampleLanguageTaxonomy, "ASP.NET MVC", 1);
            CreateTerm(codeSampleLanguageTaxonomy, "ASP>NET Forms", 2);
            CreateTerm(codeSampleLanguageTaxonomy, "Silverlight", 3);
            CreateTerm(codeSampleLanguageTaxonomy, "HTML / JS", 4);

            //creating code sample Services Taxonomy
            var codeSampleServices = _taxonomyService.GetTaxonomyByName("Code Sample Service");
            if (codeSampleServices != null) {
                _taxonomyService.DeleteTaxonomy(codeSampleServices.As<TaxonomyPart>());
            }

            var codeSampleServicesTaxonomy = _contentManager.New("Taxonomy").As<TaxonomyPart>();
            codeSampleServicesTaxonomy.Name = "Code Sample Service";
            codeSampleServicesTaxonomy.ContentItem.As<TitlePart>().Title = "Code Sample Service";
            _taxonomyService.CreateTermContentType(codeSampleServicesTaxonomy);
            _contentManager.Create(codeSampleServicesTaxonomy, VersionOptions.Published);

            CreateTerm(codeSampleServicesTaxonomy, "Taxonomy", 1);
            CreateTerm(codeSampleServicesTaxonomy, "Search", 2);
            CreateTerm(codeSampleServicesTaxonomy, "User Profiles", 3);
            CreateTerm(codeSampleServicesTaxonomy, "Business Connectivity", 4);
            CreateTerm(codeSampleServicesTaxonomy, "Services", 5);

            //creating code sample Source Taxonomy
            var codeSampleSource = _taxonomyService.GetTaxonomyByName("Code Sample Source");
            if (codeSampleSource != null) {
                _taxonomyService.DeleteTaxonomy(codeSampleSource.As<TaxonomyPart>());
            }

            var codeSampleSourceTaxonomy = _contentManager.New("Taxonomy").As<TaxonomyPart>();
            codeSampleSourceTaxonomy.Name = "Code Sample Source";
            codeSampleSourceTaxonomy.ContentItem.As<TitlePart>().Title = "Code Sample Source";
            _taxonomyService.CreateTermContentType(codeSampleSourceTaxonomy);
            _contentManager.Create(codeSampleSourceTaxonomy, VersionOptions.Published);

            CreateTerm(codeSampleSourceTaxonomy, "GitHub", 1);
            CreateTerm(codeSampleSourceTaxonomy, "MSDN Code", 2);
            CreateTerm(codeSampleSourceTaxonomy, "Gallery", 3);
            CreateTerm(codeSampleSourceTaxonomy, "CodePlex", 4);
            CreateTerm(codeSampleSourceTaxonomy, "Other", 5);

            //creating code sample Products Taxonomy
            var codeSampleProducts = _taxonomyService.GetTaxonomyByName("Code Sample Product");
            if (codeSampleProducts != null) {
                _taxonomyService.DeleteTaxonomy(codeSampleProducts.As<TaxonomyPart>());
            }

            var codeSampleProductsTaxonomy = _contentManager.New("Taxonomy").As<TaxonomyPart>();
            codeSampleProductsTaxonomy.Name = "Code Sample Product";
            codeSampleProductsTaxonomy.ContentItem.As<TitlePart>().Title = "Code Sample Product";
            _taxonomyService.CreateTermContentType(codeSampleProductsTaxonomy);
            _contentManager.Create(codeSampleProductsTaxonomy, VersionOptions.Published);

            CreateTerm(codeSampleProductsTaxonomy, "SharePoint", 1);
            CreateTerm(codeSampleProductsTaxonomy, "Exchange", 2);
            CreateTerm(codeSampleProductsTaxonomy, "Lync", 3);
            CreateTerm(codeSampleProductsTaxonomy, "Skype", 4);
            CreateTerm(codeSampleProductsTaxonomy, "Word", 5);
            CreateTerm(codeSampleProductsTaxonomy, "PowerPoint", 6);
            CreateTerm(codeSampleProductsTaxonomy, "Excel", 7);
            CreateTerm(codeSampleProductsTaxonomy, "Outlook", 8);
            CreateTerm(codeSampleProductsTaxonomy, "OneNote", 9);
            CreateTerm(codeSampleProductsTaxonomy, "Yammer", 10);
            CreateTerm(codeSampleProductsTaxonomy, "Oslo", 11);


            #endregion

            #region Create Code Sample Content Type

            //creating code sample content type
            ContentDefinitionManager.AlterPartDefinition(
                "CodeSamplePart", builder => builder
                    .WithDescription("A single code sample")

                    .WithField("SubText", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Sub Text")
                        .WithSetting("TextFieldSettings.Flavor", "textarea")
                        .WithSetting("TextFieldSettings.Hint", "Enter description of the code sample."))

                    .WithField("StartDate", cfg => cfg
                        .OfType("DateTimeField")
                        .WithDisplayName("Start Date")
                        .WithSetting("DateTimeFieldSettings.Display", "DateOnly"))

                    .WithField("EndDate", cfg => cfg
                        .OfType("DateTimeField")
                        .WithDisplayName("End Date")
                        .WithSetting("DateTimeFieldSettings.Display", "DateOnly"))

                    .WithField("ExternalLink", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("External Link")
                        .WithSetting("TextFieldSettings.Flavor", "large")
                        .WithSetting("TextFieldSettings.Hint", "Enter the link to the external resource. Ex. https://www.microsoft.com"))

                    .WithField("Location", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Location")
                        .WithSetting("TextFieldSettings.Hint", "Enter the location or other info about the Code Sample."))

                    .WithField("Image", cfg => cfg
                        .OfType(typeof (MediaLibraryPickerField).Name)
                        .WithDisplayName("Image")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "This will be the code sample thumbnail."))

                    .WithField("CodeSampleType", cfg => cfg
                        .OfType("TaxonomyField")
                        .WithDisplayName("Code Sample Type")
                        .WithSetting("TaxonomyFieldSettings.Taxonomy", codeSampleTypeTaxonomy.Name))

                    .WithField("CodeSamplePlatform", cfg => cfg
                        .OfType("TaxonomyField")
                        .WithDisplayName("Code Sample Platform")
                        .WithSetting("TaxonomyFieldSettings.Taxonomy", codeSamplePlatformTaxonomy.Name))

                    .WithField("CodeSampleLanguage", cfg => cfg
                        .OfType("TaxonomyField")
                        .WithDisplayName("Code Sample Language")
                        .WithSetting("TaxonomyFieldSettings.Taxonomy", codeSampleLanguageTaxonomy.Name))

                    .WithField("CodeSampleServices", cfg => cfg
                        .OfType("TaxonomyField")
                        .WithDisplayName("Code Sample Services")
                        .WithSetting("TaxonomyFieldSettings.Taxonomy", codeSampleServicesTaxonomy.Name))

                    .WithField("CodeSampleSource", cfg => cfg
                        .OfType("TaxonomyField")
                        .WithDisplayName("Code Sample Source")
                        .WithSetting("TaxonomyFieldSettings.Taxonomy", codeSampleSourceTaxonomy.Name))

                    .WithField("CodeSampleProducts", cfg => cfg
                        .OfType("TaxonomyField")
                        .WithDisplayName("Code Sample Products")
                        .WithSetting("TaxonomyFieldSettings.Taxonomy", codeSampleProductsTaxonomy.Name))

                );

            ContentDefinitionManager.AlterTypeDefinition(
                "CodeSample", cfg => cfg
                    .DisplayedAs("Code Sample")
                    .WithPart("TitlePart")
                    .WithPart("CodeSamplePart")
                    .WithPart("CommonPart")
                    .WithPart("IdentityPart")
                    .Draftable()
                    .Creatable()
                );

            #endregion

            #region Create Podcast Content Type

            var podcastType = _taxonomyService.GetTaxonomyByName("Podcast Type");
            if (podcastType != null) {
                _taxonomyService.DeleteTaxonomy(podcastType.As<TaxonomyPart>());
            }

            //creating video type Taxonomy
            var podcastTypeTaxonomy = _contentManager.New("Taxonomy").As<TaxonomyPart>();
            podcastTypeTaxonomy.Name = "Podcast Type";
            podcastTypeTaxonomy.ContentItem.As<TitlePart>().Title = "Podcast Type";
            _taxonomyService.CreateTermContentType(podcastTypeTaxonomy);
            _contentManager.Create(podcastTypeTaxonomy, VersionOptions.Published);

            CreateTerm(podcastTypeTaxonomy, "Podcast Type 1", 1);
            CreateTerm(podcastTypeTaxonomy, "Podcast Type 2", 2);
            CreateTerm(podcastTypeTaxonomy, "Podcast Type 3", 3);

            //creating video content type
            ContentDefinitionManager.AlterPartDefinition(
                "PodcastPart", builder => builder
                    .WithDescription("A single podcast")

                    .WithField("SubText", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Sub Text")
                        .WithSetting("TextFieldSettings.Flavor", "textarea")
                        .WithSetting("TextFieldSettings.Hint", "Enter description of the podcast."))

                    .WithField("StartDate", cfg => cfg
                        .OfType("DateTimeField")
                        .WithDisplayName("Start Date")
                        .WithSetting("DateTimeFieldSettings.Display", "DateOnly"))

                    .WithField("EndDate", cfg => cfg
                        .OfType("DateTimeField")
                        .WithDisplayName("End Date")
                        .WithSetting("DateTimeFieldSettings.Display", "DateOnly"))

                    .WithField("ExternalLink", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("External Link")
                        .WithSetting("TextFieldSettings.Flavor", "large")
                        .WithSetting("TextFieldSettings.Hint", "Enter the link to the external resource. Ex. https://www.microsoft.com"))

                    .WithField("Location", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Location")
                        .WithSetting("TextFieldSettings.Hint", "Enter the location of the podcast."))

                    .WithField("Image", cfg => cfg
                        .OfType(typeof (MediaLibraryPickerField).Name)
                        .WithDisplayName("Image")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "This will be the podcast thumbnail."))

                    .WithField("PodcastType", cfg => cfg
                        .OfType("TaxonomyField")
                        .WithDisplayName("Podcast Type")
                        .WithSetting("TaxonomyFieldSettings.Taxonomy", podcastTypeTaxonomy.Name))
                );

            ContentDefinitionManager.AlterTypeDefinition(
                "Podcast", cfg => cfg
                    .DisplayedAs("Podcast")
                    .WithPart("TitlePart")
                    .WithPart("PodcastPart")
                    .WithPart("CommonPart")
                    .WithPart("IdentityPart")
                    .Draftable()
                    .Creatable()
                );

            #endregion

            #region Create Video Content Type

            var videoType = _taxonomyService.GetTaxonomyByName("Video Type");
            if (videoType != null) {
                _taxonomyService.DeleteTaxonomy(videoType.As<TaxonomyPart>());
            }

            //creating video type Taxonomy
            var videoTypeTaxonomy = _contentManager.New("Taxonomy").As<TaxonomyPart>();
            videoTypeTaxonomy.Name = "Video Type";
            videoTypeTaxonomy.ContentItem.As<TitlePart>().Title = "Video Type";
            _taxonomyService.CreateTermContentType(videoTypeTaxonomy);
            _contentManager.Create(videoTypeTaxonomy, VersionOptions.Published);

            CreateTerm(videoTypeTaxonomy, "Success Stories", 1);
            CreateTerm(videoTypeTaxonomy, "Garage Series", 2);
            CreateTerm(videoTypeTaxonomy, "Microsoft Virtual Academy", 3);

            //creating video content type
            ContentDefinitionManager.AlterPartDefinition(
                "VideoItemPart", builder => builder
                    .WithDescription("A single video")

                    .WithField("SubText", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Sub Text")
                        .WithSetting("TextFieldSettings.Flavor", "textarea")
                        .WithSetting("TextFieldSettings.Hint", "Enter description of the video."))

                    .WithField("StartDate", cfg => cfg
                        .OfType("DateTimeField")
                        .WithDisplayName("Start Date")
                        .WithSetting("DateTimeFieldSettings.Display", "DateOnly"))

                    .WithField("EndDate", cfg => cfg
                        .OfType("DateTimeField")
                        .WithDisplayName("End Date")
                        .WithSetting("DateTimeFieldSettings.Display", "DateOnly"))

                    .WithField("EmbedCode", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Embed Code")
                        .WithSetting("TextFieldSettings.Flavor", "textarea")
                        .WithSetting("TextFieldSettings.Hint", "Enter the Video Embed Code."))

                    .WithField("Location", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Location")
                        .WithSetting("TextFieldSettings.Hint", "Enter location or other info about the Video."))

                    .WithField("Image", cfg => cfg
                        .OfType(typeof (MediaLibraryPickerField).Name)
                        .WithDisplayName("Image")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "This will be the video thumbnail."))

                    .WithField("VideoType", cfg => cfg
                        .OfType("TaxonomyField")
                        .WithDisplayName("Video Type")
                        .WithSetting("TaxonomyFieldSettings.Taxonomy", videoTypeTaxonomy.Name))
                );

            ContentDefinitionManager.AlterTypeDefinition(
                "VideoItem", cfg => cfg
                    .DisplayedAs("Video")
                    .WithPart("TitlePart")
                    .WithPart("VideoItemPart")
                    .WithPart("CommonPart")
                    .WithPart("IdentityPart")
                    .Draftable()
                    .Creatable()
                );

            #endregion

            #region Create Resource Content Type

            //creating video content type
            ContentDefinitionManager.AlterPartDefinition(
                "ResourcePart", builder => builder
                    .WithDescription("A single resource")

                    .WithField("SubText", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Sub Text")
                        .WithSetting("TextFieldSettings.Flavor", "textarea")
                        .WithSetting("TextFieldSettings.Hint", "Enter description of the resource."))

                    .WithField("LearnMoreText", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Learn More Text")
                        .WithSetting("TextFieldSettings.Hint", "Enter the learn more text"))

                    .WithField("LearnMoreUrl", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Learn More URL")
                        .WithSetting("TextFieldSettings.Hint", "Enter the learn more URL"))

                    .WithField("Image", cfg => cfg
                        .OfType(typeof (MediaLibraryPickerField).Name)
                        .WithDisplayName("Image")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "This will be the resource thumbnail."))

                );

            ContentDefinitionManager.AlterTypeDefinition(
                "Resource", cfg => cfg
                    .DisplayedAs("Resource")
                    .WithPart("TitlePart")
                    .WithPart("ResourcePart")
                    .WithPart("CommonPart")
                    .WithPart("IdentityPart")
                    .Draftable()
                    .Creatable()
                );

            #endregion

            #region Create layers for each page

            var videosLayer = _widgetsService.CreateLayer("Video", "The widgets in this layer are displayed on the videos page.", "url '~/videos'");
            var codeSamplesLayer = _widgetsService.CreateLayer("Code Samples", "The widgets in this layer are displayed on the code samples page.", "url '~/code-samples'");
            var podcastsLayer = _widgetsService.CreateLayer("Podcast", "The widgets in this layer are displayed on the podcasts page.", "url '~/podcasts'");
            var resourcesLayer = _widgetsService.CreateLayer("Resources", "The widgets in this layer are displayed on the resources page.", "url '~/resources'");

            #endregion

            #region Create Code Samples Widget

            ContentDefinitionManager.AlterPartDefinition(
                typeof (CodeSamplesWidgetPart).Name, cfg => cfg.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("CodeSamplesWidget",
                cfg => cfg
                    .WithPart("WidgetPart")
                    .WithPart("CodeSamplesWidgetPart")
                    .WithPart("CommonPart")
                    .WithSetting("Stereotype", "Widget")
                );

            var codeSamplesWidget = _widgetsService.CreateWidget(codeSamplesLayer.Id, "codeSamplesWidget", "Code Samples", "1", "AfterContent");
            codeSamplesWidget.RenderTitle = false;
            codeSamplesWidget.Name = "code-samples";
            _contentManager.Publish(codeSamplesWidget.ContentItem);

            #endregion

            #region Create Podcasts Widget

            ContentDefinitionManager.AlterPartDefinition(
                typeof (PodcastsWidgetPart).Name, cfg => cfg.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("PodcastsWidget",
                cfg => cfg
                    .WithPart("WidgetPart")
                    .WithPart("PodcastsWidgetPart")
                    .WithPart("CommonPart")
                    .WithSetting("Stereotype", "Widget")
                );

            var podcastsWidget = _widgetsService.CreateWidget(podcastsLayer.Id, "PodcastsWidget", "Podcasts", "1", "AfterContent");
            podcastsWidget.RenderTitle = false;
            podcastsWidget.Name = "podcasts";
            _contentManager.Publish(podcastsWidget.ContentItem);

            #endregion

            #region Create Videos Widget

            ContentDefinitionManager.AlterPartDefinition(
                typeof (VideosWidgetPart).Name, cfg => cfg.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("VideosWidget",
                cfg => cfg
                    .WithPart("WidgetPart")
                    .WithPart("VideosWidgetPart")
                    .WithPart("CommonPart")
                    .WithSetting("Stereotype", "Widget")
                );

            var videosWidget = _widgetsService.CreateWidget(videosLayer.Id, "VideosWidget", "Videos", "1", "AfterContent");
            videosWidget.RenderTitle = false;
            videosWidget.Name = "videos";
            _contentManager.Publish(videosWidget.ContentItem);

            #endregion

            #region Create Resources Widget

            ContentDefinitionManager.AlterPartDefinition(
                typeof (ResourcesWidgetPart).Name, cfg => cfg.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("ResourcesWidget",
                cfg => cfg
                    .WithPart("WidgetPart")
                    .WithPart("ResourcesWidgetPart")
                    .WithPart("CommonPart")
                    .WithSetting("Stereotype", "Widget")
                );

            var resourcesWidget = _widgetsService.CreateWidget(resourcesLayer.Id, "ResourcesWidget", "Resources", "1", "AfterContent");
            resourcesWidget.RenderTitle = false;
            resourcesWidget.Name = "resources";
            _contentManager.Publish(resourcesWidget.ContentItem);

            #endregion

            #region Create Feature Slider Widget - Resources

            // Create "Featured Item Group".
            var featuresGroupResources = _contentManager.Create("FeaturedItemGroup");
            featuresGroupResources.As<FeaturedItemGroupPart>().Name = "Resources Page";
            featuresGroupResources.As<FeaturedItemGroupPart>().GroupWidth = 0;
            featuresGroupResources.As<FeaturedItemGroupPart>().GroupHeight = 0;
            featuresGroupResources.As<FeaturedItemGroupPart>().IncludeImages = true;
            featuresGroupResources.As<FeaturedItemGroupPart>().ImageWidth = 0;
            featuresGroupResources.As<FeaturedItemGroupPart>().ImageHeight = 0;
            featuresGroupResources.As<FeaturedItemGroupPart>().BackgroundColor = "eb3b00";
            featuresGroupResources.As<FeaturedItemGroupPart>().ForegroundColor = "ffffff";
            featuresGroupResources.As<FeaturedItemGroupPart>().SlideSpeed = 700;
            featuresGroupResources.As<FeaturedItemGroupPart>().SlidePause = 7000;
            featuresGroupResources.As<FeaturedItemGroupPart>().ShowPager = true;
            featuresGroupResources.As<FeaturedItemGroupPart>().TransitionEffect = "blindX";
            featuresGroupResources.As<FeaturedItemGroupPart>().ImageStyle = ImageStyle.Background;
            _contentManager.Publish(featuresGroupResources);

            // Add "Featured Item Slider Widget" to "Opportunity" layer.
            var featuredItemSliderWidgetResources = _widgetsService.CreateWidget(resourcesLayer.Id, "FeaturedItemSliderWidget", "Resources Page", "1", "Featured");
            featuredItemSliderWidgetResources.RenderTitle = false;
            featuredItemSliderWidgetResources.Name = "resources-featured-item-slider";
            featuredItemSliderWidgetResources.As<FeaturedItemSliderWidgetPart>().GroupName = "Resources Page";
            _contentManager.Publish(featuredItemSliderWidgetResources.ContentItem);

            #endregion

            return 4;

        }

        public int UpdateFrom4() {

            #region Create Solution Taxonomy terms

            //creating solution type Taxonomy
            var solutionType = _taxonomyService.GetTaxonomyByName("Solution Type");
            if (solutionType != null)
            {
                _taxonomyService.DeleteTaxonomy(solutionType.As<TaxonomyPart>());
            }

            var solutionTypeTaxonomy = _contentManager.New("Taxonomy").As<TaxonomyPart>();
            solutionTypeTaxonomy.Name = "Solution Type";
            solutionTypeTaxonomy.ContentItem.As<TitlePart>().Title = "Solution Type";
            _taxonomyService.CreateTermContentType(solutionTypeTaxonomy);
            _contentManager.Create(solutionTypeTaxonomy, VersionOptions.Published);

            CreateTerm(solutionTypeTaxonomy, "Mobile Apps", 1);
            CreateTerm(solutionTypeTaxonomy, "Works With Apps", 2);
            CreateTerm(solutionTypeTaxonomy, "Office Apps", 3);

            
            #endregion

            #region Create Solution Content Type

            ContentDefinitionManager.AlterPartDefinition(
                "SolutionPart", builder => builder
                    .WithDescription("Solution")

                    .WithField("Content", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("Content")
                        .WithSetting("TextFieldSettings.Flavor", "textarea")
                        .WithSetting("TextFieldSettings.Hint", "Enter content."))

                    .WithField("Icon", cfg => cfg
                        .OfType(typeof(MediaLibraryPickerField).Name)
                        .WithDisplayName("Icon")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "Select an icon for the solution."))

                    .WithField("SolutionType", cfg => cfg
                        .OfType(typeof(TaxonomyField).Name)
                        .WithDisplayName("Solution Type")
                        .WithSetting("TaxonomyFieldSettings.Hint", "Enter a solution type.")
                    .WithSetting("TaxonomyFieldSettings.Taxonomy", solutionTypeTaxonomy.Name))

                    .WithField("Ordering", cfg => cfg
                        .OfType("NumericField")
                        .WithDisplayName("Ordering")
                        .WithSetting("NumericFieldSettings.Hint", "Select a numeric position for the item in the ordering."))

                    .WithField("ExternalLink", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("External Link")
                        .WithSetting("TextFieldSettings.Hint", "Enter the link to a resource. Ex. https://www.microsoft.com"))
                );

            ContentDefinitionManager.AlterTypeDefinition(
                "Solution", cfg => cfg
                    .DisplayedAs("Solution")
                    .WithPart("TitlePart")
                    .WithPart("IdentityPart")
                    .WithPart("SolutionPart")
                    .WithPart("CommonPart")
                    .Draftable()
                    .Creatable()
                );

            #endregion

            #region Create Solutions Widget

            ContentDefinitionManager.AlterPartDefinition(
                typeof(SolutionWidgetPart).Name, builder => builder
                     .WithField("Description", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("Description")
                        .WithSetting("TextFieldSettings.Flavor", "html")
                        .WithSetting("TextFieldSettings.Hint", "Enter the link to a resource. Ex. https://www.microsoft.com"))
                    .Attachable());

            ContentDefinitionManager.AlterTypeDefinition("SolutionWidget",
                cfg => cfg
                    .WithPart("WidgetPart")
                    .WithPart("CommonPart")
                    .WithPart("SolutionWidgetPart")
                    .WithSetting("Stereotype", "Widget")
                );
            var solutionLayer = _widgetsService.CreateLayer("Solution", "The widgets in this layer are displayed on the Solutions Showcase page.", "url '~/solutions'");
            var solutionWidget = _widgetsService.CreateWidget(solutionLayer.Id, "SolutionWidget", "Solution", "1", "AfterContent");
            
            solutionWidget.RenderTitle = false;
            solutionWidget.Name = "solution";
            _contentManager.Publish(solutionWidget.ContentItem);

            #endregion

            #region Create Patterns and Practices Taxonomy terms

            //creating patterns and practices type Taxonomy
            var patternsAndPracticesType = _taxonomyService.GetTaxonomyByName("Patterns and Practices Type");
            if (patternsAndPracticesType != null)
            {
                _taxonomyService.DeleteTaxonomy(patternsAndPracticesType.As<TaxonomyPart>());
            }

            var patternsAndPracticesTypeTaxonomy = _contentManager.New("Taxonomy").As<TaxonomyPart>();
            patternsAndPracticesTypeTaxonomy.Name = "Patterns and Practices Type";
            patternsAndPracticesTypeTaxonomy.ContentItem.As<TitlePart>().Title = "Patterns and Practices Type";
            _taxonomyService.CreateTermContentType(patternsAndPracticesTypeTaxonomy);
            _contentManager.Create(patternsAndPracticesTypeTaxonomy, VersionOptions.Published);

            CreateTerm(patternsAndPracticesTypeTaxonomy, "Guidance", 1);
            CreateTerm(patternsAndPracticesTypeTaxonomy, "Branding", 2);
            CreateTerm(patternsAndPracticesTypeTaxonomy, "Provisioning", 3);
            CreateTerm(patternsAndPracticesTypeTaxonomy, "Javascript Injection", 4);
            CreateTerm(patternsAndPracticesTypeTaxonomy, "Remote Connections", 5);

            #endregion
            
            #region Create Patterns and Practices Content Type

            ContentDefinitionManager.AlterPartDefinition(
                "PatternsAndPracticesPart", builder => builder
                    .WithDescription("A single patterns and practices item.")

                    .WithField("SubText", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("SubText")
                        .WithSetting("TextFieldSettings.Flavor", "textarea")
                        .WithSetting("TextFieldSettings.Hint", "Enter subtext."))

                   .WithField("Icon", cfg => cfg
                        .OfType(typeof(MediaLibraryPickerField).Name)
                        .WithDisplayName("Icon")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "Select an icon for this item."))

                    .WithField("PatternsAndPracticesType", cfg => cfg
                        .OfType(typeof(TaxonomyField).Name)
                        .WithDisplayName("Patterns and Practices Type")
                        .WithSetting("TaxonomyFieldSettings.Hint", "Enter a patterns and practices type.")
                    .WithSetting("TaxonomyFieldSettings.Taxonomy", patternsAndPracticesTypeTaxonomy.Name))

                    .WithField("ExternalLink", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("External Link")
                        .WithSetting("TextFieldSettings.Flavor", "large")
                        .WithSetting("TextFieldSettings.Hint", "Enter the link to the external resource. Ex. https://www.microsoft.com"))

                    .WithField("Ordering", cfg => cfg
                        .OfType("NumericField")
                        .WithDisplayName("Ordering")
                        .WithSetting("NumericFieldSettings.Hint", "Select a numeric position for the item in the ordering."))
                );

            ContentDefinitionManager.AlterTypeDefinition(
                "PatternsAndPractices", cfg => cfg
                    .DisplayedAs("Patterns and Practices")
                    .WithPart("TitlePart")
                    .WithPart("IdentityPart")
                    .WithPart("PatternsAndPracticesPart")
                    .WithPart("CommonPart")
                    .Draftable()
                    .Creatable()
                );

            #endregion

            #region Create Patterns And Practices Widget

            ContentDefinitionManager.AlterPartDefinition(
                typeof(PatternsAndPracticesWidgetPart).Name, cfg => cfg.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("PatternsAndPracticesWidget",
                cfg => cfg
                    .WithPart("WidgetPart")
                    .WithPart("CommonPart")
                    .WithPart("PatternsAndPracticesWidgetPart")
                    .WithSetting("Stereotype", "Widget")
                );
            var patternsAndPracticesLayer = _widgetsService.CreateLayer("Patterns and Practices", "The widgets in this layer are displayed on the Patterns and Practices page.", "url '~/patterns-and-practices'");
            var patternsAndPracticesWidget = _widgetsService.CreateWidget(patternsAndPracticesLayer.Id, "PatternsAndPracticesWidget", "Patterns And Pratices", "1", "AfterContent");

            patternsAndPracticesWidget.RenderTitle = false;
            patternsAndPracticesWidget.Name = "patternsAndPractices";
            _contentManager.Publish(patternsAndPracticesWidget.ContentItem);
            #endregion

            #region Create Getting Started Content Type

            ContentDefinitionManager.AlterPartDefinition(
                "GettingStartedPart", builder => builder
                    .WithDescription("Getting Started")

                    .WithField("Hashtag", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("Hashtag")
                        .WithSetting("TextFieldSettings.Hint", "Custom hashtag for direct link in lower case. For ex, extend."))

                    .WithField("Icons", cfg => cfg
                        .OfType(typeof(MediaLibraryPickerField).Name)
                        .WithDisplayName("Icons")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Multiple", "True")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "Select icons for the tabs in the first block."))

                    .WithField("Intro", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("Intro")
                        .WithSetting("TextFieldSettings.Flavor", "html")
                        .WithSetting("TextFieldSettings.Hint", "Enter intro."))

                    .WithField("FirstBlockTitle", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("First Block Title")
                        .WithSetting("TextFieldSettings.Flavor", "large")
                        .WithSetting("TextFieldSettings.Hint", "Enter first block title."))

                    .WithField("FirstBlockContent", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("First Block Content")
                        .WithSetting("TextFieldSettings.Flavor", "html")
                        .WithSetting("TextFieldSettings.Hint", "Enter first block content."))

                    .WithField("FirstBlockTitle1", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("First Block Title1")
                        .WithSetting("TextFieldSettings.Flavor", "large")
                        .WithSetting("TextFieldSettings.Hint", "Enter first block title."))

                    .WithField("FirstBlockIcon1", cfg => cfg
                        .OfType(typeof(MediaLibraryPickerField).Name)
                        .WithDisplayName("First Block Icon 1")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "The selected image will be used as the first icon for the first block."))

                    .WithField("FirstBlockScreenshots1", cfg => cfg
                        .OfType(typeof(MediaLibraryPickerField).Name)
                        .WithDisplayName("First Block Screenshots 1")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "The selected image will be used as the first screenshot for the first block.")
                        .WithSetting("MediaLibraryPickerFieldSettings.Multiple", "True"))

                    .WithField("FirstBlockTitle2", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("First Block Title2")
                        .WithSetting("TextFieldSettings.Flavor", "large")
                        .WithSetting("TextFieldSettings.Hint", "Enter first block title."))

                    .WithField("FirstBlockIcon2", cfg => cfg
                        .OfType(typeof(MediaLibraryPickerField).Name)
                        .WithDisplayName("First Block Icon 2")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "The selected image will be used as the second icon for the first block."))

                    .WithField("FirstBlockScreenshots2", cfg => cfg
                        .OfType(typeof(MediaLibraryPickerField).Name)
                        .WithDisplayName("First Block Screenshots 2")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "The selected image will be used as the second screenshot for the first block.")
                        .WithSetting("MediaLibraryPickerFieldSettings.Multiple", "True"))

                    .WithField("FirstBlockTitle3", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("First Block Title3")
                        .WithSetting("TextFieldSettings.Flavor", "large")
                        .WithSetting("TextFieldSettings.Hint", "Enter first block title."))
                   
                    .WithField("FirstBlockIcon3", cfg => cfg
                        .OfType(typeof(MediaLibraryPickerField).Name)
                        .WithDisplayName("First Block Icon 3")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif"))

                    .WithField("FirstBlockScreenshots3", cfg => cfg
                        .OfType(typeof(MediaLibraryPickerField).Name)
                        .WithDisplayName("First Block Screenshots 3")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Multiple", "True"))

                    .WithField("FirstBlockTitle4", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("First Block Title4")
                        .WithSetting("TextFieldSettings.Flavor", "large")
                        .WithSetting("TextFieldSettings.Hint", "Enter first block title."))

                    .WithField("FirstBlockIcon4", cfg => cfg
                        .OfType(typeof(MediaLibraryPickerField).Name)
                        .WithDisplayName("First Block Icon 4")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif"))

                    .WithField("FirstBlockScreenshots4", cfg => cfg
                        .OfType(typeof(MediaLibraryPickerField).Name)
                        .WithDisplayName("First Block Screenshots 4")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Multiple", "True"))

                    .WithField("FirstBlockTitle5", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("First Block Title5")
                        .WithSetting("TextFieldSettings.Flavor", "large")
                        .WithSetting("TextFieldSettings.Hint", "Enter first block title."))

                    .WithField("FirstBlockIcon5", cfg => cfg
                        .OfType(typeof(MediaLibraryPickerField).Name)
                        .WithDisplayName("First Block Icon 5")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif"))

                    .WithField("FirstBlockScreenshots5", cfg => cfg
                        .OfType(typeof(MediaLibraryPickerField).Name)
                        .WithDisplayName("First Block Screenshots 5")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Multiple", "True"))

                    .WithField("FirstBlockTitle6", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("First Block Title6")
                        .WithSetting("TextFieldSettings.Flavor", "large")
                        .WithSetting("TextFieldSettings.Hint", "Enter first block title."))

                    .WithField("FirstBlockIcon6", cfg => cfg
                        .OfType(typeof(MediaLibraryPickerField).Name)
                        .WithDisplayName("First Block Icon 6")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif"))

                    .WithField("FirstBlockScreenshots6", cfg => cfg
                        .OfType(typeof(MediaLibraryPickerField).Name)
                        .WithDisplayName("First Block Screenshots 6")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Multiple", "True"))

                    .WithField("FirstBlockLayoutStyle", cfg => cfg
                        .OfType(typeof(EnumerationField).Name)
                        .WithDisplayName("First Block Layout Type")
                        .WithSetting("EnumerationFieldSettings.Options",
                            string.Join(System.Environment.NewLine,
                                new[] { "BoxStyle", "TabStyle" }))
                        .WithSetting("EnumerationField.Hint", "Select layout type for first block"))

                    .WithField("SecondBlockTitle", cfg => cfg
                         .OfType(typeof(TextField).Name)
                        .WithDisplayName("Second Block Title")
                        .WithSetting("TextFieldSettings.Flavor", "large")
                        .WithSetting("TextFieldSettings.Hint", "Enter second block title."))

                    .WithField("SecondBlockContent", cfg => cfg
                         .OfType(typeof(TextField).Name)
                        .WithDisplayName("Second Block Content")
                        .WithSetting("TextFieldSettings.Flavor", "html")
                        .WithSetting("TextFieldSettings.Hint", "Enter second block content."))

                    .WithField("ThirdBlockTitle", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("Third Block Title")
                        .WithSetting("TextFieldSettings.Flavor", "large")
                        .WithSetting("TextFieldSettings.Hint", "Enter third block title."))

                    .WithField("ThirdBlockContent", cfg => cfg
                         .OfType(typeof(TextField).Name)
                        .WithDisplayName("Third Block Content")
                        .WithSetting("TextFieldSettings.Flavor", "html")
                        .WithSetting("TextFieldSettings.Hint", "Enter third block content."))

                    .WithField("DocumentsLink", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("Documents Link")
                        .WithSetting("TextFieldSettings.Flavor", "large")
                        .WithSetting("TextFieldSettings.Hint", "Enter a documents link."))

                    .WithField("SamplesLink", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("Samples Link")
                        .WithSetting("TextFieldSettings.Flavor", "large")
                        .WithSetting("TextFieldSettings.Hint", "Enter a samples link."))

                    .WithField("MVALink", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("MVA Link")
                        .WithSetting("TextFieldSettings.Flavor", "large")
                        .WithSetting("TextFieldSettings.Hint", "Enter an MVA link."))

                    .WithField("Ordering", cfg => cfg
                        .OfType("NumericField")
                        .WithDisplayName("Ordering")
                        .WithSetting("NumericFieldSettings.Hint", "Select a numeric position for the item in the ordering."))
                );

            ContentDefinitionManager.AlterTypeDefinition(
                "GettingStarted", cfg => cfg
                    .DisplayedAs("Getting Started")
                    .WithPart("TitlePart")
                    .WithPart("GettingStartedPart")
                    .WithPart("IdentityPart")
                    .WithPart("CommonPart")
                    .Draftable()
                    .Creatable()
                );

            #endregion

            #region Create Getting Started Widget

            ContentDefinitionManager.AlterPartDefinition(
                typeof(GettingStartedWidgetPart).Name, cfg => cfg.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("GettingStartedWidget",
                cfg => cfg
                    .WithPart("WidgetPart")
                    .WithPart("CommonPart")
                    .WithPart("GettingStartedWidgetPart")
                    .WithSetting("Stereotype", "Widget")
                );
            var gettingStartedLayer = _widgetsService.CreateLayer("Getting Started", "The widgets in this layer are displayed on the Getting Started page.", "url '~/getting-started'");
            var gettingStartedWidget = _widgetsService.CreateWidget(gettingStartedLayer.Id, "GettingStartedWidget", "Getting Started", "1", "AfterContent");

            gettingStartedWidget.RenderTitle = false;
            gettingStartedWidget.Name = "gettingStarted";
            _contentManager.Publish(gettingStartedWidget.ContentItem);
            #endregion

            #region Create new pages

            var navGlobal = _menuService.GetMenu("Global Navigation");

            //Create patterns and practices page
            var pagePatternsAndPractices = _contentManager.Create("Page");
            pagePatternsAndPractices.As<TitlePart>().Title = "Patterns and Practices";
            
            //Update alias/url
            var pagePatternsAndPracticesArp = pagePatternsAndPractices.As<AutoroutePart>();
            pagePatternsAndPracticesArp.DisplayAlias = "patterns-and-practices";
            _autorouteService.GenerateAlias(pagePatternsAndPracticesArp);
            _autorouteService.PublishAlias(pagePatternsAndPracticesArp);

            var pagePatternsAndPracticesMenu = pagePatternsAndPractices.As<MenuPart>();
            pagePatternsAndPracticesMenu.Menu = navGlobal;
            pagePatternsAndPracticesMenu.MenuText = pagePatternsAndPractices.As<TitlePart>().Title;
            pagePatternsAndPracticesMenu.MenuPosition = "4.5";
            _contentManager.Publish(pagePatternsAndPractices);

            //Create Getting Started Page
            var pageGettingStarted = _contentManager.Create("Page");
            pageGettingStarted.As<TitlePart>().Title = "Getting Started";
            
            //Update alias/url
            var pageGettingStartedArp = pageGettingStarted.As<AutoroutePart>();
            pageGettingStartedArp.DisplayAlias = "getting-started";
            _autorouteService.GenerateAlias(pageGettingStartedArp);
            _autorouteService.PublishAlias(pageGettingStartedArp);

            var pageGettingStartedMenu = pageGettingStarted.As<MenuPart>();
            pageGettingStartedMenu.Menu = navGlobal;
            pageGettingStartedMenu.MenuText = pageGettingStarted.As<TitlePart>().Title;
            pageGettingStartedMenu.MenuPosition = "2";
            _contentManager.Publish(pageGettingStarted);

            //Create Solutions Page
            var pageSolutions = _contentManager.Create("Page");
            pageSolutions.As<TitlePart>().Title = "Solutions";
            
            //Update alias/url
            var pageSolutionsArp = pageSolutions.As<AutoroutePart>();
            pageSolutionsArp.DisplayAlias = "solutions";
            _autorouteService.GenerateAlias(pageSolutionsArp);
            _autorouteService.PublishAlias(pageSolutionsArp);

            var pageSolutionsMenu = pageSolutions.As<MenuPart>();
            pageSolutionsMenu.Menu = navGlobal;
            pageSolutionsMenu.MenuText = pageSolutions.As<TitlePart>().Title;
            pageSolutionsMenu.MenuPosition = "5";
            _contentManager.Publish(pageSolutions);

            #endregion

            return 5;
        }

        public int UpdateFrom5() {

            var submitAppsLayer = _widgetsService.CreateLayer("Submit Apps Page", "The widgets in this layer are displayed on the resources page.", "url '~/submit-apps'");
            var eduLayer = _widgetsService.CreateLayer("EDU Page","The widgets in this layer are displayed on the resources page.", "url '~/edu'");

            #region Create Feature Slider Widget - Submit Apps

            // Create "Featured Item Group".
            var featuresGroupSubmitApps = _contentManager.Create("FeaturedItemGroup");
            featuresGroupSubmitApps.As<FeaturedItemGroupPart>().Name = "Submit Apps Page";
            featuresGroupSubmitApps.As<FeaturedItemGroupPart>().GroupWidth = 0;
            featuresGroupSubmitApps.As<FeaturedItemGroupPart>().GroupHeight = 0;
            featuresGroupSubmitApps.As<FeaturedItemGroupPart>().IncludeImages = true;
            featuresGroupSubmitApps.As<FeaturedItemGroupPart>().ImageWidth = 0;
            featuresGroupSubmitApps.As<FeaturedItemGroupPart>().ImageHeight = 0;
            featuresGroupSubmitApps.As<FeaturedItemGroupPart>().BackgroundColor = "eb3b00";
            featuresGroupSubmitApps.As<FeaturedItemGroupPart>().ForegroundColor = "ffffff";
            featuresGroupSubmitApps.As<FeaturedItemGroupPart>().SlideSpeed = 700;
            featuresGroupSubmitApps.As<FeaturedItemGroupPart>().SlidePause = 7000;
            featuresGroupSubmitApps.As<FeaturedItemGroupPart>().ShowPager = true;
            featuresGroupSubmitApps.As<FeaturedItemGroupPart>().TransitionEffect = "blindX";
            featuresGroupSubmitApps.As<FeaturedItemGroupPart>().ImageStyle = ImageStyle.Background;
            _contentManager.Publish(featuresGroupSubmitApps);

            // Add "Featured Item Slider Widget" to "Opportunity" layer.
            var featuredItemSliderWidgetSubmitApps = _widgetsService.CreateWidget(submitAppsLayer.Id, "FeaturedItemSliderWidget", "Submit Apps Page", "1", "Featured");
            featuredItemSliderWidgetSubmitApps.RenderTitle = false;
            featuredItemSliderWidgetSubmitApps.Name = "submitapps-featured-item-slider";
            featuredItemSliderWidgetSubmitApps.As<FeaturedItemSliderWidgetPart>().GroupName = "Submit Apps Page";
            _contentManager.Publish(featuredItemSliderWidgetSubmitApps.ContentItem);

            #endregion

            #region Create Feature Slider Widget - EDU

            // Create "Featured Item Group".
            var featuresGroupEdu = _contentManager.Create("FeaturedItemGroup");
            featuresGroupEdu.As<FeaturedItemGroupPart>().Name = "EDU Page";
            featuresGroupEdu.As<FeaturedItemGroupPart>().GroupWidth = 0;
            featuresGroupEdu.As<FeaturedItemGroupPart>().GroupHeight = 0;
            featuresGroupEdu.As<FeaturedItemGroupPart>().IncludeImages = true;
            featuresGroupEdu.As<FeaturedItemGroupPart>().ImageWidth = 0;
            featuresGroupEdu.As<FeaturedItemGroupPart>().ImageHeight = 0;
            featuresGroupEdu.As<FeaturedItemGroupPart>().BackgroundColor = "eb3b00";
            featuresGroupEdu.As<FeaturedItemGroupPart>().ForegroundColor = "ffffff";
            featuresGroupEdu.As<FeaturedItemGroupPart>().SlideSpeed = 700;
            featuresGroupEdu.As<FeaturedItemGroupPart>().SlidePause = 7000;
            featuresGroupEdu.As<FeaturedItemGroupPart>().ShowPager = true;
            featuresGroupEdu.As<FeaturedItemGroupPart>().TransitionEffect = "blindX";
            featuresGroupEdu.As<FeaturedItemGroupPart>().ImageStyle = ImageStyle.Background;
            _contentManager.Publish(featuresGroupEdu);

            // Add "Featured Item Slider Widget" to "Opportunity" layer.
            var featuredItemSliderWidgetEdu = _widgetsService.CreateWidget(eduLayer.Id, "FeaturedItemSliderWidget", "EDU Page", "1", "Featured");
            featuredItemSliderWidgetEdu.RenderTitle = false;
            featuredItemSliderWidgetEdu.Name = "edu-featured-item-slider";
            featuredItemSliderWidgetEdu.As<FeaturedItemSliderWidgetPart>().GroupName = "EDU Page";
            _contentManager.Publish(featuredItemSliderWidgetEdu.ContentItem);

            #endregion

            return 6;
        }

        public int UpdateFrom6() {
            
            #region create solution devices taxonomy

            var solutionDevices = _taxonomyService.GetTaxonomyByName("Solution Device");
            if (solutionDevices != null)
            {
                _taxonomyService.DeleteTaxonomy(solutionDevices.As<TaxonomyPart>());
            }
            var solutionDeviceTaxonomy = _contentManager.New("Taxonomy").As<TaxonomyPart>();
            solutionDeviceTaxonomy.Name = "Solution Device";
            solutionDeviceTaxonomy.ContentItem.As<TitlePart>().Title = "Solution Device";
            _taxonomyService.CreateTermContentType(solutionDeviceTaxonomy);
            _contentManager.Create(solutionDeviceTaxonomy, VersionOptions.Published);

            CreateTerm(solutionDeviceTaxonomy, "WinPhone", 1);
            CreateTerm(solutionDeviceTaxonomy, "PC", 2);
            CreateTerm(solutionDeviceTaxonomy, "Tablet", 3);
            CreateTerm(solutionDeviceTaxonomy, "Android", 4);
            CreateTerm(solutionDeviceTaxonomy, "iOS", 5);
            CreateTerm(solutionDeviceTaxonomy, "Other", 6);


            // create solution primary workload invoked taxonomy
            var solutionPrimaryWorkload = _taxonomyService.GetTaxonomyByName("Solution Primary Workload Invoked");
            if (solutionPrimaryWorkload != null)
            {
                _taxonomyService.DeleteTaxonomy(solutionPrimaryWorkload.As<TaxonomyPart>());
            }
            var solutionPrimaryWorkloadTaxonomy = _contentManager.New("Taxonomy").As<TaxonomyPart>();
            solutionPrimaryWorkloadTaxonomy.Name = "Solution Primary Workload Invoked";
            solutionPrimaryWorkloadTaxonomy.ContentItem.As<TitlePart>().Title = "Solution Primary Workload Invoked";
            _taxonomyService.CreateTermContentType(solutionPrimaryWorkloadTaxonomy);
            _contentManager.Create(solutionPrimaryWorkloadTaxonomy, VersionOptions.Published);

            CreateTerm(solutionPrimaryWorkloadTaxonomy, "Office 365", 1);
            CreateTerm(solutionPrimaryWorkloadTaxonomy, "Office", 2);
            CreateTerm(solutionPrimaryWorkloadTaxonomy, "SharePoint", 3);
            CreateTerm(solutionPrimaryWorkloadTaxonomy, "OneDrive", 4);

            // create solution identity integration method
            var solutionIntegrationMethod = _taxonomyService.GetTaxonomyByName("Solution Identity Integration Method");
            if (solutionIntegrationMethod != null)
            {
                _taxonomyService.DeleteTaxonomy(solutionIntegrationMethod.As<TaxonomyPart>());
            }
            var solutionIntegrationIntegrationMethodTaxonomy = _contentManager.New("Taxonomy").As<TaxonomyPart>();
            solutionIntegrationIntegrationMethodTaxonomy.Name = "Solution Identity Integration Method";
            solutionIntegrationIntegrationMethodTaxonomy.ContentItem.As<TitlePart>().Title = "Solution Identity Integration Method";
            _taxonomyService.CreateTermContentType(solutionIntegrationIntegrationMethodTaxonomy);
            _contentManager.Create(solutionIntegrationIntegrationMethodTaxonomy, VersionOptions.Published);

            CreateTerm(solutionIntegrationIntegrationMethodTaxonomy, "Azure AD Users and Groups", 1);
            CreateTerm(solutionIntegrationIntegrationMethodTaxonomy, "SharePoint Taxonomy", 2);
            CreateTerm(solutionIntegrationIntegrationMethodTaxonomy, "Search", 3);
            CreateTerm(solutionIntegrationIntegrationMethodTaxonomy, "SharePoint User Profiles", 4);
            CreateTerm(solutionIntegrationIntegrationMethodTaxonomy, "Business Connectivity Services", 5);
            CreateTerm(solutionIntegrationIntegrationMethodTaxonomy, "Office 365 API", 6);
            
            #endregion

            #region Update Solutions content type

            ContentDefinitionManager.AlterPartDefinition(
                "SolutionPart", builder => builder
                     .WithField("SolutionDevice", cfg => cfg
                        .OfType(typeof(TaxonomyField).Name)
                        .WithDisplayName("Device")
                        .WithSetting("TaxonomyFieldSettings.Hint", "Enter a solution Device.")
                    .WithSetting("TaxonomyFieldSettings.Taxonomy", solutionDeviceTaxonomy.Name))

                    .WithField("SolutionPrimaryWorkloadInvoked", cfg => cfg
                        .OfType(typeof(TaxonomyField).Name)
                        .WithDisplayName("Primary Workload Invoked")
                        .WithSetting("TaxonomyFieldSettings.Hint", "Enter a primary workload invoked.")
                    .WithSetting("TaxonomyFieldSettings.Taxonomy", solutionPrimaryWorkloadTaxonomy.Name))

                    .WithField("SolutionIdentityIntegrationMethod", cfg => cfg
                        .OfType(typeof(TaxonomyField).Name)
                        .WithDisplayName("Identity Integration Method")
                        .WithSetting("TaxonomyFieldSettings.Hint", "Enter a integration method.")
                    .WithSetting("TaxonomyFieldSettings.Taxonomy", solutionIntegrationIntegrationMethodTaxonomy.Name))

            );

            #endregion
            
            return 7;
        }

        public int UpdateFrom7()
        {

            #region Creating custom part - RelatedLinks

            SchemaBuilder.CreateTable("RelatedLinksRecord", table => table
                .ContentPartRecord()
                .Column<string>("RelatedLinksGroup")
                .Column<string>("LinksJson", column => column.Unlimited())
            );

            SchemaBuilder.CreateTable("RelatedLinkRecord",
              table => table
                  .Column<int>("Id", column => column.PrimaryKey().Identity())
                  .Column<int>("RelatedLinksRecord_Id")
                  .Column<string>("Type")
                  .Column<string>("Title")
                  .Column<string>("Url")
                  .Column<int>("SortOrder")
            );

            ContentDefinitionManager.AlterPartDefinition(
                typeof(RelatedLinksPart).Name, cfg => cfg.Attachable());

            #endregion

            ContentDefinitionManager.AlterTypeDefinition(
               "CodeSample", cfg => cfg
                   .WithPart("RelatedLinksPart"));

            ContentDefinitionManager.AlterTypeDefinition(
                "Training", cfg => cfg
                    .WithPart("RelatedLinksPart"));

            ContentDefinitionManager.AlterTypeDefinition(
                "Podcast", cfg => cfg
                    .WithPart("RelatedLinksPart"));

            return 8;
        }

        public int UpdateFrom8()
        {

            #region Create Ungrouped Tiles content type

            ContentDefinitionManager.AlterPartDefinition(
                "TilePart", builder => builder
                    .WithField("Icon", cfg => cfg
                        .OfType(typeof (MediaLibraryPickerField).Name)
                        .WithDisplayName("Icon")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif"))
                    .WithField("Description", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Description")
                        .WithSetting("TextFieldSettings.Flavor", "textarea")
                        .WithSetting("TextFieldSettings.Hint", "Title to be displayed beneath the banner."))
                    .WithField("Url", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Url")
                        .WithSetting("TextFieldSettings.Flavor", "large")
                        .WithSetting("TextFieldSettings.Hint", "Tile Url"))
                    .WithField("LinkText", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Link Text")
                        .WithSetting("TextFieldSettings.Flavor", "large")
                        .WithSetting("TextFieldSettings.Hint", "Tile link text. Default text will be View."))
                    .Attachable()
            );

            ContentDefinitionManager.AlterTypeDefinition(
                "Tile", cfg => cfg
                    .DisplayedAs("Tile")
                    .WithPart("TitlePart")
                    .WithPart("TilePart")
                    .WithPart("CommonPart")
                    .WithPart("MetaPart")
                    .Draftable()
                    .Creatable()
                );
            #endregion

            #region Create Sections With Tiles Page

            //Create Sections With Tiles Page content part
            ContentDefinitionManager.AlterPartDefinition(
                "SectionsWithTilesPagePart", builder => builder
                    .WithField("BannerImage", cfg => cfg
                        .OfType(typeof(MediaLibraryPickerField).Name)
                        .WithDisplayName("Banner Image")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif"))
                    .WithField("Subtitle", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("Subtitle")
                        .WithSetting("TextFieldSettings.Flavor", "large")
                        .WithSetting("TextFieldSettings.Hint", "Title to be displayed beneath the banner."))
                    .WithField("Body", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithSetting("TextFieldSettings.Flavor", "html")
                        .WithDisplayName("Body"))
                    .WithField("ExternalLink", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("External Link")
                        .WithSetting("TextFieldSettings.Hint", "Url for the link at bottom of page."))
                    .WithField("LinkText", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("Link Text")
                        .WithSetting("TextFieldSettings.Hint", "Text to be displayed in link at bottom of page."))
                    .WithField("Image", cfg => cfg
                        .OfType(typeof(MediaLibraryPickerField).Name)
                        .WithDisplayName("Image")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "Image to be displayed on right side of screen."))
                    .WithField("TilesPageStyle", cfg => cfg
                        .OfType(typeof(EnumerationField).Name)
                        .WithDisplayName("Tiles Page Style")
                        .WithSetting("EnumerationFieldSettings.Options",
                            string.Join(System.Environment.NewLine,
                                new[] { "Columns", "Rows" }))
                        .WithSetting("EnumerationField.Hint", "Select tiles display Style"))

                    .WithField("PartnerLogos", cfg => cfg
                                .OfType("MediaLibraryPickerField")
                                .WithDisplayName("Partner Logos")
                                .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                                .WithSetting("MediaLibraryPickerFieldSettings.Multiple", "True")
                                .WithSetting("MediaLibraryPickerFieldSettings.Hint", "Upload multiple logos"))

                    .WithField("Section1Title", cfg => cfg
                       .OfType(typeof(TextField).Name)
                       .WithDisplayName("Section 1 Title")
                       .WithSetting("TextFieldSettings.Flavor", "large"))
                   .WithField("Section1Body", cfg => cfg
                       .OfType(typeof(TextField).Name)
                       .WithDisplayName("Section 1 Body")
                       .WithSetting("TextFieldSettings.Flavor", "textarea"))
                   .WithField("Section1ExternalLink", cfg => cfg
                       .OfType(typeof(TextField).Name)
                       .WithDisplayName("Section 1 External Link")
                       .WithSetting("TextFieldSettings.Flavor", "large"))
                   .WithField("Section1LinkText", cfg => cfg
                       .OfType(typeof(TextField).Name)
                       .WithDisplayName("Section 1 Link Text")
                       .WithSetting("TextFieldSettings.Hint", "Text to be displayed in link.")
                       .WithSetting("TextFieldSettings.Flavor", "large"))
                    .WithField("Section1Tiles", cfg => cfg
                        .OfType("ContentPickerField")
                        .WithDisplayName("Section 1 Tiles")
                        .WithSetting("ContentPickerFieldSettings.Multiple", "True")
                        .WithSetting("ContentPickerFieldSettings.ShowContentTab", "True")
                        .WithSetting("ContentPickerFieldSettings.DisplayedContentTypes", "Tile")
                        .WithSetting("ContentPickerFieldSettings.Hint", "Pick the tiles to be displayed on this section"))

                    .WithField("Section2Title", cfg => cfg
                       .OfType(typeof(TextField).Name)
                       .WithDisplayName("Section 2 Title")
                       .WithSetting("TextFieldSettings.Flavor", "large"))
                   .WithField("Section2Body", cfg => cfg
                       .OfType(typeof(TextField).Name)
                       .WithDisplayName("Section 2 Body")
                       .WithSetting("TextFieldSettings.Flavor", "textarea"))
                   .WithField("Section2ExternalLink", cfg => cfg
                       .OfType(typeof(TextField).Name)
                       .WithDisplayName("Section 2 External Link"))
                   .WithField("Section2LinkText", cfg => cfg
                       .OfType(typeof(TextField).Name)
                       .WithDisplayName("Section 2 Link Text")
                       .WithSetting("TextFieldSettings.Hint", "Text to be displayed in link."))
                    .WithField("Section2Tiles", cfg => cfg
                        .OfType("ContentPickerField")
                        .WithDisplayName("Section 2 Tiles")
                        .WithSetting("ContentPickerFieldSettings.Multiple", "True")
                        .WithSetting("ContentPickerFieldSettings.ShowContentTab", "True")
                        .WithSetting("ContentPickerFieldSettings.DisplayedContentTypes", "Tile")
                        .WithSetting("ContentPickerFieldSettings.Hint", "Pick the tiles to be displayed on this section"))
                );

            //Create Sections With Tiles Page content item type
            ContentDefinitionManager.AlterTypeDefinition(
                "SectionsWithTilesPage", cfg => cfg
                    .DisplayedAs("Sections With Tiles Page")
                    .WithPart("TitlePart")
                    .WithPart("SectionsWithTilesPagePart")
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
            
            return 9;
        }

        public int UpdateFrom9() {
            ContentDefinitionManager.AlterPartDefinition(
                "SectionsWithTilesPagePart", builder => builder
                    .WithField("BottomRowResources", cfg => cfg
                        .OfType("ContentPickerField")
                        .WithDisplayName("Bottom Row Resources")
                        .WithSetting("ContentPickerFieldSettings.Multiple", "True")
                        .WithSetting("ContentPickerFieldSettings.ShowContentTab", "True")
                        .WithSetting("ContentPickerFieldSettings.DisplayedContentTypes", "Resource")
                        .WithSetting("ContentPickerFieldSettings.Hint", "Pick the image with description resources to be displayed in the bottom row.")));
            return 10;
        }

        public int UpdateFrom10() {
            ContentDefinitionManager.AlterPartDefinition(
                "SectionsWithTilesPagePart", builder => builder
                    .WithField("Body", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithSetting("TextFieldSettings.Flavor", "textarea")
                        .WithDisplayName("Body"))
                    .WithField("Section1Body", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("Section 2 Body")
                        .WithSetting("TextFieldSettings.Flavor", "html"))
                    .WithField("Section2Body", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("Section 2 Body")
                        .WithSetting("TextFieldSettings.Flavor", "html")));

            return 11;

        }

        public int UpdateFrom11()
        {

            ContentDefinitionManager.AlterPartDefinition(
                "SectionsWithTilesPagePart", builder => builder
                    .WithField("PartnerLogosTitle", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("Partner Logos Title")
                        .WithSetting("TextFieldSettings.Flavor", "large")));

            #region Create "BannerWithDescriptionPagePart" content part for PnP page

            ContentDefinitionManager.AlterPartDefinition(
                "BannerWithDescriptionPagePart", builder => builder
                    .WithField("TopBannerDescription", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Top Banner Description")
                        .WithSetting("TextFieldSettings.Flavor", "textarea")
                        .WithSetting("TextFieldSettings.Hint", "Description to be displayed in top banner description box."))
                    .WithField("TopBannerLink", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Top Banner Link")
                        .WithSetting("TextFieldSettings.Hint", "Url for the link in top banner description box."))
                    .WithField("TopBannerLinkText", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("Top Banner Link Text")
                        .WithSetting("TextFieldSettings.Hint", "Link Text to be displayed in top banner description box."))
                    .WithField("TopBannerThumbnailImage", cfg => cfg
                        .OfType(typeof (MediaLibraryPickerField).Name)
                        .WithDisplayName("Top Banner Profile Image")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "Top Banner Profile Image to be displayed on bottom right corner of the top banner.")));

            //Create Sections With Tiles Page content item type
            ContentDefinitionManager.AlterTypeDefinition(
                "Page", cfg => cfg
                    .WithPart("BannerWithDescriptionPagePart"));
                    
            #endregion

            #region Create Article Topic taxonomy

            var articleType = _taxonomyService.GetTaxonomyByName("Article Topic");
            if (articleType != null)
            {
                _taxonomyService.DeleteTaxonomy(articleType.As<TaxonomyPart>());
            }

            //creating video type Taxonomy
            var articleTypeTaxonomy = _contentManager.New("Taxonomy").As<TaxonomyPart>();
            articleTypeTaxonomy.Name = "Article Topic";
            articleTypeTaxonomy.ContentItem.As<TitlePart>().Title = "Article Topic";
            _taxonomyService.CreateTermContentType(articleTypeTaxonomy);
            _contentManager.Create(articleTypeTaxonomy, VersionOptions.Published);

            CreateTerm(articleTypeTaxonomy, "Article Type 1", 1);
            CreateTerm(articleTypeTaxonomy, "Article Type 2", 2);
            CreateTerm(articleTypeTaxonomy, "Article Type 3", 3);


            #endregion

            #region Create Article content type

            ContentDefinitionManager.AlterPartDefinition(
                "ArticlePart", builder => builder
                    .WithField("AuthorFirstName", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("Author First Name")
                        .WithSetting("TextFieldSettings.Flavor", "large")
                        .WithSetting("TextFieldSettings.Required", "true"))
                    .WithField("AuthorLastName", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("Author Last Name")
                        .WithSetting("TextFieldSettings.Flavor", "large")
                        .WithSetting("TextFieldSettings.Required", "true"))
                    .WithField("Description", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("Description")
                        .WithSetting("TextFieldSettings.Flavor", "textarea")
                        .WithSetting("TextFieldSettings.Required", "true"))
                    .WithField("Image", cfg => cfg
                        .OfType(typeof(MediaLibraryPickerField).Name)
                        .WithDisplayName("Image")
                        .WithSetting("MediaLibraryPickerFieldSettings.AllowedExtensions", "jpg png gif")
                        .WithSetting("MediaLibraryPickerFieldSettings.Hint", "This will be the article thumbnail."))
                    .WithField("Link", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("Link")
                        .WithSetting("TextFieldSettings.Flavor", "large")
                        .WithSetting("TextFieldSettings.Hint", "External link to the article.")
                        .WithSetting("TextFieldSettings.Required", "true"))
                     .WithField("ArticleTopic", cfg => cfg
                        .OfType(typeof(TaxonomyField).Name)
                        .WithDisplayName("Article Topic")
                        .WithSetting("TaxonomyFieldSettings.Hint", "Choose a Topic for the article.")
                        .WithSetting("TaxonomyFieldSettings.Taxonomy", articleTypeTaxonomy.Name)
                        .WithSetting("TaxonomyFieldSettings.Required", "true"))
                    );


            ContentDefinitionManager.AlterTypeDefinition(
                "Article", cfg => cfg
                    .DisplayedAs("Article")
                    .WithPart("TitlePart")
                    .WithPart("ArticlePart")
                    .WithPart("CommonPart", builder => builder
                        .WithSetting("DateEditorSettings.ShowDateEditor", "true"))
                    .Draftable()
                    .Creatable());

            #endregion

            #region Create Home Blogs Widget

            var homepageLayer = _widgetsService.GetLayers().FirstOrDefault(l => l.Name == "TheHomepage");

            ContentDefinitionManager.AlterPartDefinition(
                typeof(HomeArticlesPart).Name, cfg => cfg.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("HomeBlogsWidget",
                cfg => cfg
                    .WithPart("WidgetPart")
                    .WithPart("HomeArticlesPart")
                    .WithPart("CommonPart")
                    .WithSetting("Stereotype", "Widget")
                );

            var homeArticlesWidget = _widgetsService.CreateWidget(homepageLayer.Id, "HomeBlogsWidget", "Home Blogs", "7", "BeforeContent");
            homeArticlesWidget.RenderTitle = false;
            homeArticlesWidget.Name = "home-blogs";
            _contentManager.Publish(homeArticlesWidget.ContentItem);

            #endregion

            #region Create Blogs Widget

            var articleLayer = _widgetsService.CreateLayer("Blogs", "The widgets in this layer are displayed on the articles page.", "url '~/blogs'");

            ContentDefinitionManager.AlterPartDefinition(
                typeof(ArticlesPart).Name, cfg => cfg.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("BlogsWidget",
                cfg => cfg
                    .WithPart("WidgetPart")
                    .WithPart("ArticlesPart")
                    .WithPart("CommonPart")
                    .WithSetting("Stereotype", "Widget")
                );

            var articlesWidget = _widgetsService.CreateWidget(articleLayer.Id, "BlogsWidget", "Blogs", "1", "Content");
            articlesWidget.RenderTitle = false;
            articlesWidget.Name = "blogs";
            _contentManager.Publish(articlesWidget.ContentItem);

            #endregion

            #region Create Banner content type

            ContentDefinitionManager.AlterPartDefinition(
                "BannerWidgetPart", builder => builder
                    .WithField("BodyText", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("BodyText")
                        .WithSetting("TextFieldSettings.Flavor", "textarea")
                        .WithSetting("TextFieldSettings.Hint", "Text to be displayed in the banner area."))
                    .WithField("ExternalLink", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("Sign Up Link")
                        .WithSetting("TextFieldSettings.Hint", "Link for the banner's button.")));

            ContentDefinitionManager.AlterTypeDefinition(
                "BannerWidget", cfg => cfg
                    .DisplayedAs("Banner")
                    .WithPart("WidgetPart")
                    .WithPart("BannerWidgetPart")
                    .WithPart("IdentityPart")
                    .WithPart("CommonPart")
                );

            var bannerWidget = _widgetsService.CreateWidget(homepageLayer.Id, "BannerWidget", "Sign Up Banner", "2", "Content");
            bannerWidget.RenderTitle = false;
            bannerWidget.Name = "bannerWidget";
            _contentManager.Publish(bannerWidget.ContentItem);

            #endregion

            return 12;
        }

        public int UpdateFrom12() {
            #region Add field for permalink anchor tag on Podcasts, Code Samples, Training, and PnP

            ContentDefinitionManager.AlterPartDefinition(
                "PodcastPart", builder => builder
                    .WithField("PermalinkTag", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("Permalink Tag")
                        .WithSetting("TextFieldSettings.Hint", "Podcast identifier to be used at end of permalink (ex: dev.office.com/podcasts#<your-given-value-here>).")));

            ContentDefinitionManager.AlterPartDefinition(
                "CodeSamplePart", builder => builder
                    .WithField("PermalinkTag", cfg => cfg
                        .OfType(typeof(TextField).Name)
                        .WithDisplayName("Permalink Tag")
                        .WithSetting("TextFieldSettings.Hint", "Code sample identifier to be used at end of permalink (ex: dev.office.com/code-samples#<your-given-value-here>).")));

            ContentDefinitionManager.AlterPartDefinition(
                "TrainingPart", builder => builder
                    .WithField("PermalinkTag", cfg => cfg
                    .OfType(typeof(TextField).Name)
                    .WithDisplayName("Permalink Tag")
                    .WithSetting("TextFieldSettings.Hint", "Training item identifier to be used at end of permalink (ex: dev.office.com/training#<your-given-value-here>).")));

            ContentDefinitionManager.AlterPartDefinition(
                "PatternsAndPracticesPart", builder => builder
                    .WithField("PermalinkTag", cfg => cfg
                    .OfType(typeof(TextField).Name)
                    .WithDisplayName("Permalink Tag")
                    .WithSetting("TextFieldSettings.Hint", "Patterns and practices item identifier to be used at end of permalink (ex: dev.office.com/patterns-and-practices#<your-given-value-here>).")));

            #endregion

            return 13;
        }

        public int UpdateFrom13()
        {
            ContentDefinitionManager.AlterPartDefinition(
              "BannerWidgetPart", builder => builder
                  .WithField("LinkText", cfg => cfg
                      .OfType(typeof(TextField).Name)
                      .WithDisplayName("Link Text"))
                  .WithField("LinkBackgroundColor", cfg => cfg
                      .OfType(typeof(TextField).Name)
                      .WithDisplayName("Link Background Color"))
                  .WithField("LinkTextColor", cfg => cfg
                      .OfType(typeof(TextField).Name)
                      .WithDisplayName("Link Text Color")));
            return 14;
        }

        public int UpdateFrom14() {
            ContentDefinitionManager.AlterPartDefinition(
                "SectionsWithTilesPagePart", builder => builder
                    .WithField("Body", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithSetting("TextFieldSettings.Flavor", "html")
                        .WithDisplayName("Body")));

            return 15;
        }

        public int UpdateFrom15() {
            ContentDefinitionManager.AlterPartDefinition(
                "BannerWidgetPart", builder => builder
                    .WithField("BodyText", cfg => cfg
                        .OfType(typeof (TextField).Name)
                        .WithDisplayName("BodyText")
                        .WithSetting("TextFieldSettings.Flavor", "html")
                        .WithSetting("TextFieldSettings.Hint", "Text to be displayed in the banner area.")));
            return 16;
        }

        public int UpdateFrom16() {
            #region add ViewCountRecord Table
            SchemaBuilder.CreateTable(typeof(ViewCountRecord).Name,
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<int>("LinkId")
                    .Column<DateTime>("Date")
                );
            #endregion

            return 17;
        }

        public int UpdateFrom17(){
            SchemaBuilder.AlterTable("ViewCountRecord", table => table
                .AddColumn<string>("Type")
            );
            return 18;
        }

        public int UpdateFrom18() {
            ContentDefinitionManager.AlterTypeDefinition(
              "PatternsAndPractices", cfg => cfg
                  .WithPart("RelatedLinksPart"));
            return 19;
        }

        public int UpdateFrom19()
        {


            ContentDefinitionManager.AlterPartDefinition(
                "PatternsAndPracticesPart", builder => builder
                    .WithField("PatternsAndPracticesPlatform", cfg => cfg
                        .OfType("TaxonomyField")
                        .WithDisplayName("Patterns and Practices Platform")
                        .WithSetting("TaxonomyFieldSettings.Taxonomy", "CodeSamplePlatform")));

            ContentDefinitionManager.AlterPartDefinition(
                "PatternsAndPracticesPart", builder => builder
                    .WithField("PatternsAndPracticesLanguage", cfg => cfg
                        .OfType("TaxonomyField")
                        .WithDisplayName("Patterns and Practices Language")
                        .WithSetting("TaxonomyFieldSettings.Taxonomy", "CodeSampleLanguage")));

            ContentDefinitionManager.AlterPartDefinition(
                "PatternsAndPracticesPart", builder => builder
                    .WithField("PatternsAndPracticesService", cfg => cfg
                        .OfType("TaxonomyField")
                        .WithDisplayName("Pattern and Practices Service")
                        .WithSetting("TaxonomyFieldSettings.Taxonomy", "CodeSampleServices")));

            ContentDefinitionManager.AlterPartDefinition(
                "PatternsAndPracticesPart", builder => builder
                    .WithField("PatternsAndPracticesSource", cfg => cfg
                        .OfType("TaxonomyField")
                        .WithDisplayName("Patterns and Practices Source")
                        .WithSetting("TaxonomyFieldSettings.Taxonomy", "CodeSampleSource")));

            ContentDefinitionManager.AlterPartDefinition(
                "PatternsAndPracticesPart", builder => builder
                    .WithField("PatternsAndPracticesProduct", cfg => cfg
                        .OfType("TaxonomyField")
                        .WithDisplayName("Patterns and Practices Product")
                        .WithSetting("TaxonomyFieldSettings.Taxonomy", "CodeSampleProducts")));


            return 20;
        }

        public int UpdateFrom20()
        {
            #region Create Single Blog Post Widget

            var singleBlogLayer = _widgetsService.CreateLayer("Single Blog", "The widgets in this layer are displayed on the single blog page", "url '~/blogs/blog'");

            ContentDefinitionManager.AlterPartDefinition(
                typeof(SingleBlogWidgetPart).Name, cfg => cfg.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("SingleBlogWidget",
                cfg => cfg
                    .WithPart("WidgetPart")
                    .WithPart("SingleBlogWidgetPart")
                    .WithPart("CommonPart")
                    .WithSetting("Stereotype", "Widget")
                );

            var singleBlogWidget = _widgetsService.CreateWidget(singleBlogLayer.Id, "SingleBlogWidget", "Single Blog Item", "1", "AfterContent");
            singleBlogWidget.RenderTitle = false;
            singleBlogWidget.Name = "SingleBlog";
            _contentManager.Publish(singleBlogWidget.ContentItem);

            #endregion

            return 21;
        }
        public int UpdateFrom21()
        {
            ContentDefinitionManager.AlterPartDefinition(
             "ArticlePart", builder => builder
                 .WithField("PostBody", cfg => cfg
                     .OfType(typeof(TextField).Name)
                     .WithDisplayName("Blog post body")
                     .WithSetting("TextFieldSettings.Flavor", "html"))
                     );

            return 22;
        }

        public int UpdateFrom22()
        {
            ContentDefinitionManager.AlterPartDefinition(
                "PatternsAndPracticesPart", builder => builder
                    .WithField("PatternsAndPracticesSecondaryType", cfg => cfg
                        .OfType("TaxonomyField")
                        .WithDisplayName("Secondary Patterns and Practices Type")
                        .WithSetting("TaxonomyFieldSettings.Taxonomy", "CodeSampleType")));

            return 23;
        }

      

        private void CreateTerm(TaxonomyPart taxonomyPart, string termName, int order) {
            
            if (taxonomyPart != null && termName != null) 
            {
                var term = _taxonomyService.NewTerm(taxonomyPart);
                term.Container = taxonomyPart.ContentItem;
                term.Name = termName.Trim();
                term.Weight = order;
                term.Selectable = true;

                _taxonomyService.ProcessPath(term);
                _contentManager.Create(term, VersionOptions.Published);
            }
        }
    }

    [OrchardFeature("DevOffice.Common.SEO")]
    public class SeoMigrations : DataMigrationImpl
    {
        public int Create()
        {
            ContentDefinitionManager.AlterTypeDefinition(
               "Page", cfg => cfg
                   .WithPart("MetaPart")
               );

            ContentDefinitionManager.AlterTypeDefinition(
               "StepPage", cfg => cfg
                   .WithPart("MetaPart")
               );
            return 1;
        }
    }

    [OrchardFeature("DevOffice.Common.Search")]
    public class SearchMigrations : DataMigrationImpl
    {
        private readonly IIndexManager _indexManager;
        private readonly IOrchardServices _orchardServices;
        private readonly IIndexingService _indexingService;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IWidgetsService _widgetsService;
        private readonly IContentManager _contentManager;
        private const string IndexName = "SearchIndex";

        public SearchMigrations(
            IIndexManager indexManager, 
            IOrchardServices orchardServices,
            IIndexingService indexingService,
            IContentDefinitionManager contentDefinitionManager,
            IWidgetsService widgetService,
            IContentManager contentManager)
        {
            _indexManager = indexManager;
            _orchardServices = orchardServices;
            _indexingService = indexingService;
            _contentDefinitionManager = contentDefinitionManager;
            _widgetsService = widgetService;
            _contentManager = contentManager;
        }

        public int Create()
        {

            #region Search widget

            ContentDefinitionManager.AlterTypeDefinition("DevOfficeSearchForm",
                cfg => cfg
                    .WithPart("DevOfficeSearchFormPart")
                    .WithPart("CommonPart")
                    .WithPart("WidgetPart")
                    .WithSetting("Stereotype", "Widget")
                );

            var defaultPageLayer = _widgetsService.GetLayers().FirstOrDefault(l => l.Name == "Default");

            var resourcesWidget = _widgetsService.CreateWidget(defaultPageLayer.Id, "DevOfficeSearchForm", "Search", "6", "Navigation");
            resourcesWidget.RenderTitle = false;
            resourcesWidget.Name = "search";
            _contentManager.Publish(resourcesWidget.ContentItem);

            #endregion 

            #region Indexing all the content types fields

            _indexManager.GetSearchIndexProvider().CreateIndex(IndexName);
            

            var settings = _orchardServices.WorkContext.CurrentSite.As<SearchSettingsPart>();

            if (settings != null) {
                settings.SearchIndex = IndexName;
            }

            var page = _contentDefinitionManager.GetTypeDefinition("Page");
            page.Settings["TypeIndexing.Indexes"] = IndexName;
            _contentDefinitionManager.StoreTypeDefinition(page);

            var stepPage = _contentDefinitionManager.GetTypeDefinition("StepPage");
            stepPage.Settings["TypeIndexing.Indexes"] = IndexName;
            _contentDefinitionManager.StoreTypeDefinition(stepPage);

            var codeSample = _contentDefinitionManager.GetTypeDefinition("CodeSample");
            codeSample.Settings["TypeIndexing.Indexes"] = IndexName;
            _contentDefinitionManager.StoreTypeDefinition(codeSample);
            
            var resource = _contentDefinitionManager.GetTypeDefinition("Resource");
            resource.Settings["TypeIndexing.Indexes"] = IndexName;
            _contentDefinitionManager.StoreTypeDefinition(resource);

            var training = _contentDefinitionManager.GetTypeDefinition("Training");
            training.Settings["TypeIndexing.Indexes"] = IndexName;
            _contentDefinitionManager.StoreTypeDefinition(training);

            var videoItem = _contentDefinitionManager.GetTypeDefinition("VideoItem");
            videoItem.Settings["TypeIndexing.Indexes"] = IndexName;
            _contentDefinitionManager.StoreTypeDefinition(videoItem);

            var podcast = _contentDefinitionManager.GetTypeDefinition("Podcast");
            podcast.Settings["TypeIndexing.Indexes"] = IndexName;
            _contentDefinitionManager.StoreTypeDefinition(podcast);

            var eventType = _contentDefinitionManager.GetTypeDefinition("Event");
            eventType.Settings["TypeIndexing.Indexes"] = IndexName;
            _contentDefinitionManager.StoreTypeDefinition(eventType);

            var solution = _contentDefinitionManager.GetTypeDefinition("Solution");
            solution.Settings["TypeIndexing.Indexes"] = IndexName;
            _contentDefinitionManager.StoreTypeDefinition(solution);

            var patternsAndPracticesType = _contentDefinitionManager.GetTypeDefinition("PatternsAndPractices");
            patternsAndPracticesType.Settings["TypeIndexing.Indexes"] = IndexName;
            _contentDefinitionManager.StoreTypeDefinition(patternsAndPracticesType);

            var gettingStartedType = _contentDefinitionManager.GetTypeDefinition("GettingStarted");
            gettingStartedType.Settings["TypeIndexing.Indexes"] = IndexName;
            _contentDefinitionManager.StoreTypeDefinition(gettingStartedType);

            var blogsType = _contentDefinitionManager.GetTypeDefinition("Article");
            blogsType.Settings["TypeIndexing.Indexes"] = IndexName;
            _contentDefinitionManager.StoreTypeDefinition(blogsType);

            ContentDefinitionManager.AlterPartDefinition(
               "CodeSamplePart", builder => builder
                   .WithField("SubText", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                   .WithField("StartDate", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                   .WithField("EndDate", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                   .WithField("ExternalLink", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                   .WithField("Location", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                   .WithField("CodeSampleType", cfg => cfg.WithSetting("FieldIndexing.Included", "True")) 
                   .WithField("CodeSamplePlatform", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                   .WithField("CodeSampleLanguage", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                   .WithField("CodeSampleServices", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                   .WithField("CodeSampleSource", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                   .WithField("CodeSampleProducts", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
               );

            ContentDefinitionManager.AlterPartDefinition(
               "ResourcePart", builder => builder
                  .WithField("SubText", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                  .WithField("LearnMoreText", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                  .WithField("LearnMoreUrl", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                  .WithField("Image", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
               );

            ContentDefinitionManager.AlterPartDefinition(
                "TrainingPart", builder => builder
                   .WithField("SubText", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                   .WithField("ExternalLink", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                   .WithField("Location", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                   .WithField("TrainingType", cfg => cfg.WithSetting("FieldIndexing.Included", "True")) 
                );

            ContentDefinitionManager.AlterPartDefinition(
                "VideoItemPart", builder => builder
                   .WithField("SubText", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                   .WithField("Location", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                   .WithField("VideoType", cfg => cfg.WithSetting("FieldIndexing.Included", "True")) 
                );

            ContentDefinitionManager.AlterPartDefinition(
                "PodcastPart", builder => builder
                   .WithField("SubText", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                   .WithField("ExternalLink", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                   .WithField("Location", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                   .WithField("PodcastType", cfg => cfg.WithSetting("FieldIndexing.Included", "True")) 
                );

            ContentDefinitionManager.AlterPartDefinition(
                "EventPart", builder => builder
                   .WithField("SubText", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                   .WithField("ExternalLink", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                   .WithField("Location", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                   .WithField("EventType", cfg => cfg.WithSetting("FieldIndexing.Included", "True")) 
                );

            ContentDefinitionManager.AlterPartDefinition(
                "SolutionPart", builder => builder
                    .WithField("Content", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                    .WithField("SolutionType", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                    .WithField("SolutionDevice", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                    .WithField("SolutionPrimaryWorkloadInvoked", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                    .WithField("SolutionIdentityIntegrationMethod", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                );

            ContentDefinitionManager.AlterPartDefinition(
               "PatternsAndPracticesPart", builder => builder
                   .WithField("SubText", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                   .WithField("GitHub Url", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
                   .WithField("PatternsAndPracticesType", cfg => cfg.WithSetting("FieldIndexing.Included", "True"))
               );

            ContentDefinitionManager.AlterPartDefinition(
                "GettingStartedPart", builder => builder
                    .WithField("Intro", cfg => cfg.WithSetting("TextFieldSettings.Hint", "Enter intro."))
                    .WithField("FirstBlockTitle", cfg => cfg.WithSetting("TextFieldSettings.Hint", "Enter first block title."))
                    .WithField("FirstBlockContent", cfg => cfg.WithSetting("TextFieldSettings.Hint", "Enter first block content."))
                    .WithField("SecondBlockTitle", cfg => cfg.WithSetting("TextFieldSettings.Hint", "Enter second block title."))
                    .WithField("SecondBlockContent", cfg => cfg.WithSetting("TextFieldSettings.Hint", "Enter second block content."))
                    .WithField("ThirdBlockTitle", cfg => cfg.WithSetting("TextFieldSettings.Hint", "Enter third block title."))
                    .WithField("DocumentsLink", cfg => cfg.WithSetting("TextFieldSettings.Hint", "Enter a documents link."))
                    .WithField("SamplesLink", cfg => cfg.WithSetting("TextFieldSettings.Hint", "Enter a samples link."))
                    .WithField("MVALink", cfg => cfg.WithSetting("TextFieldSettings.Hint", "Enter an MVA link."))
                );


            if (settings != null)
            {
                settings.SearchedFields = new string[] { "title," +
                                                         "CodeSampleType,CodeSamplePlatform,CodeSampleLanguage,CodeSampleSource,CodeSampleProducts,CodeSampleServices," +
                                                         "codesample-subtext, codesample-externallink, codesample-location," +
                                                         "resource-subtext, resource-learnmoretext, resource-learnmoreurl," +
                                                         "TrainingType, training-subtext, training-externallink, training-location," +
                                                         "VideoType, videoitem-subtext, videoitem-embedcode, videoitem-location," +
                                                         "PodcastType, podcast-subtext, podcast-externallink, podcast-location," +
                                                         "EventType, event-subtext, event-externallink, event-location",
                                                         "solution-content, SolutionType, SolutionDevice, SolutionPrimaryWorkloadInvoked, SolutionIdentityIntegrationMethod" +
                                                         "patternsandpractices-subtext, patternsandpractices-githuburl, PatternsAndPracticesType",
                                                         "gettingstarted-intro, gettingstarted-firstblocktitle, gettingstarted-intro, gettingstarted-firstblockcontent",
                                                         "gettingstarted-secondblocktitle, gettingstarted-secondblockcontent, gettingstarted-thirdblocktitle, gettingstarted-thirdblockcontent," +
                                                         "gettingstarted-documentslink, gettingstarted-sampleslink, gettingstarted-mvalink" 
                };
            }
	


            _indexingService.UpdateIndex(IndexName);

            #endregion

            return 1;
        }   
    }
}