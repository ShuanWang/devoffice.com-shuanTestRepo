using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevOffice.Common.ViewModels
{
    public class SectionsWithTilesPageViewModel
    {
        public string Subtitle { get; set; }
        public string Body { get; set; }
        public string ExternalLink { get; set; }
        public string LinkText { get; set; }
        public string Image { get; set; }
        public string BannerImage { get; set; }
        public List<string> PartnerLogos { get; set; }
        public string Title { get; set; }
        public string TilePageStyle { get; set; }

        public string Section1Title { get; set; }
        public string Section1Body { get; set; }
        public string Section1TileImage { get; set; }
        public string Section1ExternalLink { get; set; }
        public string Section1LinkText { get; set; }
        public List<Tile> Section1Tiles { get; set; }

        public string Section2Title { get; set; }
        public string Section2Body { get; set; }
        public string Section2TileImage { get; set; }
        public string Section2ExternalLink { get; set; }
        public string Section2LinkText { get; set; }
        public List<Tile> Section2Tiles { get; set; }

        public List<Resource> Resources { get; set; }

    }

    public class Tile {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string LinkText { get; set; }
    }
}