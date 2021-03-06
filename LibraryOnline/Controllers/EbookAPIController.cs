﻿using LibraryOnline.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Web;
using System.IO;
using System.Text;

namespace LibraryOnline.Controllers
{
    public class EbookAPIController : ApiController
    {
        private LibraryOnlineFinalEntities db = new LibraryOnlineFinalEntities();
        //lấy môn học
        [Route("api/EbookAPI/GetSubjectEbook")]
        [HttpGet]
        public IEnumerable<Subject_Ebook> GetSubjectEbook()
        {
            var a = db.Subject_Ebook.ToList();
            return a;
        }

        //sửa ebook
        [Route("api/EbookAPI/EditSubjectById")]
        [HttpPost]
        public SubjectCreationResult EditSubjectById(Subject_Ebook subject)
        {
            var check = db.Subject_Ebook.Where(x => x.id == subject.id).FirstOrDefault();
            if(check == null)
            {
                return new SubjectCreationResult
                {
                    IsSuccess = false
                };
            }
            else
            {
                var sub = db.Subject_Ebook.Where(x => (x.id != subject.id) && 
                    (x.subebook_id == subject.subebook_id || x.name == subject.name)).FirstOrDefault();
                if (sub != null)
                {
                    return new SubjectCreationResult
                    {
                        IsSuccess = false
                    };
                }
                else
                {
                    check.subebook_id = subject.subebook_id;
                    check.name = subject.name;
                    db.SaveChanges();
                    return new SubjectCreationResult
                    {
                        IsSuccess = true,
                        Subebook_Id = check.subebook_id,
                        Name = check.name,

                    };
                }
            }


            //var sub = db.Subject_Ebook.Where(x => x.id == subject.id).FirstOrDefault();

            //if (sub == null)
            //{
            //    return new SubjectCreationResult
            //    {
            //        IsSuccess = false
            //    };
            //}
            //else
            //{
            //    sub.subebook_id = subject.subebook_id;
            //    sub.name = subject.name;
            //    db.SaveChanges();
            //    return new SubjectCreationResult
            //    {
            //        IsSuccess = true,
            //        Subebook_Id = sub.subebook_id,
            //        Name = sub.name,

            //    };
            //}
        }

        ////Lấy môn học của ebook
        [Route("api/EbookAPI/LoadSubjectEbook")]
        [HttpGet]
        public IEnumerable<Subject_Ebook> LoadSubjectEbook()
        {
            var a = db.Subject_Ebook.ToList();
            return a;
        }

        //xoá ebook
        [Route("api/EbookAPI/DeleteSubjectById")]
        [HttpPost]
        public string DeleteSubjectById(Subject_Ebook subject)
        {
            var ebook = db.Ebooks.Where(x => x.sub_id == subject.id).ToList();
            foreach (var item in ebook)
            {
                db.Ebooks.Remove(item);
                db.SaveChanges();
            }
            var rate = db.RateStars.Where(x => x.sub_id == subject.id).ToList();
            foreach (var item in rate)
            {
                db.RateStars.Remove(item);
                db.SaveChanges();
            }
            var time = db.Times.Where(x => x.sub_id == subject.id).ToList();
            foreach (var item in time)
            {
                db.Times.Remove(item);
                db.SaveChanges();
            }
            var search = db.SearchFiles.Where(x => x.sub_id == subject.id).ToList();
            foreach (var item in search)
            {
                db.SearchFiles.Remove(item);
                db.SaveChanges();
            }
            var sub = db.Subject_Ebook.Where(x => x.id == subject.id).FirstOrDefault();
            db.Subject_Ebook.Remove(sub);
            db.SaveChanges();

            return "Xóa thành công";
        }
        ////lấy file 
        //[Route("api/EbookAPI/GetFile")]
        //[HttpGet]
        //public IEnumerable<Ebook> GetFile()
        //{
        //    //var data = db.Ebooks.Include(x=>x.User).ToList();
        //    var data = db.Ebooks.ToList();
        //    return data;

        //}
        //lấy file 
        [Route("api/EbookAPI/GetFile")]
        [HttpGet]
        public object GetFile()
        {
            var data = db.Ebooks.Include(x=>x.User).Select(x=>new {
                id = x.id,
                title = x.title,
                author = x.author,
                year = x.year,
                describe = x.describe,
                filename = x.filename,
                date_upload = x.date_upload,
                username = x.User.username
            }).ToList();
            
