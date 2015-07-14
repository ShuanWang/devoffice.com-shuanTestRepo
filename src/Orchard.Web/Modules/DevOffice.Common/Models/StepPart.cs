using System;
using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using Orchard.Data.Conventions;

namespace DevOffice.Common.Models
{
    public class StepPart : ContentPart<StepPartRecord>
    {
        public IList<StepInformationRecord> Steps
        {
            get { return Record.Steps; }
        }
    }

    public class StepPartRecord : ContentPartRecord
    {
        public virtual IList<StepInformationRecord> Steps { get; set; } 
    }

    public class StepInformationRecord 
    {
        public virtual int Id { get; set; }
        public virtual int StepPartId { get; set; }
        [StringLengthMax]
        public virtual string Title { get; set; }
        [StringLengthMax]
        public virtual string Description { get; set; }
        public virtual string LeftImage { get; set; }
        public virtual string RightImage { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual int Position { get; set; }

        public virtual IList<SubstepInformationRecord> Substeps { get; set; }
    }

    public class SubstepInformationRecord 
    {
        public virtual StepInformationRecord StepInformationRecord { get; set; }

        public virtual int Id { get; set; }
        [StringLengthMax]
        public virtual string Title { get; set; }
        [StringLengthMax]
        public virtual string Description { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual int Position { get; set; }
    }


}