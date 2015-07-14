using System.Collections.Generic;
using System.Linq;
using DevOffice.Common.Models;
using DevOffice.Common.Services;
using DevOffice.Common.ViewModels;
using Orchard;
using Orchard.ContentManagement.Drivers;
using Orchard.Taxonomies.Services;
using Orchard.Caching;
using Orchard.Services;
using System;

namespace DevOffice.Common.Drivers
{
    public class PatternsAndPracticesWidgetDriver : ContentPartDriver<PatternsAndPracticesWidgetPart>
    {
        private readonly ICommonDataService _commonDataService;
        private readonly ITaxonomyService _taxonomyService;
        private readonly IOrchardServices _services;
        private readonly ICacheManager _cacheManager;
        private readonly IClock _clock;

        public PatternsAndPracticesWidgetDriver(
            ICommonDataService commonDataService, 
            ITaxonomyService taxonomyService, 
            IOrchardServices services,
            IClock clock, 
            ICacheManager cacheManager)
        {
            _commonDataService = commonDataService;
            _taxonomyService = taxonomyService;
            _services = services;
            _cacheManager = cacheManager;
            _clock = clock;
        }

        protected override DriverResult Display(PatternsAndPracticesWidgetPart part, string displayType, dynamic shapeHelper)
        {
            int cacheTime = 45;

            var topViewed = _cacheManager.Get("pnp-topViewed", ctx =>
            {
                ctx.Monitor(
                 _clock.When(TimeSpan.FromMinutes(cacheTime)));
                return _commonDataService.GetTopViewedItems("patterns-and-practices");
            });
            
            var types = GetTaxonomyTermsFromCache(cacheTime, "pnp-types", "Patterns and Practices Content Type");

            var platforms = GetTaxonomyTermsFromCache(cacheTime,"pnp-platforms" ,"Code Sample Platform");
         
            var languages = GetTaxonomyTermsFromCache(cacheTime,"pnp-languages" , "Code Sample Language");
           
            var services = GetTaxonomyTermsFromCache(cacheTime,"pnp-services" ,"Code Sample Service" );
            
            var sourceReps = GetTaxonomyTermsFromCache(cacheTime,"pnp-sourceReps" , "Code Sample Source");
            
            var products = GetTaxonomyTermsFromCache(cacheTime,"pnp-products" , "Code Sample Product");
            
            var themes = GetTaxonomyTermsFromCache(cacheTime, "pnp-themes","Patterns and Practices Theme" );

            var secondaryTypes = GetTaxonomyTermsFromCache(cacheTime, "pnp-secondary-types", "Code Sample Type");

            
            var pnp_items = _cacheManager.Get("pnp-items", ctx =>
            {
                ctx.Monitor(
                 _clock.When(TimeSpan.FromMinutes(cacheTime)));
                return _commonDataService.GetPatternsAndPractices();
            });


            return ContentShape("Parts_PatternsAndPracticesWidget",
               () =>
               {
                   var shape = shapeHelper.Parts_PatternsAndPracticesWidget();
                   shape.ContentPart = part;
                   shape.ViewModel = new PatternsAndPracticesViewModel
                   {
                       Types = types,
                       PatternsAndPractices = pnp_items,
                       TopViewed = topViewed,
                       Platforms = platforms,
                       Languages = languages,
                       Services = services,
                       SourceReps = sourceReps,
                       Products = products,
                       Themes = themes,
                       SecondaryTypes = secondaryTypes
         
                   };
                   return shape;
               });      
        }

        public List<string> GetTaxonomyTermsFromCache(int cacheTime, string cacheKey, string taxonomyByName)
        {
            var types = _cacheManager.Get(cacheKey, ctx =>
            {
                ctx.Monitor(
                 _clock.When(TimeSpan.FromMinutes(cacheTime)));
                return _taxonomyService.GetTerms(_taxonomyService.GetTaxonomyByName(taxonomyByName).Id)
                .OrderBy(x => x.Weight).Select(term => term.Name).ToList();
            });
            return types;
        }


    }
}