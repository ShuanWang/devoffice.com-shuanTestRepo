using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevOffice.Secret.Models;

namespace DevOffice.Secret.ViewModels
{
    public class RowWithTilesViewModel
    {
        public RowWithTilesViewModel()
        {
            Rows = new List<SingleRowWithTilesRecord>();
        }

        public IList<SingleRowWithTilesRecord> Rows { get; set; }
        public string TilesJson { get; set; }
    }

}