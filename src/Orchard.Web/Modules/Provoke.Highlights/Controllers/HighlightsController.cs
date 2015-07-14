using System.Web.Http;
using Orchard.Data;
using Provoke.Highlights.Models;
using Provoke.Highlights.Services;

namespace Provoke.Highlights.Controllers
{
    public class HighlightsController : ApiController
    {
        private readonly IRepository<ScenarioRecord> _scenarioRepository;
        private readonly IRepository<TaskRecord>     _taskRepository;
        private readonly IRepository<StepRecord>     _stepRepository;
        private readonly IScenarioService            _scenarioService;

        public HighlightsController(
            IRepository<ScenarioRecord> scenarioRepository,
            IRepository<TaskRecord>     taskRepository,
            IRepository<StepRecord>     stepRepository,
            IScenarioService            scenarioService
        )
        {
            _scenarioRepository = scenarioRepository;
            _taskRepository     = taskRepository;
            _stepRepository     = stepRepository;
            _scenarioService    = scenarioService;
        }

        [HttpPost]
        // http://officedeploycenter/api/Provoke.Highlights/Highlights/AddTestData
        public string AddTestData()
        {
            var highlightsScenario = new ScenarioRecord
            {
                Title = "Highlights",
                Description = "Setting up your own domain",
            };
            _scenarioRepository.Create(highlightsScenario);
            _scenarioRepository.Flush();

            var addYourDomainTask = new TaskRecord
            {
                ScenarioRecord = highlightsScenario,
                SortOrder = 1,
                Description = "Add your domain",
            };
            _taskRepository.Create(addYourDomainTask);

            var verifyYourDomainTask = new TaskRecord
            {
                ScenarioRecord = highlightsScenario,
                SortOrder = 2,
                Description = "Verify your domain",
            };
            _taskRepository.Create(verifyYourDomainTask);

            var defineDomainUseTask = new TaskRecord
            {
                ScenarioRecord = highlightsScenario,
                SortOrder = 3,
                Description = "Define domain use",
            };
            _taskRepository.Create(defineDomainUseTask);

            var updateDnsTask = new TaskRecord
            {
                ScenarioRecord = highlightsScenario,
                SortOrder = 4,
                Description = "Update DNS records",
            };
            _taskRepository.Create(updateDnsTask);
            _taskRepository.Flush();

            var addDomainStep1 = new StepRecord
            {
                TaskRecord = addYourDomainTask,
                SortOrder = 1,
                TopPosition = 200,
                LeftPosition =300,
                Title = "White",
                Description = "In the Office 365 admin center, click 'Domains'.",
                Image = "/Media/Default/images/gettingStarted.png",
            };
            _stepRepository.Create(addDomainStep1);

            var addDomainStep2 = new StepRecord
            {
                TaskRecord = addYourDomainTask,
                SortOrder = 2,
                TopPosition = 300,
                LeftPosition = 300,
                Title = "Yellow",
                Description = "Now click 'Add domain'.",
                Image = "/Media/Default/images/onboarding.png",
            };
            _stepRepository.Create(addDomainStep2);

            var verifyYourDomainStep = new StepRecord
            {
                TaskRecord = verifyYourDomainTask,
                SortOrder = 1,
                TopPosition = 300,
                LeftPosition = 200,
                Title = "Red",
                Description = "Define how you'll use your domain (this is, stat the purpose for the domain) by selecting the Office 365 services you're planning to use: Exchange Online, Lync Online, SharePoint Online, or a combination.",
                Image = "/Media/Default/images/email.png",
            };
            _stepRepository.Create(verifyYourDomainStep);

            var defineDomainUseStep1 = new StepRecord
            {
                TaskRecord = defineDomainUseTask,
                SortOrder = 1,
                TopPosition = 400,
                LeftPosition = 300,
                Title = "Blue",
                Description = "Define how you'll use your domain (this is, stat the purpose for the domain) by selecting the Office 365 services you're planning to use: Exchange Online, Lync Online, SharePoint Online, or a combination.",
                Image = "/Media/Default/images/assistance-1.png",
                Anchor = "http://anchor.com",
            };
            _stepRepository.Create(defineDomainUseStep1);

            var updateDnsRecordsStep = new StepRecord
            {
                TaskRecord = updateDnsTask,
                SortOrder = 1,
                TopPosition = 300,
                LeftPosition = 400,
                Title = "Green",
                Description = "Update your DNS records at your hosting provider or domain registrar to point to these services in Office 365.",
                Image = "/Media/Default/images/management.png",
            };
            _stepRepository.Create(updateDnsRecordsStep);

            return "Success";
        }

        [HttpGet]
        // http://officedeploycenter/api/Provoke.Highlights/Highlights/GetScenario
        public ScenarioRecord GetScenario(int scenarioId) {
            return _scenarioService.GetScenario(scenarioId);
        }
    }
}