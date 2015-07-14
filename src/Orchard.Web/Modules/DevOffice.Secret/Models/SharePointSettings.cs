using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace DevOffice.Secret.Models
{
    public class SharePointSettingsPart : ContentPart<SharePointSettingsPartRecord>
    {
        public string Username
        {
            get { return Record.Username; }
            set { Record.Username = value; }
        }
        public string Password
        {
            get { return Record.Password; }
            set { Record.Password = value; }
        }

        public string ContainingWebUrl
        {
            get { return Record.ContainingWebUrl; }
            set { Record.ContainingWebUrl = value; }
        }

        public string TargetListName
        {
            get { return Record.TargetListName; }
            set { Record.TargetListName = value; }
        }

        public string TargetListItemMetaType
        {
            get { return Record.TargetListItemMetaType; }
            set { Record.TargetListItemMetaType = value; }
        }


        public string ApiSubmission_ContainingWebUrl
        {
            get { return Record.ApiSubmission_ContainingWebUrl; }
            set { Record.ApiSubmission_ContainingWebUrl = value; }
        }

        public string ApiSubmission_TargetListName
        {
            get { return Record.ApiSubmission_TargetListName; }
            set { Record.ApiSubmission_TargetListName = value; }
        }

        public string ApiSubmission_TargetListItemMetaType
        {
            get { return Record.ApiSubmission_TargetListItemMetaType; }
            set { Record.ApiSubmission_TargetListItemMetaType = value; }
        }
    }

    public class SharePointSettingsPartRecord : ContentPartRecord {
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual string ContainingWebUrl { get; set; }
        public virtual string TargetListName { get; set; }
        public virtual string TargetListItemMetaType { get; set; }
        public virtual string ApiSubmission_ContainingWebUrl { get; set; }
        public virtual string ApiSubmission_TargetListName { get; set; }
        public virtual string ApiSubmission_TargetListItemMetaType { get; set; }
    }
}