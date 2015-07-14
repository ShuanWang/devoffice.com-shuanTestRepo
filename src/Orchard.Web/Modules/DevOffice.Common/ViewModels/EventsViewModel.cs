using System;
using System.Collections.Generic;

namespace DevOffice.Common.ViewModels
{
    public class EventsViewModel {
        public List<TaxonomyEvents> TaxonomyEvents { get; set; }
    }

    public class TaxonomyEvents {
        public List<Event> Events { get; set; }
        public string Title { get; set; }
    }

    public class Event {
        public string Title { get; set; }
        public string SubText { get; set; }
        public string Location { get; set; }
        public List<int> EventType { get; set; }
        public string Month { get; set; }
        public string Date { get; set; }
        public string Year { get; set; }
        public DateTime FullStartDate { get; set; }
        public string ExternalLink { get; set; }
    }
}