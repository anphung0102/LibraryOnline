using LibraryOnline.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace LibraryOnline.Controllers
{
    public class UploadAPIController : ApiController
    {
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
        //[Route("api/UploadAPI/UploadEbook")]
        //[HttpPost]
        //public Ebook UploadEbook(Ebook ebook, HttpPostedFileBase file)
        //{
        //    //var fileName = Path.GetFileName(ebook.filename);
        //    //var path = Path.Combine(Server.MapPath("~/Assets/fileupload/user"),fileName);
        //    //eSaveAs(path);
        //    //ebook.filename = fileName;
        //    using (LIBRARYEntities1 db = new LIBRARYEntities1())
        //    {

        //            if (file != null && file.ContentLength > 0)
        //            {
        //                string temp = RandomString(10, true) + "-";
        //                var fileName = Path.GetFileName(file.FileName);
        //                var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/file/"), temp + fileName);
        //                file.SaveAs(path);
        //                ebook.filename = temp + fileName;
        //            }

        //        db.Ebooks.Add(ebook);
        //        db.SaveChanges();
        //    }

        //    return ebook;
        //}
        //cách nay làm gần đúng
        //lay dau mà dc @@!:D e đóng hét r à

        //Lấy môn học của ebook
        //[Route("api/UploadAPI/GetSubject")]
        //[HttpGet]
        //public List<Subject> GetSubject()
        //{
        //    List<Subject> sub = new List<Subject>();
        //    using (LibraryEntities db = new LibraryEntities())
        //    {
        //        sub = db.Subjects.OrderBy(m => m.name).ToList();// v thôi

        //    }
        //    return sub;
        //}
    }
}
