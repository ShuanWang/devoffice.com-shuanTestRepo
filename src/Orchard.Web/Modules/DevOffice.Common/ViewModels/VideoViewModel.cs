using System;
using System.Collections.Generic;

namespace DevOffice.Common.ViewModels
{
    public class VideoViewModel
    {
        public List<TaxonomyVideoItem> TaxonomyVideoItems { get; set; }
    }

    public class TaxonomyVideoItem
    {
        public List<Video> VideoItems { get; set; }
        public string Title { get; set; }
    }
    public class Video
    {
        public int VideoId { get; set; }
        public string Title { get; set; }
        public string SubText { get; set; }
        public string Image { get; set; }
        public string Location { get; set; }
        public List<int> VideoTypes { get; set; }
        public string Month { get; set; }
        public string Date { get; set; }
        public string Year { get; set; }
        public DateTime FullStartDate { get; set; }
        public string EmbedCode { get; set; }
    }
}