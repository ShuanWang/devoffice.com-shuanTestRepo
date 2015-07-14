using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DevOffice.Secret.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Core.Contents.Drivers;
using Orchard.Localization;
using Orchard.Security;

namespace DevOffice.Secret.Drivers
{
    public class SharePointSettingsPartDriver: ContentPartDriver<SharePointSettingsPart> {
        
        private readonly IEncryptionService _encryptionService;

        public SharePointSettingsPartDriver(IEncryptionService encryptionService) {
            _encryptionService = encryptionService;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override string Prefix { get { return "SharePointSettings"; } }

        protected override DriverResult Editor(SharePointSettingsPart part, dynamic shapeHelper) {
            return ContentShape("Parts_SharePointSettings_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "SharePointSettings",
                    Model: part,
                    Prefix: Prefix));
        }

        protected override DriverResult Editor(SharePointSettingsPart part, IUpdateModel updater, dynamic shapeHelper) {
            var currentPassword = part.Password;
            if (updater.TryUpdateModel(part, Prefix, null, null)) {
                var newPassword = part.Password;
                if (currentPassword != newPassword) {
                    part.Password = GetEncryptedPassword(newPassword);
                }
            }

            return Editor(part, shapeHelper);
        }

        private string GetEncryptedPassword(string password) {
            var encrypterPassword = Convert.ToBase64String(_encryptionService.Encode(Encoding.UTF8.GetBytes(password)));
            return encrypterPassword;
        }
    }
}