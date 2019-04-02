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
    }
}