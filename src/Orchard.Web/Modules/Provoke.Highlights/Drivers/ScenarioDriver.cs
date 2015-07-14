using System.Linq;
using Newtonsoft.Json;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;
using Orchard.UI.Notify;
using Provoke.Highlights.Models;
using Provoke.Highlights.Services;
using Provoke.Highlights.ViewModels;

namespace Provoke.Highlights.Drivers
{
    public class ScenarioDriver : ContentPartDriver<ScenarioPart>
    {
        private readonly IOrchardServices _orchardServices;
        private readonly IScenarioService _scenarioService;
        private const string TemplateName = "Parts/Scenario";
        public Localizer T { get; set; }

        public ScenarioDriver(IOrchardServices orchardServices, IScenarioService scenarioService) {
            _orchardServices = orchardServices;
            _scenarioService = scenarioService;
        }

        protected override DriverResult Editor(ScenarioPart part, dynamic shapeHelper) {
            var scenario = _scenarioService.GetScenario(part.Id) ?? new ScenarioRecord();

            var model = new ScenarioViewModel {
                  Title = scenario.Title
                , Description = scenario.Description
                , Tasks = scenario.Tasks
                , TasksJson = scenario.Tasks == null ? string.Empty : JsonConvert.SerializeObject(scenario.Tasks)
                , RelatedResources = scenario.RelatedResources.ToList()
                , RelatedResourcesJson = scenario.RelatedResources == null ? string.Empty : JsonConvert.SerializeObject(scenario.RelatedResources)
            };

            return ContentShape("Parts_Scenario_Edit",
                   () => shapeHelper.EditorTemplate(
                       TemplateName: TemplateName,
                       Model: model,
                       Prefix: Prefix
                    ));
        }

        protected override DriverResult Editor(ScenarioPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            if (!updater.TryUpdateModel(part, Prefix, null, null))
            {
                _orchardServices.Notifier.Error(T("Please enter all the required fields and submit again"));
            }
            
            var model = BuildEditorViewModel(part, part.TasksJson, part.ResourcesJson);

            if (part.ContentItem != null)
            {
                _scenarioService.UpdateScenarioPart(part.ContentItem, model);
            }

            return ContentShape("Parts_Items_Edit", () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: model, Prefix: Prefix));
        }

        private static ScenarioViewModel BuildEditorViewModel(ScenarioPart part, string tasksJson = null, string resourcesJson = null)
        {
            var scenarioViewModel = new ScenarioViewModel {
                  Title       = part.Title
                , Description = part.Description
                , Tasks       = part.Tasks
                , TasksJson   = tasksJson
                , RelatedResources = part.RelatedResources
                , RelatedResourcesJson = resourcesJson
            };

            return scenarioViewModel;
        }
    }
}