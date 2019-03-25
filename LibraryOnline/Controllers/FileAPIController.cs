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


        //Upload file cho Ebook 
        [Route("api/FileAPI/UploadFiles")]
        [HttpPost]
        public string UploadFiles()
        {
            var httpPostedFile = HttpContext.Current.Request.Files["fileInput"];//lấy file
            if (httpPostedFile != null)
            {
                //đường dẫn lưu file
                string temp = RandomString(10, true) + "-";
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Upload/"), temp + httpPostedFile.FileName);//tên file
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
            string strExtexsion = Path.GetExtension(httpPostedFile.FileName).Trim();//lấy đuôi file
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
                    db.SaveChanges();//lưu dât thôi cái này t chưa chạy t mới test gửi data từ  ajax qua thôi
                   
                    var fileinfo = db.Ebooks.OrderByDescending(x => x.id).FirstOrDefault();
                    var date_up = fileinfo.date_upload.Value.ToString("MM/dd/yyyy");
                    MyHub.PostFileEbook(fileinfo.id, fileinfo.title, fileinfo.author, fileinfo.describe,
                        fileinfo.year, fileinfo.filename, date_up);
                    a = "Thành công";//đc chưa m// oke đc đó còi còn thiếu trường nào thêm vô thôi
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
            var a = db.Subject_Ebook.ToList();
            return a;
        }


        //Tạo môn học trong Ebook
        [Route("api/FileAPI/CreateSubject")]
        [HttpPost]
        public string CreateSubject(SubjectViewModel subject)
        {
            var sub = db.Subject_Ebook.Where(x => x.name.Equals(subject.Name)).FirstOrDefault();
            if(sub != null)
            {
                return "Tên môn đã tồn tại! Vui lòng đặt tên khác.";
            }
            else
            {
                db.Subject_Ebook.Add(new Subject_Ebook
                {
                    name = subject.Name,
                });
                db.SaveChanges();
                var sub_ebook = db.Subject_Ebook.Where(x => x.name.Equals(subject.Name)).FirstOrDefault();
                
                MyHub.Post(sub_ebook.id, sub_ebook.name);
                return "Tạo môn thành công.";
            }
        }

        //lấy ebook
        [Route("api/FileAPI/GetEbook")]
        [HttpGet]
        public IEnumerable<Ebook> GetEbook(int id) 
        {
             return db.Ebooks.Where(x=>x.sub_id == id).ToList();
        }
        //lấy ebook
        [Route("api/FileAPI/GetEbookDetail")]
        [HttpGet]
        public IEnumerable<Ebook> GetEbookDetail(int id)
        {
            return db.Ebooks.Where(x => x.id == id).ToList();
        }
        //lấy file theo id
        [Route("api/FileAPI/GetFileById")]
        [HttpGet]
        public Ebook GetFileById(int id) 
        {
            return db.Ebooks.Where(x => x.id == id).FirstOrDefault();
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
