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
    }
}