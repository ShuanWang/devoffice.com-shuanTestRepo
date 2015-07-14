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
    public class SectionsWithTilesPageDriver : ContentPartDriver<SectionsWithTilesPagePart>
    {

        private readonly ICommonDataService _commonDataService;
        public SectionsWithTilesPageDriver(ICommonDataService commonDataService) {
            _commonDataService = commonDataService;
        }

        protected override DriverResult Display(SectionsWithTilesPagePart part, string displayType, dynamic shapeHelper) {
            var model = _commonDataService.GetSectionsWithTilesPage(part.ContentItem);
            if (model.TilePageStyle == "Columns") {
                return ContentShape("Parts_TilesColumnsPage",
                    () => shapeHelper.Parts_TilesColumnsPage(ViewModel: model));
            }
            else {
                return ContentShape("Parts_TilesRowsPage",
                    () => shapeHelper.Parts_TilesRowsPage(ViewModel: model));
            }
        }
    }
}