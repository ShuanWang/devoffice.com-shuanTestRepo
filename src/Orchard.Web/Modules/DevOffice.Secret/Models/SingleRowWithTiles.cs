using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevOffice.Secret.Models
{
    public class SingleRowWithTilesRecord
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Body { get; set; }
        public virtual string ExternalLink { get; set; }
        public virtual string LinkText { get; set; }
        public virtual int SortOrder { get; set; }

        public virtual string Tile1Title { get; set; }
        public virtual string Tile1ExternalLink { get; set; }
        public virtual string Tile1Thumbnail { get; set; }

        public virtual string Tile2Title { get; set; }
        public virtual string Tile2ExternalLink { get; set; }
        public virtual string Tile2Thumbnail { get; set; }

        public virtual RowWithTilesRecord RowWithTilesRecord { get; set; }


    }
}