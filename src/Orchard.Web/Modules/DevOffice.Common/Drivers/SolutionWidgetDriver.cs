using System;
using System.Collections.Generic;
using System.Linq;
using DevOffice.Common.Models;
using DevOffice.Common.Services;
using DevOffice.Common.ViewModels;
using Orchard;
using Orchard.ContentManagement.Drivers;
using Orchard.Taxonomies.Models;
using Orchard.Taxonomies.Services;

namespace DevOffice.Common.Drivers {
    public class SolutionWidgetDriver : ContentPartDriver<SolutionWidgetPart> {
        private readonly ICommonDataService _commonDataService;
        private readonly ITaxonomyService _taxonomyService;
        private readonly IOrchardServices _orchardServices;

        public SolutionWidgetDriver(ICommonDataService commonDataService, ITaxonomyService taxonomyService,IOrchardServices orchardServices) {
            _commonDataService = commonDataService;
            _taxonomyService = taxonomyService;
            _orchardServices = orchardServices;

        }

        protected override DriverResult Display(SolutionWidgetPart part, string displayType, dynamic shapeHelper) {

            var topSolutions = _commonDataService.GetTopFourSolutionsFromCache();
            var solutions = new List<Solution>();
            var filters = new List<string>();
            solutions.AddRange(topSolutions);

            var queryString = _orchardServices.WorkContext.HttpContext.Request.QueryString["filters"];
            if (!string.IsNullOrEmpty(queryString))
            {
                solutions.AddRange(_commonDataService.GetSolutions(queryString.Split(',')));
                filters.AddRange(queryString.Split(',').Select(x => x.ToLower()));
            }
            else
            {
                solutions.AddRange(_commonDataService.GetSolutions());
            }

            List<string> solutionTypeTerms = _taxonomyService.GetTerms(_taxonomyService.GetTaxonomyByName("Solution Type").Id).OrderBy(x => x.Weight).Select(x=> x.Name).ToList();
            List<string> solutionDeviceTerms = _taxonomyService.GetTerms(_taxonomyService.GetTaxonomyByName("Solution Device").Id).OrderBy(x => x.Weight).Select(x => x.Name).ToList();
            List<string> solutionPrimaryWorkloadInvokedTerms = _taxonomyService.GetTerms(_taxonomyService.GetTaxonomyByName("Solution Primary Workload Invoked").Id).OrderBy(x => x.Weight).Select(x => x.Name).ToList();
            List<string> solutionIdentityIntegrationMethodTerms = _taxonomyService.GetTerms(_taxonomyService.GetTaxonomyByName("Solution Identity Integration Method").Id).OrderBy(x => x.Weight).Select(x => x.Name).ToList();


            var terms = _taxonomyService.GetTerms(_taxonomyService.GetTaxonomyByName("Solution Type").Id).OrderBy(x => x.Weight);
            var model = new SolutionViewModel {
                TaxonomySolutions = new List<TaxonomySolution>(),
                Devices = solutionDeviceTerms,
                Types = solutionTypeTerms,
                PrimaryWorkloadInvoked = solutionPrimaryWorkloadInvokedTerms,
                IdentityIntegrationMethod = solutionIdentityIntegrationMethodTerms,
                SelectedTypes = filters.Intersect(solutionTypeTerms.Select(x=> x.ToLower())).ToList(),
                SelectedDevices= filters.Intersect(solutionDeviceTerms.Select(x=> x.ToLower())).ToList(),
                SelectedPrimaryWorkloadInvoked = filters.Intersect(solutionPrimaryWorkloadInvokedTerms.Select(x=>x.ToLower())).ToList(),
                SelectedIdentityIntegrationMethod = filters.Intersect(solutionIdentityIntegrationMethodTerms.Select(x=>x.ToLower())).ToList()

            };
         
            if (terms.Any()) {
                foreach (var term in terms) {
                    model.TaxonomySolutions.Add(new TaxonomySolution {
                        Title = term.Name,
                        Solutions = solutions.Where(x => x.SolutionType.Any() && x.SolutionType.Contains(term.Weight)).OrderBy(x => x.Ordering).ToList()
                    });
                }
            }
            else {
                model.TaxonomySolutions.Add(new TaxonomySolution {
                    Title = "Solution",
                    Solutions = solutions.OrderBy(x => x.Ordering).ToList()
                });
            }

            dynamic item = (dynamic) part.ContentItem;
            model.HtmlData = item.SolutionWidgetPart.Description.Value;

            return ContentShape("Parts_SolutionWidget",
                  () =>
                  {
                      var shape = shapeHelper.Parts_SolutionWidget();
                      shape.ContentPart = part;
                      shape.ViewModel = model;
                      return shape;
                  });

        }
    }
}