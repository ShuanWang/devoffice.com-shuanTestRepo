using System;

namespace DevOffice.Common.Models
{
    public class FeedbackInformationRecord
    {
        public virtual int Id { get; set; }
        public virtual bool IsHelpful { get; set; }
        public virtual string FeedbackContent { get; set; }
        public virtual DateTime DateCreated { get; set; }
    }
}