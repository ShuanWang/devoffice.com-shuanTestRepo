using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevOffice.Secret.Models;
using DevOffice.Secret.Services;
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


        protected override DriverResult Editor(RowWithTilesPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_RowWithTiles_Edit", () => shapeHelper.EditorTemplate(
                //TemplateName: "Parts/RowWithTiles",
                Model: _secretServices.BuildEditorViewModel(part, "")));
            //Prefix: Prefix));
        }
    }
}