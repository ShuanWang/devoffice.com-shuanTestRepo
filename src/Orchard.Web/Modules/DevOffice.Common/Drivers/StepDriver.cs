using System;
using DevOffice.Common.Models;
using DevOffice.Common.Services;
using DevOffice.Common.ViewModels;
using Orchard.ContentManagement.Drivers;

namespace DevOffice.Common.Drivers
{
    public class StepDriver : ContentPartDriver<StepPart> {

        private readonly IStepDataService _stepService;

        public StepDriver(IStepDataService stepService) 
        {
            _stepService = stepService;
        }

        protected override DriverResult Display(StepPart part, string displayType, dynamic shapeHelper) {
            dynamic item = part.ContentItem;

            var model = new StepsDisplayViewModel {Steps = _stepService.GetStepsForContentItem(part.ContentItem), StepsPage = item.AutoroutePart.Path };

            return ContentShape("Parts_Steps",
                () => shapeHelper.Partial(
                    TemplateName: "Parts/Steps",
                    Model: model));
        }

        //GET
        protected override DriverResult Editor(StepPart part, dynamic shapeHelper) {
            var model = new StepsEditorViewModel {Steps = _stepService.GetStepsForContentItem(part.ContentItem)};

            return ContentShape("Parts_Steps_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/Steps",
                    Model: model,
                    Prefix: Prefix
                    ));
        }

        //POST
        protected override DriverResult Editor(StepPart part, Orchard.ContentManagement.IUpdateModel updater, dynamic shapeHelper) {
            var model = new StepsEditorViewModel();
            try {
                if (updater.TryUpdateModel(model, Prefix, null, null)) 
                {
                    if (part.ContentItem.Id != 0 && model.Steps != null) 
                    {
                        _stepService.UpdateStepsForContentItem(part.ContentItem, model.Steps);    
                    }
                }
            }
            catch (Exception e) 
            {
                //todo: should we catch and log?    
            }

            return Editor(part, shapeHelper);
        }

        protected override string Prefix
        {
            get
            {
                return "Steps";
            }
        }
    }
}