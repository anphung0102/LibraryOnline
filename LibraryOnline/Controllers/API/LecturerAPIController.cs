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
        public object GetEbook_Lecturer(int  id) 
        {
            var data = (from e in db.Ebooks
                        from s in db.Subject_Ebook
                        where e.sub_id == s.id && e.user_id == id
                        select new
                        {
                            id = e.id,
                            ebook_id = e.ebook_id,
                            title = e.title,
                            author = e.author,
                            year = e.year,
                            describe = e.describe,
                            filename = e.filename,
                            date_upload = e.date_upload,
                            sub_id = e.sub_id,
                            sub_name = s.name
                        }).ToList();

            return data;
        }
        //lấy ds essay 
        [Route("api/LecturerAPI/GetEssay_Lecturer")]
        [HttpGet]
        public object GetEssay_Lecturer(int id)
        {
            var data = (from e in db.Essays
                        from s in db.Subject_Essay
                        where e.sub_id == s.id && e.user_id == id
                        select new
                        {
                            id = e.id,
                            essay_id = e.essay_id,
                            title = e.title,
                            instructor = e.instructor,
                            executor1 = e.executor1,
                            executor2 = e.executor2,
                            course = e.course,
                            describe = e.describe,
                            filename = e.filename,
                            date_upload = e.date_upload,
                            sub_id = s.id,
                            sub_name = s.name
                        }).ToList();
            return data;
        }
        //lấy ds thesis
        [Route("api/LecturerAPI/GetThesis_Lecturer")]
        [HttpGet]
        public object GetThesis_Lecturer(int id)
        {
            var data = (from e in db.Theses
                        from s in db.Subject_Thesis
                        where e.sub_id == s.id && e.user_id == id
                        select new
                        {
                            id = e.id,
                            thesis_id = e.thesis_id,
                            title = e.title,
                            instructor = e.instructor,
                            executor1 = e.executor1,
                            executor2 = e.executor2,
                            course = e.cource,
                            describe = e.describe,
                            filename = e.filename,
                            date_upload = e.date_upload,
                            sub_id = s.id,
                            sub_name = s.name
                        }).ToList();
            return data;
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
