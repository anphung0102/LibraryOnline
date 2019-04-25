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
    }
}