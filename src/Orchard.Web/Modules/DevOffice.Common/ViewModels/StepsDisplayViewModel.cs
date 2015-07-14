using System.Collections.Generic;
using DevOffice.Common.Models;

namespace DevOffice.Common.ViewModels
{
    public class StepsDisplayViewModel
    {
        public List<StepInformationRecord> Steps { get; set; }
        public string StepsPage { get; set; }
    }
}