using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevOffice.Secret.Models;
using DevOffice.Secret.ViewModels;
using Orchard.Data;

namespace DevOffice.Secret.Services
{
    public class SecretServices:ISecretServices
    {
        private readonly IRepository<RowWithTilesRecord> _rowWithTilesRepository; 

          public SecretServices( IRepository<RowWithTilesRecord> rowWithTilesRepository)
        {
            _rowWithTilesRepository = rowWithTilesRepository;
        }


          public RowWithTilesViewModel BuildEditorViewModel(RowWithTilesPart part, string itemsJson = "")
          {
              var ivm = new RowWithTilesViewModel {};

              //if (!string.IsNullOrEmpty(itemsJson))
              //{
              //    ivm.TilesJson = itemsJson;
              //}
              return ivm;
          }



    }
}