using Orchard;
using Orchard.ContentManagement;

namespace DevOffice.Secret.Models {
	
    public class EmailSettingsPart : ContentPart<EmailSettingsPartRecord> {
        public string SendGridAccountName {
            get { return Record.SendGridAccountName; }
            set { Record.SendGridAccountName = value; }
        }
        public string SendGridAccountPassword {
            get { return Record.SendGridAccountPassword; }
            set { Record.SendGridAccountPassword = value; }
        }
        public string ActivityFeedApiFromEmailAddress
        {
            get { return Record.ActivityFeedApiFromEmailAddress; }
            set { Record.ActivityFeedApiFromEmailAddress = value; }
        }
        public string ActivityFeedApiFromEmailTitle
        {
            get { return Record.ActivityFeedApiFromEmailTitle; }
            set { Record.ActivityFeedApiFromEmailTitle = value; }
        }
        public string ActivityFeedApiEmailSubject
        {
            get { return Record.ActivityFeedApiEmailSubject; }
            set { Record.ActivityFeedApiEmailSubject = value; }
        }
        public string ActivityFeedApiEmailText
        {
            get { return Record.ActivityFeedApiEmailText; }
            set { Record.ActivityFeedApiEmailText = value; }
        }
        public string ActivityFeedApiEmailHtml
        {
            get { return Record.ActivityFeedApiEmailHtml; }
            set { Record.ActivityFeedApiEmailHtml = value; }
        }
        public string CloudStorageFromEmailAddress
        {
            get { return Record.CloudStorageFromEmailAddress; }
            set { Record.CloudStorageFromEmailAddress = value; }
        }
        public string CloudStorageFromEmailTitle
        {
            get { return Record.CloudStorageFromEmailTitle; }
            set { Record.CloudStorageFromEmailTitle = value; }
        }
        public string CloudStorageEmailSubject
        {
            get { return Record.CloudStorageEmailSubject; }
            set { Record.CloudStorageEmailSubject = value; }
        }
        public string CloudStorageEmailText
        {
            get { return Record.CloudStorageEmailText; }
            set { Record.CloudStorageEmailText = value; }
        }
        public string CloudStorageEmailHtml
        {
            get { return Record.CloudStorageEmailHtml; }
            set { Record.CloudStorageEmailHtml = value; }
        }
    }
}
