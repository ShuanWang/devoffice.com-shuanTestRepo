using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using Remotion.Linq.Clauses.ResultOperators;

namespace DevOffice.Secret.Models
{
    public class RowWithTilesRecord : ContentPartRecord
    {
          public RowWithTilesRecord()
        {
            Rows = new List<SingleRowWithTilesRecord>();
        }
        public virtual string TilesJson { get; set; }
        public virtual IList<SingleRowWithTilesRecord> Rows { get; set; }
       
    }

    public class RowWithTilesPart : ContentPart<RowWithTilesRecord>
    {

        public IList<SingleRowWithTilesRecord> Rows
        {
            get { return Record.Rows; }
            set { Record.Rows = value; }
        }

        public string TilesJson
        {
            get { return Record.TilesJson; }
            set { Record.TilesJson = value; }
        }
    }
}