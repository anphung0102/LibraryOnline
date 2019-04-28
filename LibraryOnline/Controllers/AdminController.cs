using LibraryOnline.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LibraryOnline.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Admin()
        {
            return View();
        }
        // GET: Details
        public ActionResult Details(int? id)
        {
            ViewBag.Id = id;
            return View();
        }
        public ActionResult ManageUploadEbook()
        {
            return View();
        }
    }
}