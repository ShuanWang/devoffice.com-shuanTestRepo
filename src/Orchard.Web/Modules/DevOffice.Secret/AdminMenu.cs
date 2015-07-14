using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Localization;
using Orchard.UI.Navigation;

namespace DevOffice.Secret
{
    public class AdminMenu: INavigationProvider
    {
        public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T("View"), "-1", menu =>
            {
                menu.LinkToFirstChild(false);
                menu.Add(T("Simple body with Image Pages"), "6", item => item.Action("List", "Admin", new { area = "Contents", id = "PartnerReqsPage" }));
                menu.Add(T("Rows with Tiles Pages"), "7", item => item.Action("List", "Admin", new { area = "Contents", id = "OverviewPage" }));
              
            });
        }
    }
}