using System.Web.Mvc;

namespace MDAR_AcuityServiceAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "AlivePage";

            return View();
        }
    }
}