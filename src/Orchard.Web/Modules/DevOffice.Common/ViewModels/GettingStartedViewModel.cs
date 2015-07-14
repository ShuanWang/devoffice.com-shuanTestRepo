using System.Collections.Generic;

namespace DevOffice.Common.ViewModels {
    public class GettingStartedViewModel {
        public List<GettingStartedTab> Tabs { get; set; }
    }
    
    public class GettingStartedTab {
        public string Title { get; set; }
        public string HashTag { get; set; }
        public string Intro { get; set; }
        public string FirstBlockTitle { get; set; }
        public string FirstBlockContent  { get; set; }
        public string SecondBlockTitle { get; set; }
        public string SecondBlockContent { get; set; }
        public string ThirdBlockTitle { get; set; }
        public string ThirdBlockContent { get; set; }
        public string DocumentsLink { get; set; }
        public string SamplesLink { get; set; }
        public string MVALink { get; set; }
        public int Ordering { get; set; }
        public string FirstBlockLayoutStyle { get; set; }

        public List<ImageLinks> Icons { get; set; }
        public string FirstBlockIcon1 { get; set; }
        public string FirstBlockIcon2 { get; set; }
        public string FirstBlockIcon3 { get; set; }
        public string FirstBlockIcon4 { get; set; }
        public string FirstBlockIcon5 { get; set; }
        public string FirstBlockIcon6 { get; set; } 
        public string FirstBlockTitle1 { get; set; }
        public string FirstBlockTitle2 { get; set; }
        public string FirstBlockTitle3 { get; set; }
        public string FirstBlockTitle4 { get; set; }
        public string FirstBlockTitle5 { get; set; }
        public string FirstBlockTitle6 { get; set; }
        public List<ImageLinks> FirstBlockScreenshots1 { get; set; }
        public List<ImageLinks> FirstBlockScreenshots2 { get; set; }
        public List<ImageLinks> FirstBlockScreenshots3 { get; set; }
        public List<ImageLinks> FirstBlockScreenshots4 { get; set; }
        public List<ImageLinks> FirstBlockScreenshots5 { get; set; }
        public List<ImageLinks> FirstBlockScreenshots6 { get; set; }
    }

    public class ImageLinks {
        public string Image { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
    }

}