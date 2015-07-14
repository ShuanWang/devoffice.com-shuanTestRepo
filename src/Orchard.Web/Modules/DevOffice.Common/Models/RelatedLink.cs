using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevOffice.Common.Models
{
    public class RelatedLinkRecord
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Type { get; set; }
        public virtual string Url { get; set; }
        public virtual int SortOrder { get; set; }
        public virtual RelatedLinksRecord RelatedLinksRecord { get; set; }
    }
}