using System;
using System.Web.Mvc;
using System.Web.WebPages;
using MDAR_AcuityServiceAPI.Classes;
using MDAR_AcuityServiceAPI.Utils;

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