using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevOffice.Common.Models
{
    public class ViewCountRecord
    {
        public virtual int Id { get; set; }
        public virtual int LinkId { get; set; }
        public virtual DateTime Date { get; set; }
        public string Type { get; set; }
    }
}