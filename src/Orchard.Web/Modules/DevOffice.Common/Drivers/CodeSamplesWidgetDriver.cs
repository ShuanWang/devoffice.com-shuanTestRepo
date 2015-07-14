using System;
using System.Runtime.ConstrainedExecution;
using System.Web;
using DevOffice.Common.Models;
using DevOffice.Common.Services;
using DevOffice.Common.ViewModels;
using Orchard;
using Orchard.ContentManagement.Drivers;
using System.Collections.Generic;
using System.Linq;
using Orchard.Taxonomies.Services;
using Orchard.Caching;
using Orchard.Services;

namespace DevOffice.Common.Drivers
{
    public class CodeSamplesWidgetDriver : ContentPartDriver<CodeSamplesWidgetPart>
    {
        private readonly ICommonDataService _commonDataService;
        private readonly ITaxonomyService _taxonomyService;
        private readonly IOrchardServices _services;
        private readonly ICacheManager _cacheManager;
        private readonly IClock _clock;

        public CodeSamplesWidgetDriver(ICommonDataService commonDataService, ITaxonomyService taxonomyService, IOrchardServices Services, IClock clock, ICacheManager cacheManager)
        {
            _commonDataService = commonDataService;
            _taxonomyService = taxonomyService;
            _services = Services;
            _cacheManager = cacheManager;
            _clock = clock;
        }

        protected override DriverResult Display(CodeSamplesWidgetPart part, string displayType, dynamic shapeHelper) {
            int CacheTime = 45;
            List<string> types = GetTaxonomyTermsFromCache(CacheTime, "codesamples-types", "Code Sample Type");
            List<string> platforms = GetTaxonomyTermsFromCache(CacheTime, "codesamples-platforms", "Code Sample Platform");
            List<string> languages = GetTaxonomyTermsFromCache(CacheTime, "codesamples-languages", "Code Sample Language");
            List<string> services = GetTaxonomyTermsFromCache(CacheTime, "codesamples-services", "Code Sample Service");
            List<string> sourceReps = GetTaxonomyTermsFromCache(CacheTime, "codesamples-sourceReps", "Code Sample Source");
            List<string> products = GetTaxonomyTermsFromCache(CacheTime, "codesamples-products", "Code Sample Product");

            return ContentShape("Parts_CodeSamplesWidget",
                   () =>
                   {
                       var shape = shapeHelper.Parts_CodeSamplesWidget();
                       shape.ContentPart = part;
                       shape.ViewModel = new CodeSamplesViewModel
                       {
                           Types = types,
                           Platforms = platforms,
                           Languages = languages,
                           Services = services,
                           SourceReps = sourceReps,
                           Products = products,
                           CodeSamples = _cacheManager.Get("code-samples-items",ctx =>
                                {
                                            ctx.Monitor(
                               _clock.When(TimeSpan.FromMinutes(CacheTime))); 
                              return _commonDataService.GetCodeSamples(); }),

                           TopViewed = _cacheManager.Get("code-samples-topviewed",ctx =>
                                {
                                            ctx.Monitor(
                               _clock.When(TimeSpan.FromMinutes(CacheTime))); 
                              return  _commonDataService.GetTopViewedItems("code-samples"); })
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