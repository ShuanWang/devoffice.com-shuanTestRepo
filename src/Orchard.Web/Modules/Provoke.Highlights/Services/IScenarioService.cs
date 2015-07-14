using Orchard;
using Orchard.ContentManagement;
using Provoke.Highlights.Models;
using Provoke.Highlights.ViewModels;

namespace Provoke.Highlights.Services {
    public interface IScenarioService : IDependency {
        void UpdateScenarioPart(ContentItem item, ScenarioViewModel model);
        ScenarioRecord GetScenario(int scenarioId);
    }
}