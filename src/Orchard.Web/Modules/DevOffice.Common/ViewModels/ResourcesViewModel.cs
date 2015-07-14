using System.Collections.Generic;

namespace DevOffice.Common.ViewModels
{
    public class ResourcesViewModel
    {
        public List<Resource> Resources { get; set; }
    }

    public class Resource
    {
        public string Title { get; set; }
        public string SubText { get; set; }
        public string Image { get; set; }
        public string LinkText { get; set; }
        public string LinkUrl { get; set; }
        public int Id { get; set; }
    }
}