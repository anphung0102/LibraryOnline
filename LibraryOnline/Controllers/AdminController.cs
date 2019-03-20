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
        // GET: Adminđdđkdkdk
        public ActionResult Admin()
        {
            //LibraryEntities db = new LibraryEntities();
            //List<Subject> sub = db.Subjects.ToList();
            //ViewBag.LoadSubject = sub;
            return View();
        }
        // hàm tạo tên file upload
        //private string RandomString(int size, bool lowerCase)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    char c;
        //    Random rand = new Random();
        //    for (int i = 0; i < size; i++)
        //    {
        //        c = Convert.ToChar(Convert.ToInt32(rand.Next(65, 87)));
        //        sb.Append(c);
        //    }
        //    if (lowerCase)
        //        return sb.ToString().ToLower();
        //    return sb.ToString();

        //}
        //public ActionResult Upload(Post post)
        //{
        //    // upload file
        //    foreach (var file in post.Files)
        //    {

        //        if (file != null && file.ContentLength > 0)
        //        {
        //            string temp = RandomString(10, true) + "-";
        //            var fileName = Path.GetFileName(file.FileName);
        //            var path = Path.Combine(Server.MapPath("~/Content/file"), temp + fileName);
        //            file.SaveAs(path);
        //        }
        //    }
        //    return RedirectToAction("/Index");
        //}
    }
}