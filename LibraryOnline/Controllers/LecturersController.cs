using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryOnline.Controllers
{
    public class LecturersController : Controller
    {
        // GET: Lecturters
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Manage(int ? id)
        {
            ViewBag.Id = id;
            return View();
        }
        public ActionResult ManageEssay(int? id)
        {
            ViewBag.Id = id;
            return View();
        }
        public ActionResult ManageThesis(int? id)
        {
            ViewBag.Id = id;
            return View();
        }
    }
}