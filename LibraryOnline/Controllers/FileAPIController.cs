using LibraryOnline.Models;
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
                    return "/Admin/Admin";

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

     
            //Upload file cho Ebook mình làm trc cái ebook thoi mấy cái kia xong copy qua
        [Route("api/FileAPI/UploadFiles")]
        [HttpPost]
        public string UploadFiles()
        {
            var httpPostedFile = HttpContext.Current.Request.Files["fileInput"];//lấy file
            if (httpPostedFile != null)
            {
                //đường dẫn lưu file
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Upload/"), httpPostedFile.FileName);//tên file
                //lưu file vào đường dẫn
                httpPostedFile.SaveAs(fileSavePath);
            }
            
            var title = HttpContext.Current.Request["title"];
            var describe = HttpContext.Current.Request["describe"];
            var author = HttpContext.Current.Request["author"];
            var year = HttpContext.Current.Request["year"];
            var userid = HttpContext.Current.Request["userid"];
            var subid = HttpContext.Current.Request["subid"]; 
            var date_upload = DateTime.Now;
            int user_id = Convert.ToInt32(userid);
            int sub_id = Convert.ToInt32(subid);
            //mai m làm thêm cái ngày up là Datetime.Now gì đó
            string strExtexsion = Path.GetExtension(httpPostedFile.FileName).Trim();//lấydduooio file
            string a = "";
            if (strExtexsion == ".pdf")//chỉ cho up pdf
            {
                using (LibraryEntities db = new LibraryEntities())
                {
                    //Add vô bảng ebook 
                    db.Ebooks.Add(
                        new Ebook
                        {
                            title = title,
                            describe = describe,
                            author = author,
                            year = year,
                            filename = httpPostedFile.FileName,
                            date_upload = date_upload,
                            user_id = user_id,
                            sub_id = sub_id,
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
        [Route("api/FileAPI/GetSubjectEbook")]
        [HttpGet]
        public IEnumerable<Subject_Ebook> GetSubjectEbook() 
        {
            return db.Subject_Ebook.ToList();
        }


        //Tạo môn học
        [Route("api/FileAPI/CreateSubject")]
        [HttpPost]
        public SubjectViewModel CreateSubject(SubjectViewModel subject)
        {
            using (LibraryEntities db = new LibraryEntities())
            {
                db.Subject_Ebook.Add(new Subject_Ebook
                {
                    name = subject.Name,
                });
                db.SaveChanges();
            }

            return subject;
        }

        //lấy ebook
        [Route("api/FileAPI/GetEbook")]
        [HttpGet]
        public IEnumerable<Ebook> GetEbook(int id) 
        {
             return db.Ebooks.Where(x=>x.sub_id == id).OrderBy(a => a.title).ToList();
        }

        ////lấy essay
        //[Route("api/FileAPI/GetEssay")]
        //[HttpGet]
        //public HttpResponseMessage GetEssay()
        //{
        //    List<Essay> essay = new List<Essay>();
        //    using (LibraryEntities db = new LibraryEntities())
        //    {
        //        essay = db.Essays.OrderBy(a => a.title).ToList();
        //        HttpResponseMessage response;
        //        response = Request.CreateResponse(HttpStatusCode.OK, essay);
        //        return response;
        //    }
        //}
    }
}
