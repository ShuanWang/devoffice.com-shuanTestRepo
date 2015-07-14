using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevOffice.Common.Models;
using DevOffice.Common.ViewModels;
using Orchard.Data;

namespace DevOffice.Common.Controllers
{
    public class ViewCountController: Controller
    {
            private readonly IRepository<ViewCountRecord> _viewCountRepository;

            public ViewCountController(IRepository<ViewCountRecord> viewCountRepository)
            {
                _viewCountRepository = viewCountRepository;
            }

        [System.Web.Http.HttpPost]
            public void Post(int itemId, string type)
            {
                //create a new record
                ViewCountRecord viewCount = new ViewCountRecord
                {
                    LinkId = itemId,
                    Date = DateTime.UtcNow,
                    Type = type
                };
                //add it to the repository
                _viewCountRepository.Create(viewCount);

            }
        
    }
}