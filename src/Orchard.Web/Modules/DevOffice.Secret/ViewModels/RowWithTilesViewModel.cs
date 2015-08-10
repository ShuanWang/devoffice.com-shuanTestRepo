using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevOffice.Secret.Models;

namespace DevOffice.Secret.ViewModels
{
    public class RowWithTilesViewModel
    {
        //public string RelatedLinksGroup { get; set; }



         public IList<RowWithTilesRecord> Rows { get; set; }
         public string LinksJson { get; set; }
    }

    
}