using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevOffice.Secret.Models;
using DevOffice.Secret.ViewModels;
using Orchard;

namespace DevOffice.Secret.Services
{
    public interface ISecretServices : IDependency
    {
        RowWithTilesViewModel BuildEditorViewModel(RowWithTilesPart part, string itemsJson);
    }
}