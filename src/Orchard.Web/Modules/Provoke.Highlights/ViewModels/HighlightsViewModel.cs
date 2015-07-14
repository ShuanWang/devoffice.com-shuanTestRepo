using System.Collections.Generic;
using Provoke.Highlights.Models;

namespace Provoke.Highlights.ViewModels
{
    public class HighlightsViewModel
    {
        public List<ScenarioViewModel> Scenarios { get; set; }
        public string RelatedResourcesIntro { get; set; }
        public string LabIntro { get; set; }
        public string PageIntro { get; set; }
        public string Title { get; set; }
    }
}