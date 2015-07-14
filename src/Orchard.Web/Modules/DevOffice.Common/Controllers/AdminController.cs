using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard.Widgets.Services;

namespace DevOffice.Common.Controllers
{
    public class AdminController: Controller
    {
        private readonly IWidgetsService _widgetService;

        public AdminController(IWidgetsService widgetService) {
            _widgetService = widgetService;
        }

        //Redirect to Widget Edit page from Resources Top Download in Admin Menu
        public ActionResult RedirectToFooterWidgetAdminUrl()
        {
            var widget = _widgetService.GetWidgets().FirstOrDefault(l => l.Title == "Footer");
            if (widget != null)
            {
                Response.Redirect("/Admin/Widgets/EditWidget/" + widget.Id);
            }
            return null;
        }
    }
}