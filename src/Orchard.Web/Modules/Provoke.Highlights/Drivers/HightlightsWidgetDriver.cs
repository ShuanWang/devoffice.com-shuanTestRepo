using System;
using System.Collections.Generic;
using DevOffice.Common.Drivers;
using Newtonsoft.Json;
using Orchard.ContentManagement.Drivers;
using Provoke.Highlights.Models.Widgets;
using Provoke.Highlights.Services;
using Provoke.Highlights.ViewModels;
using DevOffice.Common.ViewModels;

namespace Provoke.Highlights.Drivers
{
    public class HightlightsWidgetDriver : ContentPartDriver<HighlightsWidgetPart>
    {
        private readonly IScenarioService _scenarioService;

        public HightlightsWidgetDriver(IScenarioService scenarioService)
        {
            _scenarioService = scenarioService;
        }

        protected override DriverResult Display(HighlightsWidgetPart part, string displayType, dynamic shapeHelper) {

            dynamic partD = part;
            
            dynamic highlightWidgets = partD.HighlightPicker.ContentItems;
            //var highlightShapes = new DriverResult[highlightWidgets.Length];
            var viewModels = new List<ScenarioViewModel>();
            //var resources = new RelatedLinksViewModel[highlightWidgets.Length];

            for(var i = 0; i < highlightWidgets.Length; i++) {
                var highlight = highlightWidgets[i];
                var scenario = _scenarioService.GetScenario(highlight.Id);

                viewModels.Add( new ScenarioViewModel {
                    Title = scenario.Title,
                    Description = scenario.Description,
                    Tasks = scenario.Tasks,
                    TasksJson = scenario.Tasks == null ? string.Empty : JsonConvert.SerializeObject(scenario.Tasks),
                    RelatedResources = scenario.RelatedResources,
                    RelatedResourcesJson = scenario.RelatedResources == null? string.Empty : JsonConvert.SerializeObject(scenario.RelatedResources)
                });
                
            }
            var highlightModel = new HighlightsViewModel() {
                Scenarios = viewModels,
                PageIntro = partD.CommonHighlightPart.PageIntro.Value,
                LabIntro = partD.CommonHighlightPart.LabIntro.Value,
                RelatedResourcesIntro = partD.CommonHighlightPart.RelatedResourcesIntro.Value,
                Title = partD.WidgetPart.Title
            };

            return ContentShape("Parts_HighlightsWidget",
                        () => shapeHelper.Partial(
                            TemplateName: "Parts/HighlightsWidget",
                            Model: highlightModel
                        )); 
                //Combined(highlightShapes);

        }
    }
}