using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevOffice.Common.Models;
using DevOffice.Common.Services;
using DevOffice.Common.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Data;
using Orchard.Localization;
using Orchard.UI.Notify;

namespace DevOffice.Common.Drivers
{
    public class RelatedLinksDriver: ContentPartDriver<RelatedLinksPart> {

        private readonly IRepository<RelatedLinkRecord> _itemsRepository;
        private readonly INotifier _notifier;
        private readonly ICommonDataService _commonDataService;

        public RelatedLinksDriver(
            ICommonDataService commonDataService, 
            INotifier notifier, 
            IRepository<RelatedLinkRecord> itemsRepository) {
            _commonDataService = commonDataService;
            _notifier = notifier;
            _itemsRepository = itemsRepository;
        }

        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }

        private const string TemplateName = "Parts/RelatedLinks";

        protected override string Prefix
        {
            get { return "DevOffice_Common"; }
        }

        protected override DriverResult Display(RelatedLinksPart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_RelatedLinks", () => shapeHelper.Parts_Items(
                CarouselItems: part
                ));
        }

        protected override DriverResult Editor(RelatedLinksPart part, dynamic shapeHelper) {
            return ContentShape("Parts_RelatedLinks_Edit", () =>
            {

                return shapeHelper.EditorTemplate(
                    TemplateName: TemplateName,
                    Model: _commonDataService.BuildEditorViewModel(part),
                    Prefix: Prefix);
            });
        }

        protected override DriverResult Editor(RelatedLinksPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var model = _commonDataService.BuildEditorViewModel(part);
            if (!updater.TryUpdateModel(model, Prefix, null, null))
            {
                //_notifier.Error(T("Error during Carousel Item update."));
                Services.Notifier.Error(T("Please enter all the required fields and submit again"));
            }

            if (part.ContentItem != null)
            {
                _commonDataService.UpdateRelatedLinks(part.ContentItem, model);
            }

            return ContentShape("Parts_RelatedLinks_Edit",
                              () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: model, Prefix: Prefix));
        }

        
    }
}