using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryOnline.Controllers
{
    public class DetailsController : Controller
    {
        // GET: Detail
        public ActionResult Details(int? id)
        {
            ViewBag.Id = id;
            return View();
        }
    }
}