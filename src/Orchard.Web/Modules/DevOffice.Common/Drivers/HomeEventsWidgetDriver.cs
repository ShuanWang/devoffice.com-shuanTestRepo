using System;
using DevOffice.Common.Models;
using DevOffice.Common.Services;
using DevOffice.Common.ViewModels;
using Orchard.ContentManagement.Drivers;
using System.Collections.Generic;
using System.Linq;
using Orchard.Taxonomies.Services;

namespace DevOffice.Common.Drivers
{
    public class HomeEventsWidgetDriver: ContentPartDriver<HomeEventsPart> {
        private readonly ICommonDataService _commonDataService;
        private readonly ITaxonomyService _taxonomyService;

        public HomeEventsWidgetDriver(ICommonDataService commonDataService, ITaxonomyService taxonomyService)
        {
            _commonDataService = commonDataService;
            _taxonomyService = taxonomyService;
        }

        protected override DriverResult Display(HomeEventsPart part, string displayType, dynamic shapeHelper) {
            var events = _commonDataService.GetEvents();
            
            var terms = _taxonomyService.GetTerms(_taxonomyService.GetTaxonomyByName("Event Type").Id).OrderBy(x => x.Weight).Take(3);

            var model = new EventsViewModel();
            model.TaxonomyEvents = new List<TaxonomyEvents>();
            foreach (var term in terms)
            {
                model.TaxonomyEvents.Add(new TaxonomyEvents
                {
                    Title = term.Name,
                    Events = events.Where(x => x.EventType.Contains(term.Weight) && (x.FullStartDate >= DateTime.Today)).Take(4).ToList()
                });
            }

            return ContentShape("Parts_HomeEventsWidget",
                () => {
                    var shape = shapeHelper.Parts_HomeEventsWidget();
                    shape.ContentPart = part;
                    shape.ViewModel = model;
                
                    return shape;
            });

        }
    }
}