using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace DevOffice.Common.Models
{
    public class RelatedLinksRecord: ContentPartRecord
    {
        public RelatedLinksRecord()
        {
            Links = new List<RelatedLinkRecord>();
        }
        public virtual string RelatedLinksGroup { get; set; }
        public virtual string LinksJson { get; set; }
        public virtual IList<RelatedLinkRecord> Links { get; set; }
    }

    public class RelatedLinksPart : ContentPart<RelatedLinksRecord> {
        public string RelatedLinksGroup
        {
            get { return Record.RelatedLinksGroup; }
            set { Record.RelatedLinksGroup = value; }
        }

        public string LinksJson
        {
            get { return Record.LinksJson; }
            set { Record.LinksJson = value; }
        }

        public IList<RelatedLinkRecord> Links
        {
            get { return Record.Links; }
            set { Record.Links = value; }
        }
    }

}