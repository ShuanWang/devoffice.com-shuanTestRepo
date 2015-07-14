using System.Collections.Generic;
using Newtonsoft.Json;

namespace Provoke.Highlights.Models
{
    public class TaskRecord
    {
        public virtual int Id { get; set; }

        public virtual string Title { get; set; }
        public virtual string Duration { get; set; }
        public virtual string Description    { get; set; }
        public virtual int    SortOrder      { get; set; }

        [JsonIgnoreAttribute]
        public virtual ScenarioRecord ScenarioRecord { get; set; }
        public virtual ICollection<StepRecord> Steps { get; set; }
    }
}