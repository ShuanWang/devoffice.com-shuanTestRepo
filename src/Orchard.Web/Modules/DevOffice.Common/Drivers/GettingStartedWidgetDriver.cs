using System.Collections.Generic;
using System.Linq;
using DevOffice.Common.Models;
using DevOffice.Common.Services;
using DevOffice.Common.ViewModels;
using Orchard.ContentManagement.Drivers;
using Orchard.Taxonomies.Services;

namespace DevOffice.Common.Drivers {
    public class GettingStartedWidgetDriver : ContentPartDriver<GettingStartedWidgetPart> {
        private readonly ICommonDataService _commonDataService;
        private readonly ITaxonomyService _taxonomyService;

        public GettingStartedWidgetDriver(ICommonDataService commonDataService, ITaxonomyService taxonomyService) {
            _commonDataService = commonDataService;
            _taxonomyService = taxonomyService;
        }

        protected override DriverResult Display(GettingStartedWidgetPart part, string displayType, dynamic shapeHelper) {

            return ContentShape("Parts_GettingStartedWidget",
                () => {
                    var shape = shapeHelper.Parts_GettingStartedWidget();
                    shape.ContentPart = part;
                    shape.ViewModel = new GettingStartedViewModel {
                        Tabs = _commonDataService.GetGettingStartedTabs()
                    };
                    return shape;
                });



        }
    }
}