using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevOffice.Common.ViewModels {
    public class SolutionViewModel {
        public List<string> Types { get; set; }
        public List<string> Devices { get; set; }
        public List<string> PrimaryWorkloadInvoked { get; set; }
        public List<string> IdentityIntegrationMethod { get; set; }


        public List<string> SelectedTypes { get; set; }
        public List<string> SelectedDevices { get; set; }
        public List<string> SelectedPrimaryWorkloadInvoked { get; set; }
        public List<string> SelectedIdentityIntegrationMethod { get; set; }

        public List<TaxonomySolution> TaxonomySolutions { get; set; }
        public string HtmlData { get; set; }
    }

    public class TaxonomySolution {
        public List<Solution> Solutions { get; set; }
        public string Title { get; set; }        

    }

    public class Solution {
        public List<int> SolutionType { get; set; }
        public List<int> SolutionDevice { get; set; }
        public List<int> SolutionPrimaryWorkloadInvolved { get; set; }
        public List<int> SolutionIdentityIntegrationMethod { get; set; }
        public List<string> FilterTerms { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Ordering { get; set; }
        public string Image { get; set; }
        public string ExternalLink { get; set; }
        public int Column { get; set; }
    }
}