using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevOffice.Secret.Models;
using DevOffice.Secret.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Data;
using Orchard.Localization;

namespace DevOffice.Secret.Drivers
{
    public class RowWithTilesDriver : ContentPartDriver<RowWithTilesPart>
    {
        private readonly IRepository<SingleRowWithTilesRecord> _itemsRepository;
        private readonly ISecretServices _secretServices;

        public RowWithTilesDriver(
            ISecretServices secretServices,
            IRepository<SingleRowWithTilesRecord> itemsRepository)
        {
            _secretServices = secretServices;
            _itemsRepository = itemsRepository;
        }
        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }

        protected override string Prefix
        {
            get { return "DevOffice_Secret"; }
        }


        protected override DriverResult Editor(RowWithTilesPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var model = _secretServices.BuildEditorViewModel(part);
            if (!updater.TryUpdateModel(model, Prefix, null, null))
            {
                //_notifier.Error(T("Error during Carousel Item update."));
                //Services.Notifier.Error(T("Please enter all the required fields and submit again"));
            }

            if (part.ContentItem != null)
            {
                _secretServices.UpdateRowsWithTiles(part.ContentItem, model);
            }

            return ContentShape("Parts_RowWithTiles_Edit",
                              () => shapeHelper.EditorTemplate(TemplateName: "Parts/RowWithTiles", Model: model, Prefix: Prefix));
        }


        protected override DriverResult Editor(RowWithTilesPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_RowWithTiles_Edit", () => shapeHelper.EditorTemplate(
                TemplateName: "Parts/RowWithTiles",
                Model: _secretServices.BuildEditorViewModel(part),
                Prefix: Prefix
                ));

           
        }
    }
}