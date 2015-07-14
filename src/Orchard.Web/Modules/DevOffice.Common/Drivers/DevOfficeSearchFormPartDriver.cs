using DevOffice.Common.Models;
using DevOffice.Common.ViewModels;
using Orchard.ContentManagement.Drivers;

namespace DevOffice.Common.Drivers
{
    public class DevOfficeSearchFormPartDriver : ContentPartDriver<DevOfficeSearchFormPart>
    {

        protected override DriverResult Display(DevOfficeSearchFormPart part, string displayType, dynamic shapeHelper)
        {
            var model = new SearchViewModel();
            return ContentShape("Parts_DevOfficeSearchForm",
                                () =>
                                {
                                    var shape = shapeHelper.Parts_DevOfficeSearchForm();
                                    shape.ContentPart = part;
                                    shape.ViewModel = model;
                                    return shape;
                                });
        }
    }
}