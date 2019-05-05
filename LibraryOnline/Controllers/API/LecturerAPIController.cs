using LibraryOnline.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace LibraryOnline.Controllers.API
{
    public class LecturerAPIController : ApiController
    {
        private LibraryOnlineFinalEntities db = new LibraryOnlineFinalEntities();
        //lấy ds ebook 
        [Route("api/LecturerAPI/GetEbook_Lecturer")]
        [HttpGet]
        public IEnumerable<Ebook> GetEbook_Lecturer(int  id) 
        {
            return db.Ebooks.Where(x => x.user_id == id).ToList();
        }
        //lấy ds essay 
        [Route("api/LecturerAPI/GetEssay_Lecturer")]
        [HttpGet]
        public IEnumerable<Essay> GetEssay_Lecturer(int id)
        {
            return db.Essays.Where(x => x.user_id == id).ToList();
        }
        //lấy ds thesis
        [Route("api/LecturerAPI/GetThesis_Lecturer")]
        [HttpGet]
        public IEnumerable<Thesis> GetThesis_Lecturer(int id)
        {
            return db.Theses.Where(x => x.user_id == id).ToList();
        }
        //Tạo môn học trong Ebook 
        //[Route("api/LecturerAPI/UploadEbooks")]
        //[HttpPost]
        //public EbookCreationResult UploadEbooks ()
        //{
        //    var httpPostedFile = HttpContext.Current.Request.Files["fileInput"];
        //    if (httpPostedFile != null)
        //    {
        //        //đường dẫn lưu file
        //        var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Upload/"), httpPostedFile.FileName);//tên file
        //        //lưu file vào đường dẫn
        //        httpPostedFile.SaveAs(fileSavePath);
        //    }

        //    var title = HttpContext.Current.Request["title"];
        //    var describe = HttpContext.Current.Request["describe"];
        //    var author = HttpContext.Current.Request["author"];
        //    var year = HttpContext.Current.Request["year"];
        //    var userid = HttpContext.Current.Request["userid"];
        //    var subid = HttpContext.Current.Request["subid"];
        //    var username = HttpContext.Current.Request["username"];
        //    var date_upload = DateTime.Now;
        //    int user_id = Convert.ToInt32(userid);
        //    int sub_id = Convert.ToInt32(subid);
        //    string strExtexsion = Path.GetExtension(httpPostedFile.FileName).Trim();//lấy đuôi file
        //    var sub = db.Ebooks.Where(x => x.title.Equals(title)).FirstOrDefault();//này làm gì làm lấy so sánh đồ giống cái m làm t sửa lại

        //    if (sub != null)
        //    {
        //        return new EbookCreationResult
        //        {
        //            IsSuccess = false
        //        };
        //    }
        //    else
        //    {
        //        if (strExtexsion == ".pdf")//chỉ cho up pdf
        //        {
        //            db.Ebooks.Add(new Ebook
        //            {
        //                ebook_id = "",
        //                title = title,
        //                author = author,
        //                year = year,
        //                describe = describe,
        //                filename = httpPostedFile.FileName,
        //                date_upload = DateTime.Now,
        //                user_id = user_id,
        //                sub_id = sub_id
        //            });
        //            db.SaveChanges();
        //        }

        //        var loadebook = db.Ebooks.Where(x => x.title.Equals(title)).FirstOrDefault();

        //        //MyHub.Post(sub_ebook.id, sub_ebook.name);
        //        return new EbookCreationResult
        //        {
        //            IsSuccess = true,
        //            Id = loadebook.id,
        //            Ebook_Id= loadebook.ebook_id,
        //            Title = loadebook.title,
        //            Describe = loadebook.describe,
        //            Author = loadebook.author,
        //            Year = loadebook.year,
        //            FileName =loadebook.filename,
        //            //Date_Upload = loadebook.date_upload
        //        };
        //    }
        //}
    }
}
