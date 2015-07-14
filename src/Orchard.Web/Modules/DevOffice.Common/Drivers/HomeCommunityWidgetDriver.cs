using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevOffice.Common.Models;
using DevOffice.Common.Services;
using DevOffice.Common.ViewModels;
using Orchard.ContentManagement.Drivers;

namespace DevOffice.Common.Drivers
{
    public class HomeCommunityWidgetDriver : ContentPartDriver<HomeCommunityWidgetPart>
    {
        private readonly ICommonDataService _commonDataService;

        public HomeCommunityWidgetDriver(ICommonDataService commonDataService)
        {
            _commonDataService = commonDataService;
        }

        protected override DriverResult Display(HomeCommunityWidgetPart part, string displayType, dynamic shapeHelper)
        {
            var model = new CommunityViewModel();
            model.CommunityItems = _commonDataService.GetCommunityItems();

            return ContentShape("Parts_HomeCommunityWidget",
                () => shapeHelper.Partial(
                    TemplateName: "Parts/HomeCommunityWidget",
                    Model: model));

        }
    }
}