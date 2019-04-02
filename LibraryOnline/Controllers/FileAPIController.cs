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
using Newtonsoft.Json;

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
            if (sub != null)
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

        //lấy ebookpaging
        [Route("api/FileAPI/GetEbookPaging")]
        [HttpGet]
        public IEnumerable<Ebook> GetEbookPaging(int id)
        {
            return db.Ebooks.Where(x => x.sub_id == id).ToArray();
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

        //xoá ebook
        [Route("api/FileAPI/DeleteSubjectById")]
        [HttpPost]
        public string DeleteSubjectById(Subject_Ebook subject) 
        {
            var ebook = db.Ebooks.Where(x => x.sub_id == subject.id).ToList();
            foreach(var item in ebook)
            {
                db.Ebooks.Remove(item);
                db.SaveChanges();
            }
            var sub = db.Subject_Ebook.Where(x => x.id == subject.id).FirstOrDefault();
            MyHub.DeleteSubject(subject.id);
            db.Subject_Ebook.Remove(sub);
            db.SaveChanges();
          
            return "Xóa thành công";
        }
        //sửa ebook
        [Route("api/FileAPI/EditSubjectById")]
        [HttpPost]
        public string EditSubjectById(Subject_Ebook subject)
        {
            var sub = db.Subject_Ebook.Where(x => x.id == subject.id).FirstOrDefault();

        //[Route("api/FileAPI/DeleteSubjectById1")]
        //[HttpPost]
        //public string DeleteSubjectById1(Subject_Ebook sub) 
        //{
        //    return "Xóa thành công"+sub.name;
        //}
        [Route("api/FileAPI/EbookPaging")]
        [HttpGet]
        public IEnumerable<Ebook> EbookPaging(int id,[FromUri]PagingParameterModel pagingparametermodel)
        {

            // Return List of Customer  
            var source = db.Ebooks.Where(x => x.id == id).AsQueryable();

            // Get's No of Rows Count   
            int count = source.Count();

            // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
            int CurrentPage = pagingparametermodel.pageNumber;

            // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
            int PageSize = pagingparametermodel.pageSize;

            // Display TotalCount to Records to User  
            int TotalCount = count;

            // Calculating Totalpage by Dividing (No of Records / Pagesize)  
            int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            // Returns List of Customer after applying Paging   
            var items = source.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            // if CurrentPage is greater than 1 means it has previousPage  
            var previousPage = CurrentPage > 1 ? "Yes" : "No";

            // if TotalPages is greater than CurrentPage means it has nextPage  
            var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

            // Object which we are going to send in header   
            var paginationMetadata = new
            {
                totalCount = TotalCount,
                pageSize = PageSize,
                currentPage = CurrentPage,
                totalPages = TotalPages,
                previousPage,
                nextPage
            };

            // Setting Header  
            HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
            // Returing List of Customers Collections  
            return items;

        }
    }
}
