using System;
using System.Collections.Generic;
using System.Data;
using Orchard;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Widgets.Services;
using Devoffice.GettingStarted.Models;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Autoroute.Models;
using Orchard.Autoroute.Services;
using Orchard.Core.Title.Models;
using Orchard.Core.Common.Models;
using Orchard.Core.Navigation.Services;



namespace Devoffice.GettingStarted {
    [OrchardFeature("DevOffice.GettingStarted")]

    public class Migrations : DataMigrationImpl
    {
        private readonly IWidgetsService _widgetsService;
        private readonly IContentManager _contentManager;
        private readonly IAutorouteService _autorouteService;
        private readonly IMenuService _menuService;
        public Migrations(IContentManager contentManager,
                          IWidgetsService widgetsService)
        {
            _contentManager = contentManager;
            _widgetsService = widgetsService;
        }


        public int Create()
        {

            #region Create AddinsWidget
            var addinsLayer = _widgetsService.CreateLayer("Getting Started Add-ins", "The widgets in this layer are displayed on the Getting Started Add-ins Pages", "url '~/GettingStarted/Addins'");
            ContentDefinitionManager.AlterPartDefinition(
                typeof(AddinsWidgetPart).Name, cfg => cfg.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("AddinsWidget",
                cfg => cfg
                    .WithPart("WidgetPart")
                    .WithPart("AddinsWidgetPart")
                    .WithPart("CommonPart")
                    .WithSetting("Stereotype", "Widget")
                );

            var addinsWidget = _widgetsService.CreateWidget(addinsLayer.Id, "AddinsWidget", "Getting Started Add-ins Widget", "1", "Content");
            addinsWidget.RenderTitle = false;
            addinsWidget.Name = "AddinsWidget";
            _contentManager.Publish(addinsWidget.ContentItem);
            #endregion

            #region Create Office 365 APIs Widget
            var apiLayer = _widgetsService.CreateLayer("Getting Started APIs", "The widgets in this layer are displayed on the Getting Started O365 API Pages", "url '~/GettingStarted/Api'");
            ContentDefinitionManager.AlterPartDefinition(
                typeof(ApiWidgetPart).Name, cfg => cfg.Attachable());

            ContentDefinitionManager.AlterTypeDefinition("ApiWidget",
                cfg => cfg
                    .WithPart("WidgetPart")
                    .WithPart("ApiWidgetPart")
                    .WithPart("CommonPart")
                    .WithSetting("Stereotype", "Widget")
                );

            var apiWidget = _widgetsService.CreateWidget(apiLayer.Id, "ApiWidget", "Getting Started O365 API Widget", "1", "Content");
            apiWidget.RenderTitle = false;
            apiWidget.Name = "ApiWidget";
            _contentManager.Publish(apiWidget.ContentItem);
            #endregion

            return 1;
        }

        //public int UpdateFrom1()
        //{
        //    #region Create new pages - addins, apis

        //    //Create Addins Page
        //    var pageAddins = _contentManager.Create("Page");
`        //    pageAddins.As<TitlePart>().Title = "Addins";
        //    pageAddins.As<BodyPart>().Text = String.Empty;

        //    var pageAddinsArp = pageAddins.As<AutoroutePart>();
        //    pageAddinsArp.DisplayAlias = "getting-started/addins";
        //    _autorouteService.GenerateAlias(pageAddinsArp);
        //    _autorouteService.PublishAlias(pageAddinsArp);

        //    //Create Api Page
        //    var pageApi = _contentManager.Create("Page");
        //    pageApi.As<TitlePart>().Title = "Api";
        //    pageApi.As<BodyPart>().Text = String.Empty;

        //    var pageApiArp = pageApi.As<AutoroutePart>();
        //    pageApiArp.DisplayAlias = "getting-started/api";
        //    _autorouteService.GenerateAlias(pageApiArp);
        //    _autorouteService.PublishAlias(pageApiArp);

        //    #endregion
        //    //update layer url
        //    var addinsLayer = _widgetsService.GetLayers();
        //    addinsLayer.LayerRule = "~\getting-started\addins";
        //    ContentDefinitionManager.AlterPartDefinition(
        //        typeof(AddinsWidgetPart).


        //    return 2;
        //}
    }
}