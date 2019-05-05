using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryOnline.Controllers
{
    public class ReadFileDetailsController : Controller
    {
        // GET: ReadFileDetails
        public ActionResult Index(string ebook_id)
        {
            ViewBag.Id = ebook_id;
            return View();
        }
        public ActionResult Essay(string essay_id)
        {
            ViewBag.Id = essay_id;
            return View();
        }
        public ActionResult Thesis(string thesis_id)
        {
            ViewBag.Id = thesis_id;
            return View();
        }
    }
}