using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevOffice.Secret.Models;


namespace DevOffice.Secret.ViewModels
{
    public class OverviewPageViewModel
    {
        public string Title { get; set; }
        public string BannerImage { get; set; }
        public string IntroText { get; set; }

        public List<SingleRowWithTilesRecord> Rows { get; set; }
    }
}