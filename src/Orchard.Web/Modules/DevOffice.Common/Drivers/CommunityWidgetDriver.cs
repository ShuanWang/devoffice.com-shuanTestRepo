using DevOffice.Common.Models;
using DevOffice.Common.Services;
using DevOffice.Common.ViewModels;
using Orchard.ContentManagement.Drivers;

namespace DevOffice.Common.Drivers
{
    public class CommunityWidgetDriver : ContentPartDriver<CommunityWidgetPart>
    {
        private readonly ICommonDataService _commonDataService;

        public CommunityWidgetDriver(ICommonDataService commonDataService)
        {
            _commonDataService = commonDataService;
        }

        protected override DriverResult Display(CommunityWidgetPart part, string displayType, dynamic shapeHelper)
        {
            var model = new CommunityViewModel();
            model.CommunityItems = _commonDataService.GetCommunityItems();

            return ContentShape("Parts_CommunityWidget",
                () => shapeHelper.Partial(
                    TemplateName: "Parts/CommunityWidget",
                    Model: model));

        }
    }
}