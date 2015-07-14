using Newtonsoft.Json;

namespace Provoke.Highlights.Models
{
    public class StepRecord
    {
        public virtual int Id { get; set; }

        public virtual string Title        { get; set; } //
        public virtual string Description  { get; set; } // 
        public virtual int    SortOrder    { get; set; } // 
        public virtual double TopPosition  { get; set; } // integer pixel value
        public virtual double LeftPosition { get; set; } // integer pixel value
        public virtual string Anchor       { get; set; } // top bottom left right
        public virtual string Image        { get; set; } // Media library

        [JsonIgnore]
        public virtual TaskRecord TaskRecord { get; set; }
    }
}