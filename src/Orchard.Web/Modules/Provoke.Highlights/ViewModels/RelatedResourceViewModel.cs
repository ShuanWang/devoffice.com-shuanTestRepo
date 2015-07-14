using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevOffice.Common.Models;
using Provoke.Highlights.Models;

namespace Provoke.Highlights.ViewModels
{
    public class RelatedResourceViewModel
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Type { get; set; }
        public virtual string Url { get; set; }
        public virtual int SortOrder { get; set; }
        public virtual string RelatedResourceJson { get; set; }
        public virtual int ScenarioId { get; set; }
    }
}