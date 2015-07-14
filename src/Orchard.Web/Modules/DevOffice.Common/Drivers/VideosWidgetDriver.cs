using DevOffice.Common.Models;
using DevOffice.Common.Services;
using DevOffice.Common.ViewModels;
using Orchard.ContentManagement.Drivers;
using System.Collections.Generic;
using System.Linq;
using Orchard.Taxonomies.Services;

namespace DevOffice.Common.Drivers
{
    public class VideosWidgetDriver : ContentPartDriver<VideosWidgetPart>
    {
        private readonly ICommonDataService _commonDataService;
        private readonly ITaxonomyService _taxonomyService;
        public VideosWidgetDriver(ICommonDataService commonDataService, ITaxonomyService taxonomyService)
        {
            _commonDataService = commonDataService;
            _taxonomyService = taxonomyService;
        }

        protected override DriverResult Display(VideosWidgetPart part, string displayType, dynamic shapeHelper) {
            List<Video> videoItems = _commonDataService.GetVideos();
            var terms = _taxonomyService.GetTerms(_taxonomyService.GetTaxonomyByName("Video Type").Id).OrderBy(x => x.Weight);

            var model = new VideoViewModel();
            model.TaxonomyVideoItems = new List<TaxonomyVideoItem>();

            if (terms.Any()) {
                foreach (var term in terms) {
                    model.TaxonomyVideoItems.Add(new TaxonomyVideoItem
                    {
                        Title = term.Name,
                        VideoItems = videoItems.Where(x => x.VideoTypes.Contains(term.Weight)).ToList()
                    });
                }
            }
            else {
                model.TaxonomyVideoItems.Add(new TaxonomyVideoItem
                {
                    Title = "Videos",
                    VideoItems = videoItems
                });
            }

            return ContentShape("Parts_VideosWidget",
                () => shapeHelper.Partial(
                    TemplateName: "Parts/VideosWidget",
                    Model: model
                    ));

        }
    }
}