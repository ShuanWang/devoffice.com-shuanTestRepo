using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using System.Collections.Generic;
using Provoke.Highlights.ViewModels;

namespace Provoke.Highlights.Models
{
    public class ScenarioRecord : ContentPartRecord {
        public ScenarioRecord() {
            Tasks = new List<TaskRecord>();
            RelatedResources = new List<RelatedResourceRecord>();
        }

        public virtual ICollection<TaskRecord> Tasks { get; set; }
        public virtual ICollection<RelatedResourceRecord> RelatedResources { get; set; }
        public virtual string Title       { get; set; }
        public virtual string Description { get; set; }
    }

    public class ScenarioPart : ContentPart<ScenarioRecord> {
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

        public string Description
        {
            get { return Record.Description; }
            set { Record.Description = value; }
        }

        public ICollection<TaskRecord> Tasks
        {
            get { return Record.Tasks; }
            set { Record.Tasks = value; }
        }
        public ICollection<RelatedResourceRecord> RelatedResources
        {
            get { return Record.RelatedResources; }
            set { Record.RelatedResources = value; }
        }

        public string TasksJson { get; set; }
        public string ResourcesJson { get; set; }
    }
}