using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevOffice.Common.ViewModels
{
    public class BannerViewModel
    {
        public string BodyText { get; set; }
        public string ExternalLink { get; set; }
        public string ExternalLinkText { get; set; }
        public string LinkBackgroundColor { get; set; }
        public string LinkTextColor { get; set; }
    }
}