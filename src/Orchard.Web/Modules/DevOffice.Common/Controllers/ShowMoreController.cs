using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using DevOffice.Common.Services;

namespace DevOffice.Common.Controllers
{
    public class ShowMoreController : Controller {

        private readonly ICommonDataService _commonDataService;
        public ShowMoreController(ICommonDataService commonDataService) {
            _commonDataService = commonDataService;
        }

        public ActionResult GetArticles(int pageNumber = 0, int count = 20) {
            var articles = _commonDataService.GetArticles(pageNumber, count);
            return View("Parts/NewArticlesWidgetList", articles);
        }


        // GET: ShowMore
        public ActionResult Index()
        {
            return View();
        }
    }
}