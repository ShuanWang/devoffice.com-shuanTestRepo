using System.Collections.Generic;
using Sunkist.FeaturedItemSlider.ViewModels;

namespace Sunkist.FeaturedItemSlider.Models {
    public class FeaturedItemViewModel {
        public string Headline { get; set; }
        public string SubHeadline { get; set; }
        public string LinkUrl { get; set; }
        public bool SeparateLink { get; set; }
        public string LinkText { get; set; }
        public string ImagePath { get; set; }
        public int SlideNumber { get; set; }
        public List<string> DescriptionImagePaths { get; set; }
        public List<ImageLinksViewModel.ImageLinks> ImageLinks { get; set; }
        public string FeaturedImagePath { get; set; }
    }
}