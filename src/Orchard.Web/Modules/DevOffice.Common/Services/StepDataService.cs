using System;
using System.Collections.Generic;
using System.Linq;
using DevOffice.Common.Models;
using Orchard.ContentManagement;
using Orchard.Data;

namespace DevOffice.Common.Services
{
    public class StepDataService : IStepDataService {
        private readonly IRepository<StepInformationRecord> _stepInformationRepository;
        private readonly IRepository<SubstepInformationRecord> _substepInformationRepository;

        public StepDataService(
            IRepository<StepInformationRecord> stepInformationRepository,
            IRepository<SubstepInformationRecord> substepInformationRepository) 
        {
            _stepInformationRepository = stepInformationRepository;
            _substepInformationRepository = substepInformationRepository;
        }

        public List<StepInformationRecord> GetStepsForContentItem(ContentItem item) {
            var stepPart = item.As<StepPart>();
            if (stepPart != null) 
            {
                var steps = _stepInformationRepository.Fetch(s => s.StepPartId == stepPart.Id && !s.IsDeleted).OrderBy(s => s.Position).ToList();

                foreach (var step in steps) {
                    step.Substeps = _substepInformationRepository.Fetch(s => s.StepInformationRecord == step && !s.IsDeleted).OrderBy(s => s.Position).ToList();
                }

                return steps;
            }

            return null;
        }

        public void UpdateStepsForContentItem(ContentItem item, IEnumerable<StepInformationRecord> steps) 
        {
            var record = item.As<StepPart>().Record;

            var oldSteps = steps.Where(s => s.Id != -1);
            var newSteps = steps.Where(s => s.Id == -1);

            foreach (var oldStep in oldSteps) 
            {
                oldStep.StepPartId = record.Id;
                UpdateSubstepForStep(oldStep);
                _stepInformationRepository.Update(oldStep);
            }

            foreach (var newStep in newSteps) 
            {
                newStep.StepPartId = record.Id;
                newStep.CreateDate = DateTime.UtcNow;
                _stepInformationRepository.Create(newStep);
                UpdateSubstepForStep(newStep);
            }
        }

        public void UpdateSubstepForStep(StepInformationRecord step) 
        {
            if (step.Substeps != null) {
                var oldSubsteps = step.Substeps.Where(s => s.Id != -1);
                var newSubsteps = step.Substeps.Where(s => s.Id == -1);

                foreach (var oldSubstep in oldSubsteps) 
                {
                    oldSubstep.StepInformationRecord = step;
                    _substepInformationRepository.Update(oldSubstep);
                }

                foreach (var newSubstep in newSubsteps) 
                {
                    newSubstep.StepInformationRecord = step;
                    newSubstep.CreateDate = DateTime.UtcNow;
                    _substepInformationRepository.Create(newSubstep);
                }
            }
        }
    }
}