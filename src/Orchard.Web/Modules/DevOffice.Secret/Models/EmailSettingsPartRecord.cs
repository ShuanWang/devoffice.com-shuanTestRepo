using Orchard;
using Orchard.ContentManagement.Records;

namespace DevOffice.Secret.Models {
	
    public class EmailSettingsPartRecord : ContentPartRecord {
        public virtual string SendGridAccountName{ get; set; }
        public virtual string SendGridAccountPassword{ get; set; }
        public virtual string ActivityFeedApiFromEmailAddress { get; set; }
        public virtual string ActivityFeedApiFromEmailTitle { get; set; }
        public virtual string ActivityFeedApiEmailSubject { get; set; }
        public virtual string ActivityFeedApiEmailText { get; set; }
        public virtual string ActivityFeedApiEmailHtml { get; set; }
        public virtual string CloudStorageFromEmailAddress { get; set; }
        public virtual string CloudStorageFromEmailTitle { get; set; }
        public virtual string CloudStorageEmailSubject { get; set; }
        public virtual string CloudStorageEmailText { get; set; }
        public virtual string CloudStorageEmailHtml { get; set; }
    }
}