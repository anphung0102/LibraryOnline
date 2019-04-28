using LibraryOnline.Models;
using Newtonsoft.Json;
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
    public class AdminAPIController : ApiController
    {
        private LibraryOnlineFinalEntities db = new LibraryOnlineFinalEntities();
        //Upload file cho Ebook 
        [Route("api/AdminAPI/UploadFiles")]
        [HttpPost]
        public HttpResponseMessage UploadFiles()
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
            var username = HttpContext.Current.Request["username"];
            var date_upload = DateTime.Now;
            int user_id = Convert.ToInt32(userid);
            int sub_id = Convert.ToInt32(subid);
            string strExtexsion = Path.GetExtension(httpPostedFile.FileName).Trim();//lấy đuôi file
            
            if (strExtexsion == ".pdf")//chỉ cho up pdf
            {

                using (LibraryOnlineFinalEntities db = new LibraryOnlineFinalEntities())
                {
                    //Add vô bảng ebook 
                    db.Ebooks.Add(
                        new Ebook
                        {
                            ebook_id = "",
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
                    var book_id = db.Ebooks.OrderByDescending(x => x.id).Select(x=>x.ebook_id).FirstOrDefault();
                    db.SearchFiles.Add(
                      new SearchFile
                      {
                          book_id = book_id,
                          title = title,
                          author = author,
                          year = year,
                          instructor = "",
                          executor1 = "",
                          executor2 = "",
                          describe = describe,
                          filename = httpPostedFile.FileName,
                          date_upload = date_upload,
                          user_id = user_id,
                          sub_id = sub_id,
                          username = username
                      });
                    db.SaveChanges();//lưu dât thôi cái này t chưa chạy t mới test gửi data từ  ajax qua thôi
                    var user = db.Users.Where(x => x.id == user_id).Select(x => x.username).FirstOrDefault();
                    var subject = db.Subject_Ebook.Where(x => x.id == sub_id).Select(x => x.name).FirstOrDefault();
                    var fileinfo = db.Ebooks.OrderByDescending(x => x.id).FirstOrDefault();
                    var date_up = date_upload.ToString("MM/dd/yyyy");
                    MyHub.PostFileEbook(fileinfo.id, fileinfo.title, fileinfo.author, fileinfo.describe,
                        fileinfo.year, fileinfo.filename, date_up, user, subject);
                 
                    return Request.CreateResponse("Thành công");
                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi!!!");

        }


        ////Lấy môn học của ebook
        ////Lấy môn học của ebook
        [Route("api/AdminAPI/GetSubjectEbook")]
        [HttpGet]
        public IEnumerable<Subject_Ebook> GetSubjectEbook()
        {
            var a = db.Subject_Ebook.ToList();
            return a;
        }

        
        //Tạo môn học trong Ebook
        [Route("api/AdminAPI/CreateSubject")]
        [HttpPost]
        public SubjectCreationResult CreateSubject(SubjectViewModel subject)
        {
            var sub = db.Subject_Ebook.Where(x => x.name.Equals(subject.Name)).FirstOrDefault();
            if (sub != null)
            {
                return new SubjectCreationResult
                {
                    IsSuccess = false
                };
            }
            else
            {
                db.Subject_Ebook.Add(new Subject_Ebook
                {
                    subebook_id = "",
                    name = subject.Name,
                });
                db.SaveChanges();
                var sub_ebook = db.Subject_Ebook.Where(x => x.name.Equals(subject.Name)).FirstOrDefault();

                //MyHub.Post(sub_ebook.id, sub_ebook.name);
                return new SubjectCreationResult
                {
                    IsSuccess = true,
                    Id = sub_ebook.id,
                    Name = sub_ebook.name
                };
            }
        }

        //lấy ebook
        [Route("api/AdminAPI/GetEbook")]
        [HttpGet]
        public IEnumerable<Ebook> GetEbook(int id)
        {
            return db.Ebooks.Where(x => x.sub_id == id).ToList();
        }

        //lấy ebookpaging
        [Route("api/AdminAPI/GetEbookPaging")]
        [HttpGet]
        public IEnumerable<Ebook> GetEbookPaging(int id)
        {
            return db.Ebooks.Where(x => x.sub_id == id).ToArray();
        }
        //lấy ebook
        [Route("api/AdminAPI/GetEbookDetail")]
        [HttpGet]
        public IEnumerable<Ebook> GetEbookDetail(int id)
        {
            return db.Ebooks.Where(x => x.id == id).ToList();
        }
        //lấy file theo id
        [Route("api/AdminAPI/GetFileById")]
        [HttpGet]
        public Ebook GetFileById(string ebook_id)
        {
            return db.Ebooks.Where(x => x.ebook_id == ebook_id).FirstOrDefault();
        }

        //xoá ebook
        [Route("api/AdminAPI/DeleteSubjectById")]
        [HttpPost]
        public string DeleteSubjectById(Subject_Ebook subject)
        {
            var ebook = db.Ebooks.Where(x => x.sub_id == subject.id).ToList();
            foreach (var item in ebook)
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

        [Route("api/AdminAPI/DeleteSubjectById1")]
        [HttpPost]
        public string DeleteSubjectById1(Subject_Ebook sub)
        {
            return "Xóa thành công" + sub.name;
        }
        // lấy ds ebook quyền admin
        [Route("api/AdminAPI/GetEbookByAdmin")]
        [HttpGet]
        public IEnumerable<Ebook> GetEbookByAdmin()
        {
           
            return db.Ebooks.ToList(); ;
        }
        // Tiểu luận
        // lấy ds môn tiểu luận
        [Route("api/AdminAPI/GetSubjectEssay")]
        [HttpGet]
        public IEnumerable<Subject_Essay> GetSubjectEssay()
        {

            return db.Subject_Essay.ToList(); 
        }

        // lấy ds tiểu luận
        [Route("api/AdminAPI/GetEssay")]
        [HttpGet]
        public IEnumerable<Essay> GetEssay(int id)
        {
         
                var a= db.Essays.Where(x => x.sub_id == id).ToList();
            return a;
        }

        // lấy ds tiểu luận phân trang
        [Route("api/AdminAPI/GetEssayPaging")]
        [HttpGet]
        public IEnumerable<Essay> GetEssayPaging(int id)
        {
            return db.Essays.Where(x => x.sub_id == id).ToArray();
        }
        //Khóa luận
        // lấy ds môn khóa luận
        [Route("api/AdminAPI/GetSubjectThesis")]
        [HttpGet]
        public IEnumerable<Subject_Thesis> GetSubjectThesis()
        {

            return db.Subject_Thesis.ToList();
        }
        [Route("api/AdminAPI/GetThesis")]
        [HttpGet]
        public IEnumerable<Thesis> GetThesis(int id)
        {

            var a = db.Theses.Where(x => x.sub_id == id).ToList();
            return a;
        }

        // lấy ds tiểu luận phân trang
        [Route("api/AdminAPI/GetThesisPaging")]
        [HttpGet]
        public IEnumerable<Thesis> GetThesisPaging(int id)
        {
            return db.Theses.Where(x => x.sub_id == id).ToArray();
        }
        //lưu đánh giá sách
        [Route("api/AdminAPI/SaveRate")]
        [HttpPost]
        public RateStarResult SaveRate(RateStarModel ratemodel)
        {
            var temp = db.RateStars.Where(x => x.book_id == ratemodel.BookId &&
                                   x.user_id == ratemodel.UserId).FirstOrDefault();
            if (temp != null)
            {
                temp.rate = ratemodel.Rate;
                db.SaveChanges();
                return new RateStarResult
                {
                    IsSuccess = true
                };
            }
            else
            {
                db.RateStars.Add(new RateStar
                {
                  book_id = ratemodel.BookId,
                  usename = ratemodel.Username,
                  user_id = ratemodel.UserId,
                  rate = ratemodel.Rate
                });
                db.SaveChanges();
               
                return new RateStarResult
                {
                    IsSuccess = true
                };
            }
        }


        //Upload Khóa Luận
        [Route("api/AdminAPI/UploadFilesThesis")]
        public HttpResponseMessage UploadFilesThesis()
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
            var instructor = HttpContext.Current.Request["instructor"];
            var student1 = HttpContext.Current.Request["student1"];
            var student2 = HttpContext.Current.Request["student2"]; 
            var year = HttpContext.Current.Request["year"];
            var userid = HttpContext.Current.Request["userid"];
            var subid = HttpContext.Current.Request["subid"];
            var date_upload = DateTime.Now;
            int user_id = Convert.ToInt32(userid);
            int sub_id = Convert.ToInt32(subid);
            string strExtexsion = Path.GetExtension(httpPostedFile.FileName).Trim();//lấy đuôi file

            if (strExtexsion == ".pdf")//chỉ cho up pdf
            {

                using (LibraryOnlineFinalEntities db = new LibraryOnlineFinalEntities())
                {
                    //Add vô bảng ebook 
                    db.Theses.Add(
                        new Thesis
                        {
                            thesis_id = "",
                            title = title,
                            describe = describe,
                            instructor = instructor,
                            executor1 = student1,
                            executor2 = student2,
                            filename = httpPostedFile.FileName,
                            date_upload = date_upload,
                            user_id = user_id,
                            sub_id = sub_id,
                        });
                    db.SaveChanges();
                    var book_id = db.Theses.OrderByDescending(x => x.id).Select(x => x.thesis_id).FirstOrDefault();
                    
                    db.SearchFiles.Add(
                      new SearchFile
                      {
                          book_id = book_id,
                          title = title,
                          author = "",
                          year = "",
                          instructor = instructor,
                          executor1 = student1,
                          executor2 = student2,
                          describe = describe,
                          filename = httpPostedFile.FileName,
                          date_upload = date_upload,
                          user_id = user_id,
                          sub_id = sub_id
                      });
                    db.SaveChanges();//lưu dât thôi cái này t chưa chạy t mới test gửi data từ  ajax qua thôi
                    var user = db.Users.Where(x => x.id == user_id).Select(x => x.username).FirstOrDefault();
                    var subject = db.Subject_Thesis.Where(x => x.id == sub_id).Select(x => x.name).FirstOrDefault();
                    var fileinfo = db.Theses.OrderByDescending(x => x.id).FirstOrDefault();
                    var date_up = date_upload.ToString("dd/MM/yyyy");
                    MyHub.PostFileThesis(fileinfo.id, fileinfo.title, fileinfo.instructor, fileinfo.executor1,
                        fileinfo.executor1, fileinfo.describe, fileinfo.filename, date_up, user, subject);

                    return Request.CreateResponse("Thành công");
                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi!!!");

        }


    }
}
