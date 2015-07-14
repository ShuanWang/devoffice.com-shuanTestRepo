using System;
using DevOffice.Common.Models;
using DevOffice.Common.Services;
using DevOffice.Common.ViewModels;
using Orchard.ContentManagement.Drivers;
using System.Collections.Generic;
using System.Linq;
using Orchard.Taxonomies.Models;
using Orchard.Taxonomies.Services;

namespace DevOffice.Common.Drivers
{
    public class EventsWidgetDriver: ContentPartDriver<EventsPart> {
        private readonly ICommonDataService _commonDataService;
        private readonly ITaxonomyService _taxonomyService;

        public EventsWidgetDriver(ICommonDataService commonDataService, ITaxonomyService taxonomyService) {
            _commonDataService = commonDataService;
            _taxonomyService = taxonomyService;
        }

        protected override DriverResult Display(EventsPart part, string displayType, dynamic shapeHelper) {
            List<Event> events = _commonDataService.GetEvents();
            var terms = _taxonomyService.GetTerms(_taxonomyService.GetTaxonomyByName("Event Type").Id).OrderBy(x => x.Weight);
            
            EventsViewModel model = new EventsViewModel();
            model.TaxonomyEvents = new List<TaxonomyEvents>();

            if (terms.Any()) {
                foreach (var term in terms) {
                    model.TaxonomyEvents.Add(new TaxonomyEvents {
                        Title = term.Name,
                        Events = events.Where(x => x.EventType.Any() && x.EventType.Contains(term.Weight) && (x.FullStartDate >= DateTime.Today || term.Weight > 2)).ToList()
                    });
                }
            }
            else {
                model.TaxonomyEvents.Add(new TaxonomyEvents
                {
                    Title = "Events",
                    Events = events
                });
            }

            return ContentShape("Parts_EventsWidget",
                () => shapeHelper.Partial(
                    TemplateName: "Parts/EventsWidget",
                    Model: model
                    ));

        }
    }
}