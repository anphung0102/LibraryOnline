﻿using LibraryOnline.Models;
using LibraryOnline.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Data.Entity;

namespace LibraryOnline.Controllers
{
    public class LoginInfo
    {
        public string User { get; set; }
        public string Pass { get; set; }
    }

    public class FileAPIController : ApiController
    {
        //
        private LibraryEntities db = new LibraryEntities();
        [Route("api/FileAPI/Login")]
        [HttpPost]
        public string Login(LoginInfo loginInfo)
        {
            using (LibraryEntities db = new LibraryEntities())
            {
                var role = db.Users.Where(x => x.username == loginInfo.User && x.password == loginInfo.Pass)
                          .Select(x => x.role_id).FirstOrDefault();
                var user_id = db.Users.Where(x => x.username == loginInfo.User && x.password == loginInfo.Pass)
                    .Select(x => x.id).FirstOrDefault();
                HttpContext.Current.Session["username"] = loginInfo.User;
                HttpContext.Current.Session["user_id"] = user_id;
                if (role == 1)
                    return "/Admin/Index";

            }

            return "";
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

        [Route("api/FileAPI/UploadFiles")]
        [HttpPost]
        public string UploadFiles()
        {
            var httpPostedFile = HttpContext.Current.Request.Files["fileInput"];
            if (httpPostedFile != null)
            {
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Upload/"), httpPostedFile.FileName);

                // Save the uploaded file to "UploadedFiles" folder
                httpPostedFile.SaveAs(fileSavePath);
            }
            var title = HttpContext.Current.Request["title"];
            var describe = HttpContext.Current.Request["describe"];
            var author = HttpContext.Current.Request["author"];
            var year = HttpContext.Current.Request["year"];
           
            string strExtexsion = Path.GetExtension(httpPostedFile.FileName).Trim();
            string a = "";
            if (strExtexsion == ".pdf")
            {
                using (LibraryEntities db = new LibraryEntities())
                {
                    db.Ebooks.Add(
                        new Ebook
                        {
                            title = title,
                            describe = describe,
                            author = author,
                            year = year,
                        });
                    db.SaveChanges();
                    a = "Thành công";
                }
            }
            else a = "lỗi";

            return a;

        }
        ////Lấy môn học của ebook
        ////Lấy môn học của ebook
        [Route("api/FileAPI/GetSubject")]
        [HttpGet]
        public IEnumerable<Subject> GetSubject()
        {
            return db.Subjects.ToList();
        }


        //Tạo môn học
        [Route("api/FileAPI/CreateSubject")]
        [HttpPost]
        public SubjectViewModel CreateSubject(SubjectViewModel subject)
        {
            using (LibraryEntities db = new LibraryEntities())
            {
                db.Subjects.Add(new Subject
                {
                    name = subject.Name,
                });
                db.SaveChanges();
            }

            return subject;
        }

        //lấy ebook
        [Route("api/FileAPI/Get")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            List<Ebook> ebook = new List<Ebook>();
            using (LibraryEntities db = new LibraryEntities())
            {
                ebook = db.Ebooks.OrderBy(a => a.title).ToList();
                HttpResponseMessage response;
                response = Request.CreateResponse(HttpStatusCode.OK, ebook);
                return response;
            }
        }
    }
}
