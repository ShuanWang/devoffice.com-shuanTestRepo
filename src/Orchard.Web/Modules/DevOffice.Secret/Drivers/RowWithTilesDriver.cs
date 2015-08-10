using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevOffice.Secret.Models;
using DevOffice.Secret.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Data;

namespace DevOffice.Secret.Drivers
{
    public class RowWithTilesDriver : ContentPartDriver<RowWithTilesPart>
    {



        private readonly ISecretServices _secretServices;

        public RowWithTilesDriver(
            ISecretServices secretServices,
            IRepository<RowWithTilesRecord> itemsRepository)
        {
            _secretServices = secretServices;

        }



        protected override DriverResult Editor(RowWithTilesPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var model = _secretServices.BuildEditorViewModel(part, "");
            if (!updater.TryUpdateModel(model, Prefix, null, null))
            {
                //_notifier.Error(T("Error during Carousel Item update."));
                //Services.Notifier.Error(T("Please enter all the required fields and submit again"));
            }

            if (part.ContentItem != null)
            {
                //_commonDataService.UpdateRelatedLinks(part.ContentItem, model);
            }

            return ContentShape("Parts_RowWithTiles_Edit",
                              () => shapeHelper.EditorTemplate(TemplateName: "Parts/RowWithTiles", Model: model, Prefix: Prefix));
        }


        protected override DriverResult Editor(RowWithTilesPart part, dynamic shapeHelper)
        {
            //return ContentShape("Parts_RowWithTiles_Edit", () => shapeHelper.EditorTemplate(
            //    //TemplateName: "Parts/RowWithTiles",
            //    Model: _secretServices.BuildEditorViewModel(part, "")));
            //Prefix: Prefix));

            return ContentShape("Parts_RowWithTiles_Edit",
             () => shapeHelper.EditorTemplate(
                                      TemplateName: "Parts/RowWithTiles"));
        }
    }
}