using System.Collections.Generic;
using DevOffice.Common.Models;
using Orchard;
using Orchard.ContentManagement;

namespace DevOffice.Common.Services
{
    public interface IStepDataService : IDependency {
        List<StepInformationRecord> GetStepsForContentItem(ContentItem item);
        void UpdateStepsForContentItem(ContentItem item, IEnumerable<StepInformationRecord> steps);
        void UpdateSubstepForStep(StepInformationRecord step);
    }
}