using System;
using System.Text;
using JetBrains.Annotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Notify;
using DevOffice.Secret.Models;

namespace DevOffice.Secret.Drivers
{
    [UsedImplicitly]
    public class EmailSettingsPartDriver : ContentPartDriver<EmailSettingsPart>
    {
        private readonly IEncryptionService _encryptionService;
        private readonly INotifier _notifier;
        private const string TemplateName = "Parts/EmailSettingsPart";

        public Localizer T { get; set; }

        public EmailSettingsPartDriver(INotifier notifier, IEncryptionService encryptionService)
        {
            _notifier = notifier;
            _encryptionService = encryptionService;
            T = NullLocalizer.Instance;
        }

       protected override DriverResult Editor(EmailSettingsPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_EmailSettingsPart",
                    () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(EmailSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var currentPassword = part.SendGridAccountPassword;
            if (updater.TryUpdateModel(part, Prefix, null, null))
            {
                var newPassword = part.SendGridAccountPassword;
                if (currentPassword != newPassword)
                {
                    part.SendGridAccountPassword = GetEncryptedPassword(newPassword);
                }
            }
            else
            {
                _notifier.Error(T("Error during email settings update!"));
            }
            return Editor(part, shapeHelper);
        }

        private string GetEncryptedPassword(string password)
        {
            var encrypterPassword = Convert.ToBase64String(_encryptionService.Encode(Encoding.UTF8.GetBytes(password)));
            return encrypterPassword;
        }

    }
}