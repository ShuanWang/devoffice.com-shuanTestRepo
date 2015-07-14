using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevOffice.Common.Models;

namespace DevOffice.Common.ViewModels
{
    public class RelatedLinksViewModel
    {
         public string RelatedLinksGroup { get; set; }

         public RelatedLinksViewModel()
         {
             Links = new List<RelatedLinkRecord>();
        }

         public IList<RelatedLinkRecord> Links { get; set; }
         public string LinksJson { get; set; }
    }
}