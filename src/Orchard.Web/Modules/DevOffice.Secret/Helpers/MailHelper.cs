using System;
using System.Configuration;
using System.Linq;
using System.Text;
using DevOffice.Secret.Models;
using Orchard.ContentManagement;
using Orchard.Security;
using SendGrid;
using System.Net;
using System.Net.Mail;

namespace DevOffice.Secret.Helpers
{
    public class MailHelper {

        private readonly IEncryptionService _encryptionService;
        private EmailSettingsPart _mailSettings;

        public MailHelper(IContentManager contentManager, IEncryptionService encryptionService) 
        {
            _encryptionService = encryptionService;
            _mailSettings = contentManager.Query<EmailSettingsPart, EmailSettingsPartRecord>().List().FirstOrDefault();
            if (_mailSettings == null) 
            {
                _mailSettings = new EmailSettingsPart();
                _mailSettings.Record = new EmailSettingsPartRecord();
            }
        }

        public void SendActivityFeedApiEmail(string emailAddress, string firstName)
        {
            if (string.IsNullOrWhiteSpace(_mailSettings.ActivityFeedApiFromEmailAddress) == false) 
            {
                var mailMessage = new SendGridMessage();
                mailMessage.AddTo(emailAddress);
                mailMessage.From = new MailAddress(_mailSettings.ActivityFeedApiFromEmailAddress, _mailSettings.ActivityFeedApiFromEmailTitle);
                mailMessage.Subject = _mailSettings.ActivityFeedApiEmailSubject;
                mailMessage.Text = _mailSettings.ActivityFeedApiEmailText.Replace("{FirstName}", firstName);
                mailMessage.Html = _mailSettings.ActivityFeedApiEmailHtml.Replace("{FirstName}", firstName);
                SendWithSendGrid(mailMessage);
            }
        }

        public void SendCloudStorageEmail(string emailAddress, string firstName)
        {
            if (string.IsNullOrWhiteSpace(_mailSettings.ActivityFeedApiFromEmailAddress) == false) 
            {
                var mailMessage = new SendGridMessage();
                mailMessage.AddTo(emailAddress);
                mailMessage.From = new MailAddress(_mailSettings.CloudStorageFromEmailAddress, _mailSettings.CloudStorageFromEmailTitle);
                mailMessage.Subject = _mailSettings.CloudStorageEmailSubject;
                mailMessage.Text = _mailSettings.CloudStorageEmailText.Replace("{FirstName}", firstName);
                mailMessage.Html = _mailSettings.CloudStorageEmailHtml.Replace("{FirstName}", firstName);
                SendWithSendGrid(mailMessage);
            }
        }

        private void SendWithSendGrid(SendGridMessage message) {

            var unencodedPassword = Encoding.UTF8.GetString(_encryptionService.Decode(Convert.FromBase64String(_mailSettings.SendGridAccountPassword)));
            var credentials = new NetworkCredential(_mailSettings.SendGridAccountName, unencodedPassword);

            // Create an Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email.
            transportWeb.Deliver(message);
        }
    }
}