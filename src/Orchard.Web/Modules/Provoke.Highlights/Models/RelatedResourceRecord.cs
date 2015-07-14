using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace Provoke.Highlights.Models
{
    public class RelatedResourceRecord {
        //: ContentPartRecord {

        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Type { get; set; }
        public virtual string Url { get; set; }
        public virtual int SortOrder { get; set; }
        
        [JsonIgnoreAttribute]
        public virtual ScenarioRecord ScenarioRecord { get; set; }
    }

    public class RelatedResourcePart : ContentPart<RelatedResourceRecord>
        {
            public int Id
            {
                get { return Record.Id; }
                set { Record.Id = value; }
            }

            public string Title
            {
                get { return Record.Title; }
                set { Record.Title = value; }
            }

            public string Type
            {
                get { return Record.Type; }
                set { Record.Type = value; }
            }

            public string Url
            {
                get { return Record.Url; }
                set { Record.Url = value; }
            }

            //public string RelatedResourceJson
            //{
            //    get { return Record.RelatedResourceJson; }
            //    set { Record.RelatedResourceJson = value; }
            //}

            public int SortOrder 
            {
                get { return Record.SortOrder; }
                set { Record.SortOrder = value; }
            }

            public virtual ScenarioRecord ScenarioRecord {
                get { return Record.ScenarioRecord; }
                set { Record.ScenarioRecord = value; }
            }
        }
    }


