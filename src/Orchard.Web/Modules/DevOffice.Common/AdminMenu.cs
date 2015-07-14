using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Localization;
using Orchard.UI.Navigation;

namespace DevOffice.Common
{
    public class AdminMenu : INavigationProvider
    {
        public Localizer T { get; set; }
        public string MenuName { get { return "admin";  } }

        public void GetNavigation(NavigationBuilder builder) {
            builder.Add(T("View"), "-1", menu => {
                menu.LinkToFirstChild(false);

                menu.Add(T("Pages"), "5", item => item.Action("List", "Admin", new { area="Contents", id="Page" }));
                menu.Add(T("Step Pages"), "5", item => item.Action("List", "Admin", new { area = "Contents", id = "StepPage" }));
                menu.Add(T("Community Items"), "5", item => item.Action("List", "Admin", new { area = "Contents", id = "Community" }));
                menu.Add(T("Events"), "5", item => item.Action("List", "Admin", new { area = "Contents", id = "Event" }));
                menu.Add(T("Quick Link Items"), "5", item => item.Action("List", "Admin", new { area = "Contents", id = "QuickLinks" }));
                menu.Add(T("Training Items"), "5", item => item.Action("List", "Admin", new { area = "Contents", id = "Training" }));
                menu.Add(T("Videos"), "5", item => item.Action("List", "Admin", new { area = "Contents", id = "VideoItem" }));
                menu.Add(T("Code Samples"), "5", item => item.Action("List", "Admin", new { area = "Contents", id = "CodeSample" }));
                menu.Add(T("Podcasts"), "5", item => item.Action("List", "Admin", new { area = "Contents", id = "Podcast" }));
                menu.Add(T("Resources"), "5", item => item.Action("List", "Admin", new { area = "Contents", id = "Resource" }));
                menu.Add(T("Solutions"), "5", item => item.Action("List", "Admin", new { area = "Contents", id = "Solution" }));
                menu.Add(T("Patterns And Practices"), "5", item => item.Action("List", "Admin", new { area = "Contents", id = "PatternsAndPractices" }));
                menu.Add(T("Getting Started Tabs"), "5", item => item.Action("List", "Admin", new { area = "Contents", id = "GettingStarted" }));
                menu.Add(T("Sections with Tiles Pages"), "5", item => item.Action("List", "Admin", new { area = "Contents", id = "SectionsWithTilesPage" }));
                menu.Add(T("Tiles"), "5", item => item.Action("List", "Admin", new { area = "Contents", id = "Tile" }));
                menu.Add(T("Articles"), "5", item => item.Action("List", "Admin", new { area = "Contents", id = "Article" }));
                menu.Add(T("Highlights"), "5", item => item.Action("List", "Admin", new { area = "Contents", id = "Highlight" }));
            });
        }
    }
}