            return data;
      }
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder sb = new StringBuilder();
            char c;
            Random rand = new Random();
            for (int i = 0; i < size; i++)
            {
                c = Convert.ToChar(Convert.ToInt32(rand.Next(65, 87)));
                sb.Append(c);
            }
            if (lowerCase)
                return sb.ToString().ToLower();
            return sb.ToString();

        }

        //sửa file 
        [Route("api/EbookAPI/EditFileById")]
        [HttpPost]
        public string EditFileById() 
        {
            var id = HttpContext.Current.Request["id"];
            var title = HttpContext.Current.Request["title"];
            var describe = HttpContext.Current.Request["describe"];
            var author = HttpContext.Current.Request["author"];
            var year = HttpContext.Current.Request["year"];
            var userid = HttpContext.Current.Request["userid"];
            var date_upload = DateTime.Now;
            int _id = Convert.ToInt32(id);
            var file = db.Ebooks.Where(x => x.id == _id).FirstOrDefault();

            if (file != null)
            {
                var httpPostedFile = HttpContext.Current.Request.Files["fileInput"];//lấy file
                if (httpPostedFile != null)
                {
                    //đường dẫn lưu file
                    string temp = RandomString(10, true) + "-";
                    var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Upload/"), httpPostedFile.FileName);//tên file
                    string strExtexsion = Path.GetExtension(httpPostedFile.FileName).Trim();//lấy đuôi file                                                                                                                //lưu file vào đường dẫn
                    httpPostedFile.SaveAs(fileSavePath);

                    if (strExtexsion == ".pdf")//chỉ cho up pdf
                    {
                        file.title = title;
                        file.author = author;
                        file.describe = describe;
                        file.year = year;
                        file.filename = httpPostedFile.FileName;
                        file.date_upload = DateTime.Now;
                       // MyHub.EditFileEbook(file.title,file.author,file.describe,file.year,file.filename,file.date_upload);
                        db.SaveChanges();
                        return "Sửa thành công";
                    }
                    else
                    {
                        return "Sửa không thành công";
                    }
                }
                else
                {
                    file.title = title;
                    file.author = author;
                    file.describe = describe;
                    file.year = year;
                    file.date_upload = DateTime.Now;
                    db.SaveChanges();
                    return "Sửa thành công";
                }

            }
            return "lỗi";
        }


        //xoá file 
        [Route("api/EbookAPI/DeleteFileById")]
        [HttpPost]
        public string DeleteFileById(Subject_Ebook subject)
        {
            var ebook = db.Ebooks.Where(x => x.id == subject.id).ToList();
            foreach (var item in ebook)
            {
                db.Ebooks.Remove(item);
                db.SaveChanges();
            }
            return "Xóa thành công";
        }

        //xoá ebook
        [Route("api/EbookAPI/DeleteFileUploadById")]
        [HttpPost]
        public string DeleteFileUploadById(int id)
        {
            //var ebook = db.Ebooks.Where(x => x.sub_id == subject.id).ToList();
            //foreach (var item in ebook)
            //{
            //    db.Ebooks.Remove(item);
            //    db.SaveChanges();
            //}
            var eb = db.Ebooks.Where(x => x.id == id ).FirstOrDefault();
            var search = db.SearchFiles.Where(x => x.book_id == eb.ebook_id).FirstOrDefault();
            var rate = db.RateStars.Where(x => x.book_id == eb.ebook_id).ToList();
            var time = db.Times.Where(x => x.bookid == eb.ebook_id).ToList();
            if(eb!=null)
            {
                db.Ebooks.Remove(eb);
                db.SaveChanges();
            }
            if(search != null)
            {
                db.SearchFiles.Remove(search);
                db.SaveChanges();
            }
            if (rate != null)
            {
                foreach (var item in rate)
                {
                    db.RateStars.Remove(item);
                    db.SaveChanges();
                }

            }
            if (time != null)
            {
                foreach (var item in time)
                {
                    db.Times.Remove(item);
                    db.SaveChanges();
                }
                
            }
            return "Xóa thành công";
        }
    }
}
