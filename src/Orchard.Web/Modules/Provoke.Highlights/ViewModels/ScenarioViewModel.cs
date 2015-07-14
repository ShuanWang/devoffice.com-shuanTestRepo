using DevOffice.Common.Models;
using Provoke.Highlights.Models;
using System.Collections.Generic;

namespace Provoke.Highlights.ViewModels
{
    public class ScenarioViewModel
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }

        public virtual ICollection<TaskRecord> Tasks { get; set; }
        public string TasksJson { get; set; }
        public string RelatedResourcesJson { get; set; }

        public bool IsAdminView { get; set; }
        public ICollection<RelatedResourceRecord> RelatedResources { get; set; }
        //public string LinksJson { get; set; }


        public ScenarioViewModel()
        {
            Tasks = new List<TaskRecord>();
            RelatedResources = new List<RelatedResourceRecord>();
        }

    }
}