using DevOffice.Common.Models;
using DevOffice.Common.Services;
using DevOffice.Common.ViewModels;
using Orchard;
using Orchard.ContentManagement.Drivers;
using System.Collections.Generic;
using System.Linq;
using Orchard.Taxonomies.Services;
using System.Text.RegularExpressions;

namespace DevOffice.Common.Drivers
{
    public class PodcastsWidgetDriver : ContentPartDriver<PodcastsWidgetPart>
    {
        private readonly ICommonDataService _commonDataService;
        private readonly ITaxonomyService _taxonomyService;
        private readonly IOrchardServices _services;

        public PodcastsWidgetDriver(ICommonDataService commonDataService, ITaxonomyService taxonomyService,   IOrchardServices services)
        {
            _commonDataService = commonDataService;
            _taxonomyService = taxonomyService;
            _services = services;
        }

        protected override DriverResult Display(PodcastsWidgetPart part, string displayType, dynamic shapeHelper) {

            var queryString = _services.WorkContext.HttpContext.Request.QueryString["filters"];
            List<Training> trainingItems = _commonDataService.GetPodcasts();
            var terms = _taxonomyService.GetTerms(_taxonomyService.GetTaxonomyByName("Podcast Type").Id).OrderBy(x => x.Weight);
             var filters = new List<string>();
             var podcasts = new List<Training>();

            var model = new TrainingViewModel();
            model.Type = "Podcast";
                List<string> types = _taxonomyService.GetTerms(_taxonomyService.GetTaxonomyByName("Podcast Type").Id).OrderBy(x => x.Weight).Select(term => term.Name).ToList();
            model.TaxonomyTrainingItems = new List<TaxonomyTrainingItem>();
            if (terms.Any()) {
                foreach (var term in terms) {
                    model.TaxonomyTrainingItems.Add(new TaxonomyTrainingItem {
                        Title = term.Name,
                        SafeTitle = Regex.Replace(term.Name, "[^0-9a-zA-Z]+", string.Empty),
                        TrainingItems = trainingItems.Where(x => x.TrainingTypes.Contains(term.Weight)).ToList()
                    });
                }
            }
            else {
                model.TaxonomyTrainingItems.Add(new TaxonomyTrainingItem
                {
                    Title = "Podcasts",
                    TrainingItems = trainingItems
                });
            }

            return ContentShape("Parts_PodcastsWidget",
                () =>
                {
                    var shape = shapeHelper.Parts_PodcastsWidget();
                    shape.ContentPart = part;
                    shape.ViewModel = model;

                    return shape;
                });

        }
    }
}