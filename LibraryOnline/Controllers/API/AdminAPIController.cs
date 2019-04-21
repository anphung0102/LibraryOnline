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
        private LibraryOnlineEntities db = new LibraryOnlineEntities();
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
            var date_upload = DateTime.Now;
            int user_id = Convert.ToInt32(userid);
            int sub_id = Convert.ToInt32(subid);
            string strExtexsion = Path.GetExtension(httpPostedFile.FileName).Trim();//lấy đuôi file
            
            if (strExtexsion == ".pdf")//chỉ cho up pdf
            {

                using (LibraryOnlineEntities db = new LibraryOnlineEntities())
                {
                    //Add vô bảng ebook 
                    db.EBOOKS.Add(
                        new EBOOK
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
                    db.SaveChanges();//lưu dât thôi cái này t chưa chạy t mới test gửi data từ  ajax qua thôi
                    var user = db.USERS.Where(x => x.id == user_id).Select(x => x.username).FirstOrDefault();
                    var subject = db.SUBJECTEBOOKs.Where(x => x.id == sub_id).Select(x => x.name).FirstOrDefault();
                    var fileinfo = db.EBOOKS.OrderByDescending(x => x.id).FirstOrDefault();
                    var date_up = fileinfo.date_upload.Value.ToString("MM/dd/yyyy");
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
        public IEnumerable<SUBJECTEBOOK> GetSubjectEbook()
        {
            var a = db.SUBJECTEBOOKs.ToList();
            return a;
        }

        
        //Tạo môn học trong Ebook
        [Route("api/AdminAPI/CreateSubject")]
        [HttpPost]
        public SubjectCreationResult CreateSubject(SubjectViewModel subject)
        {
            var sub = db.SUBJECTEBOOKs.Where(x => x.name.Equals(subject.Name)).FirstOrDefault();
            if (sub != null)
            {
                return new SubjectCreationResult
                {
                    IsSuccess = false
                };
            }
            else
            {
                db.SUBJECTEBOOKs.Add(new SUBJECTEBOOK
                {
                    subebook_id = "",
                    name = subject.Name,
                });
                db.SaveChanges();
                var sub_ebook = db.SUBJECTEBOOKs.Where(x => x.name.Equals(subject.Name)).FirstOrDefault();

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
        public IEnumerable<EBOOK> GetEbook(int id)
        {
            return db.EBOOKS.Where(x => x.sub_id == id).ToList();
        }

        //lấy ebookpaging
        [Route("api/AdminAPI/GetEbookPaging")]
        [HttpGet]
        public IEnumerable<EBOOK> GetEbookPaging(int id)
        {
            return db.EBOOKS.Where(x => x.sub_id == id).ToArray();
        }
        //lấy ebook
        [Route("api/AdminAPI/GetEbookDetail")]
        [HttpGet]
        public IEnumerable<EBOOK> GetEbookDetail(int id)
        {
            return db.EBOOKS.Where(x => x.id == id).ToList();
        }
        //lấy file theo id
        [Route("api/AdminAPI/GetFileById")]
        [HttpGet]
        public EBOOK GetFileById(int id)
        {
            return db.EBOOKS.Where(x => x.id == id).FirstOrDefault();
        }

        //xoá ebook
        [Route("api/AdminAPI/DeleteSubjectById")]
        [HttpPost]
        public string DeleteSubjectById(SUBJECTEBOOK subject)
        {
            var ebook = db.EBOOKS.Where(x => x.sub_id == subject.id).ToList();
            foreach (var item in ebook)
            {
                db.EBOOKS.Remove(item);
                db.SaveChanges();
            }
            var sub = db.SUBJECTEBOOKs.Where(x => x.id == subject.id).FirstOrDefault();
            MyHub.DeleteSubject(subject.id);
            db.SUBJECTEBOOKs.Remove(sub);
            db.SaveChanges();

            return "Xóa thành công";
        }

        [Route("api/AdminAPI/DeleteSubjectById1")]
        [HttpPost]
        public string DeleteSubjectById1(SUBJECTEBOOK sub)
        {
            return "Xóa thành công" + sub.name;
        }

    }
}
