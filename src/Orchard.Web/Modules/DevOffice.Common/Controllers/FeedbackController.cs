using System.Web.Mvc;
using DevOffice.Common.Models;
using DevOffice.Common.Services;

namespace DevOffice.Common.Controllers
{
    public class FeedbackController : Controller {

        private readonly ICommonDataService _commonDataService;

        public FeedbackController(
            ICommonDataService commonDataService) {
            _commonDataService = commonDataService;
        }

        [HttpPost]
        public ActionResult PostFeedback(FeedbackInformationRecord feedback) {
            if (feedback != null) {
                _commonDataService.CreateFeedbackRecord(feedback);
                return Json(new { Message = "" });
            }
            return Json(new { Message = "Failed" });
        }

    }
}