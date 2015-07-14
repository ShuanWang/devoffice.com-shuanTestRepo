
using DevOffice.Common.Models;
using DevOffice.Common.Services;
using DevOffice.Common.ViewModels;
using Orchard.ContentManagement.Drivers;
using System.Collections.Generic;
using System.Linq;
using Orchard.Taxonomies.Services;
using System.Text.RegularExpressions;
using Orchard.Caching;
using Orchard.Services;
using System;

namespace DevOffice.Common.Drivers
{
    public class TrainingWidgetDriver : ContentPartDriver<TrainingWidgetPart>
    {
        private readonly ICommonDataService _commonDataService;
        private readonly ITaxonomyService _taxonomyService;
        private readonly ICacheManager _cacheManager;
        private readonly IClock _clock;
        public TrainingWidgetDriver(ICommonDataService commonDataService, ITaxonomyService taxonomyService, IClock clock, ICacheManager cacheManager)
        {
            _commonDataService = commonDataService;
            _taxonomyService = taxonomyService;
            _cacheManager = cacheManager;
            _clock = clock;
        }

        protected override DriverResult Display(TrainingWidgetPart part, string displayType, dynamic shapeHelper)
        {
            int cacheTime = 45;
            var trainingItems = _cacheManager.Get("training-items", ctx =>
            {
                ctx.Monitor(
                 _clock.When(TimeSpan.FromMinutes(cacheTime)));
                return _commonDataService.GetTrainingItems();
            });
           //// var terms = _taxonomyService.GetTerms(_taxonomyService.GetTaxonomyByName("Training Type").Id).OrderBy(x => x.Weight);


           // Dictionary<int, string> uniqueDictionaryOfTerms = new Dictionary<int, string>();
           // var nonDistinctValues = trainingItems.SelectMany(x => x.TermsTagged);
           // foreach (var keyValuePair in nonDistinctValues.Where(keyValuePair => !uniqueDictionaryOfTerms.ContainsKey(keyValuePair.Key))) {
           //     uniqueDictionaryOfTerms.Add(keyValuePair.Key,keyValuePair.Value);
           // }
           List<string> taxonomyTypes = _taxonomyService.GetTerms(_taxonomyService.GetTaxonomyByName("Training Type").Id).OrderBy(x => x.Weight).Select(term => term.Name).ToList();

                
            var model = new TrainingViewModel();
            model.AllTrainingItems = trainingItems;
            model.TopViewed = _cacheManager.Get("training-topViewed", ctx =>
            {
                ctx.Monitor(
                 _clock.When(TimeSpan.FromMinutes(cacheTime)));
                return _commonDataService.GetTopViewedItems("training");
            });                
               
            model.Type = "Training";
            model.TaxonomyNames = taxonomyTypes;

                //foreach (var term in uniqueDictionaryOfTerms)
                //{
                //    model.TaxonomyTrainingItems.Add(new TaxonomyTrainingItem {
                //        Title = term.Value,
                //        SafeTitle = Regex.Replace(term.Value, "[^0-9a-zA-Z]+", string.Empty),
                //        //TrainingItems = trainingItems.Where(x => x.TermsTagged.Contains(term)).ToList() //don't think i need this?
                //    });
                //}
            

         
            return ContentShape("Parts_TrainingWidget",
                () => shapeHelper.Partial(
                    TemplateName: "Parts/TrainingWidget",
                    Model:model
                    ));

        }
    }
}