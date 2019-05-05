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
        public ActionResult Details(string ebook_id) 
        {
            ViewBag.ID = ebook_id; 
            return View();
        }
        public ActionResult DetailsEssay(string essay_id)
        {
            ViewBag.ID = essay_id;
            return View();
        }
        public ActionResult DetailsThesis(string thesis_id)
        {
            ViewBag.ID = thesis_id;
            return View();
        }
    }
}