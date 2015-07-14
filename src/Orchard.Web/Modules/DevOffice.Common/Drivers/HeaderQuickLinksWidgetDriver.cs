using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevOffice.Common.Models;
using DevOffice.Common.Services;
using DevOffice.Common.ViewModels;

using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace DevOffice.Common.Drivers
{
    public class HeaderQuickLinksDriver : ContentPartDriver<HeaderQuickLinksPart>
    {

        private readonly ICommonDataService _commonDataService;

        public HeaderQuickLinksDriver(ICommonDataService commonDataService)
        {
            _commonDataService = commonDataService;
        }

        protected override DriverResult Display(HeaderQuickLinksPart part, string displayType, dynamic shapeHelper)
        {
            QuickLinksViewModel model = new QuickLinksViewModel();
            model.QuickLinks = _commonDataService.GetQuickLinks();

            return ContentShape("Parts_HeaderQuickLinksWidget",
                () => shapeHelper.Partial(
                    TemplateName: "Parts/HeaderQuickLinksWidget",
                    Model: model));

        }
    }
}