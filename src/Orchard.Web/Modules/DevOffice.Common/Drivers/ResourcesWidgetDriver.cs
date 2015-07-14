using DevOffice.Common.Models;
using DevOffice.Common.Services;
using DevOffice.Common.ViewModels;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using System.Collections.Generic;
using System.Linq;
using Orchard.Taxonomies.Services;

namespace DevOffice.Common.Drivers
{
    public class ResourcesWidgetDriver : ContentPartDriver<ResourcesWidgetPart> {

        private readonly ICommonDataService _commonDataService;
        private readonly ITaxonomyService _taxonomyService;
        public ResourcesWidgetDriver(ICommonDataService commonDataService, ITaxonomyService taxonomyService)
        {
            _commonDataService = commonDataService;
            _taxonomyService = taxonomyService;
        }

        protected override DriverResult Display(ResourcesWidgetPart part, string displayType, dynamic shapeHelper) {
            var model = new ResourcesViewModel() {
                Resources = _commonDataService.GetResources()
            };
            return ContentShape("Parts_ResourcesWidget",
                () => shapeHelper.Partial(
                    TemplateName: "Parts/ResourcesWidget",
                    Model: model
                    ));

        }
    }
}