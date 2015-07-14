using DevOffice.Common.Models;
using DevOffice.Common.Services;
using DevOffice.Common.ViewModels;
using Orchard;
using Orchard.ContentManagement.Drivers;
using System.Collections.Generic;
using System.Linq;
using Orchard.Taxonomies.Services;
using System.Text.RegularExpressions;
using System.Web;

namespace DevOffice.Common.Drivers
{
    public class SingleBlogWidgetDriver : ContentPartDriver<SingleBlogWidgetPart>
    {
        private readonly ICommonDataService _commonDataService;
        private readonly IOrchardServices _services;

        public SingleBlogWidgetDriver(ICommonDataService commonDataService, IOrchardServices services)
        {
           
          
            _commonDataService = commonDataService;
            _services = services;
        }

        protected override DriverResult Display(SingleBlogWidgetPart part, string displayType, dynamic shapeHelper)
        {
            BlogPost singleItem = null;
            if (HttpContext.Current.Request.QueryString["id"] != null)
            {
                try
                {
                    int id = int.Parse(HttpContext.Current.Request.QueryString["id"]);
                    singleItem = _commonDataService.GetArticleById(id);
                }
                catch
                {
                    singleItem = null;
                }
            }
           
           
            return ContentShape("Parts_SingleBlogWidget",
                () =>
                {
                    var shape = shapeHelper.Parts_SingleBlogWidget();
                    shape.ContentPart = part;
                    shape.ViewModel = singleItem;

                    return shape;
                });

        }
    }
}