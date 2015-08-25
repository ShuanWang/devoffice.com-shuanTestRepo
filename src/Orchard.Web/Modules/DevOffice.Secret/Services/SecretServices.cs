using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevOffice.Secret.Models;
using DevOffice.Secret.ViewModels;
using Newtonsoft.Json;
using Orchard.ContentManagement;
using Orchard.Data;

namespace DevOffice.Secret.Services
{
    public class SecretServices:ISecretServices
    {
        private readonly IRepository<SingleRowWithTilesRecord> _singleRowWithTilesRepository;
        private readonly IRepository<RowWithTilesRecord> _partRowWithTilesRepository;

        public SecretServices(IRepository<SingleRowWithTilesRecord> singleRowWithTilesRepository, IRepository<RowWithTilesRecord> partRowWithTilesRepository)
        {
            _singleRowWithTilesRepository = singleRowWithTilesRepository;
            _partRowWithTilesRepository = partRowWithTilesRepository;
        }


        public RowWithTilesViewModel BuildEditorViewModel(RowWithTilesPart part, string itemsJson = "")
          {
              var ivm = new RowWithTilesViewModel();
              ivm.Rows = new List<SingleRowWithTilesRecord>();
              ivm.TilesJson = part.TilesJson;

              foreach (var item in part.Rows)
              {
                  ivm.Rows.Add(new SingleRowWithTilesRecord
                  {
                      Id = item.Id,
                      Title = item.Title,
                      Body = item.Body,
                      ExternalLink = item.ExternalLink,
                      LinkText = item.LinkText,
                      SortOrder = item.SortOrder,

                      Tile1Title = item.Tile1Title,
                      Tile1ExternalLink = item.Tile1ExternalLink,
                      Tile1Thumbnail = item.Tile1Thumbnail,

                      Tile2Title = item.Tile2Title,
                      Tile2ExternalLink = item.Tile2ExternalLink,
                      Tile2Thumbnail = item.Tile2Thumbnail,


                  });
              }
              ivm.Rows = ivm.Rows.OrderBy(x => x.SortOrder).ToList();

              if (!string.IsNullOrEmpty(itemsJson))
              {
                  ivm.TilesJson = itemsJson;
              }

              return ivm;
          }

          public void UpdateRowsWithTiles(ContentItem item, RowWithTilesViewModel model)
          {
              var rows = item.As<RowWithTilesPart>().Record;
              var allRows = rows.Rows;
              //rows. = model.RelatedLinksGroup;
              rows.TilesJson = model.TilesJson;

              var tilesJson = model.TilesJson;
              

              var oldLinks = _singleRowWithTilesRepository.Fetch(r => r.RowWithTilesRecord.Id == rows.Id).ToList();
              var updatedLinks = (List<SingleRowWithTilesRecord>)JsonConvert.DeserializeObject(tilesJson, typeof(List<SingleRowWithTilesRecord>));

              var deletedLinks = oldLinks.Where(x => !updatedLinks.Select(ui => ui.Id).Contains(x.Id));

              foreach (var deletedLink in deletedLinks)
              {
                  _singleRowWithTilesRepository.Delete(deletedLink);
              }

              foreach (var updatedLink in updatedLinks)
              {
                  if (updatedLink.Id == 0 || !allRows.Any())
                  {
                      updatedLink.RowWithTilesRecord = rows;
                      _singleRowWithTilesRepository.Create(updatedLink);
                  }
                  else if (updatedLink.Id > 0)
                  {
                      updatedLink.RowWithTilesRecord = rows;
                      _singleRowWithTilesRepository.Update(updatedLink); //NEVER HITS THIS
                  }
              }

              //Updating Links in the part manually. 
              //NOTE: Deleting this line will have a delay of 5 mins to update Links on the edit page
              rows.Rows = _singleRowWithTilesRepository.Fetch(r => r.RowWithTilesRecord.Id == rows.Id).ToList();
          }


    }
}