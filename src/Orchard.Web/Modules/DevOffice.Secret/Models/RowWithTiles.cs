using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace DevOffice.Secret.Models
{
    public class RowWithTilesRecord : ContentPartRecord
    {
        public virtual string TilesJson { get; set; }
    }

    public class RowWithTilesPart : ContentPart<RowWithTilesPart>
    {

        public string TilesJson
        {
            get { return Record.TilesJson; }
            set { Record.TilesJson = value; }
        }
    }
}