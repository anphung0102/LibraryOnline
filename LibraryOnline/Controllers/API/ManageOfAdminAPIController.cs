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
    public class ManageOfAdminAPIController : ApiController
    {
        private LibraryOnlineFinalEntities db = new LibraryOnlineFinalEntities();
        // lấy subject Essay
        [Route("api/ManageOfAdminAPI/GetSubjectEssay")]
        [HttpGet]
        public IEnumerable<Subject_Essay> GetSubjectEssay()
        {
            //var a = db.Subject_Ebook.ToList();
            //return a;
            return db.Subject_Essay.ToList();
        }

        //Tạo môn học trong Essay
        [Route("api/ManageOfAdminAPI/CreateSubjectEssay")]
        [HttpPost]
        public SubjectCreationResult CreateSubject(SubjectViewModel subject)
        {
            var sub = db.Subject_Essay.Where(x => x.name.Equals(subject.Name)).FirstOrDefault();
            if (sub != null)
            {
                return new SubjectCreationResult
                {
                    IsSuccess = false
                };
            }
            else
            {
                db.Subject_Essay.Add(new Subject_Essay
                {
                    subessay_id = "",
                    name = subject.Name,
                });
                db.SaveChanges();
               // var sub_essay = db.Subject_Essay.Where(x => x.name.Equals(subject.Name)).FirstOrDefault();
                var sub_essay = db.Subject_Essay.OrderByDescending(x => x.id).Take(1).FirstOrDefault();
                var subessay_id = db.Subject_Essay.Where(x => x.id == sub_essay.id).Select(x => x.subessay_id).FirstOrDefault();
                
                return new SubjectCreationResult
                {
                    IsSuccess = true,
                    Id = sub_essay.id,
                    Subessay_Id = subessay_id,
                    Name = sub_essay.name
                };
            }
        }

        //xoá essay
        [Route("api/ManageOfAdminAPI/DeleteSubjectEssay")]
        [HttpPost]
        public string DeleteSubjectEssay(Subject_Essay subject)
        {
            var essay = db.Essays.Where(x => x.sub_id == subject.id).ToList();
            foreach (var item in essay)
            {
                db.Essays.Remove(item);
                db.SaveChanges();
            }
            var sub = db.Subject_Essay.Where(x => x.id == subject.id).FirstOrDefault();
            db.Subject_Essay.Remove(sub);
            db.SaveChanges();

            return "Xóa thành công";
        }

        // lấy subject thesis
        [Route("api/ManageOfAdminAPI/GetSubjectThesis")]
        [HttpGet]
        public IEnumerable<Subject_Thesis> GetSubjectThesis()
        {
            //var a = db.Subject_Ebook.ToList();
            //return a;
            return db.Subject_Thesis.ToList();
        }

        //Tạo môn học trong Thesis
        [Route("api/ManageOfAdminAPI/CreateSubjectThesis")]
        [HttpPost]
        public SubjectCreationResult CreateSubjectThesis(SubjectViewModel subject)
        {
            var sub = db.Subject_Thesis.Where(x => x.name.Equals(subject.Name)).FirstOrDefault();
            if (sub != null)
            {
                return new SubjectCreationResult
                {
                    IsSuccess = false
                };
            }
            else
            {
                db.Subject_Thesis.Add(new Subject_Thesis
                {
                    subthesis_id = "",
                    name = subject.Name,
                });
                db.SaveChanges();
                //var sub_thesis = db.Subject_Thesis.Where(x => x.name.Equals(subject.Name)).FirstOrDefault();
                var sub_thesis = db.Subject_Thesis.OrderByDescending(x => x.id).Take(1).FirstOrDefault();
                var subethesis_id = db.Subject_Thesis.Where(x => x.id == sub_thesis.id).Select(x => x.subthesis_id).FirstOrDefault();

                return new SubjectCreationResult
                {
                    IsSuccess = true,
                    Id = sub_thesis.id,
                    Subthesis_Id = subethesis_id,
                    Name = sub_thesis.name
                };
            }
        }
        //sửa essay
        [Route("api/ManageOfAdminAPI/EditSubjectEssayById")]
        [HttpPost]
        public SubjectCreationResult EditSubjectEssayById(Subject_Essay subject)
        {
            var sub = db.Subject_Essay.Where(x => x.id == subject.id).FirstOrDefault();
            if (sub == null)
            {
                return new SubjectCreationResult
                {
                    IsSuccess = false
                };
            }
            else
            {
                sub.name = subject.name;
                db.SaveChanges();
                return new SubjectCreationResult
                {
                    IsSuccess = true
                };
            }
        }
        //sửa thesis
        [Route("api/ManageOfAdminAPI/EditSubjectThesisById")]
        [HttpPost]
        public SubjectCreationResult EditSubjectThesisById(Subject_Thesis subject)
        {
            var sub = db.Subject_Thesis.Where(x => x.id == subject.id).FirstOrDefault();
            if (sub == null)
            {
                return new SubjectCreationResult
                {
                    IsSuccess = false
                };
            }
            else
            {
                sub.name = subject.name;
                db.SaveChanges();
                return new SubjectCreationResult
                {
                    IsSuccess = true
                };
            }
        }
        //xoá essay
        [Route("api/ManageOfAdminAPI/DeleteSubjectThesis")]
        [HttpPost]
        public string DeleteSubjectThesis(Subject_Thesis subject)
        {
            var thesis = db.Theses.Where(x => x.sub_id == subject.id).ToList();
            foreach (var item in thesis)
            {
                db.Theses.Remove(item);
                db.SaveChanges();
            }
            var sub = db.Subject_Thesis.Where(x => x.id == subject.id).FirstOrDefault();
            db.Subject_Thesis.Remove(sub);
            db.SaveChanges();

            return "Xóa thành công";
        }
        // lấy ds essay quyền admin
        [Route("api/ManageOfAdminAPI/GetEssayByAdmin")]
        [HttpGet]
        public IEnumerable<Essay> GetEssayByAdmin()
        {

            return db.Essays.ToList(); ;
        }

        //Tạo môn học trong Ebook
        [Route("api/ManageOfAdminAPI/UploadEssays")]
        [HttpPost]
        public EssayCreationResult UploadEssays()
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
            var executor1 = HttpContext.Current.Request["student1"];
            var executor2 = HttpContext.Current.Request["student2"];
            var year = HttpContext.Current.Request["year"];
            var userid = HttpContext.Current.Request["userid"];
            var subid = HttpContext.Current.Request["subid"];
            var date_upload = DateTime.Now;
            //int course = Convert.ToInt32(year);
            int user_id = Convert.ToInt32(userid);
            int sub_id = Convert.ToInt32(subid);
            string strExtexsion = Path.GetExtension(httpPostedFile.FileName).Trim();//lấy đuôi file
            var sub = db.Essays.Where(x => x.title.Equals(title)).FirstOrDefault();//này làm gì làm lấy so sánh đồ giống cái m làm t sửa lại

            if (sub != null)
            {
                return new EssayCreationResult
                {
                    IsSuccess = false
                };
            }
            else
            {
                if (strExtexsion == ".pdf")//chỉ cho up pdf
                {
                    db.Essays.Add(new Essay
                    {
                        essay_id = "",
                        title = title,
                        describe = describe,
                        instructor = instructor,
                        executor1 = executor1,
                        executor2 = executor2,
                        //course = year,
                        filename = httpPostedFile.FileName,
                        date_upload = date_upload,
                        user_id = user_id,
                        sub_id = sub_id,
                    });
                    db.SaveChanges();
                }

                var loadessay = db.Essays.Where(x => x.title.Equals(title)).FirstOrDefault();

                //MyHub.Post(sub_ebook.id, sub_ebook.name);
                return new EssayCreationResult
                {
                    IsSuccess = true,
                    Id = loadessay.id,
                    Essay_Id = loadessay.essay_id,
                    Title = loadessay.title,
                    Describe = loadessay.describe,
                    Instructor = loadessay.instructor,
                    Executor1 = loadessay.executor1,
                    Executor2 = loadessay.executor2,
                    FileName = loadessay.filename,
                  //  Date_Upload = loadessay.date_upload
                };
            }
        }

        //xoá essay
        [Route("api/ManageOfAdminAPI/DeleteFileUploadEssay")]
        [HttpPost]
        public string DeleteFileUploadById(Essay subject)
        {
            //var ebook = db.Ebooks.Where(x => x.sub_id == subject.id).ToList();
            //foreach (var item in ebook)
            //{
            //    db.Ebooks.Remove(item);
            //    db.SaveChanges();
            //}
            var sub = db.Essays.Where(x => x.id == subject.id).FirstOrDefault();
            db.Essays.Remove(sub);
            db.SaveChanges();

            return "Xóa thành công";
        }

        // lấy ds thesis quyền admin
        [Route("api/ManageOfAdminAPI/GetThesisByAdmin")]
        [HttpGet]
        public IEnumerable<Thesis> GetThesisByAdmin()
        {

            return db.Theses.ToList(); ;
        }

        //upload thesis
        [Route("api/ManageOfAdminAPI/UploadThesis")]
        [HttpPost]
        public ThesisCreationResult UploadThesis()
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
            var executor1 = HttpContext.Current.Request["student1"];
            var executor2 = HttpContext.Current.Request["student2"];
            var year = HttpContext.Current.Request["year"];
            var userid = HttpContext.Current.Request["userid"];
            var subid = HttpContext.Current.Request["subid"];
            var date_upload = DateTime.Now;
            //int course = Convert.ToInt32(year);
            int user_id = Convert.ToInt32(userid);
            int sub_id = Convert.ToInt32(subid);
            string strExtexsion = Path.GetExtension(httpPostedFile.FileName).Trim();//lấy đuôi file
            var sub = db.Theses.Where(x => x.title.Equals(title)).FirstOrDefault();//này làm gì làm lấy so sánh đồ giống cái m làm t sửa lại

            if (sub != null)
            {
                return new ThesisCreationResult
                {
                    IsSuccess = false
                };
            }
            else
            {
                if (strExtexsion == ".pdf")//chỉ cho up pdf
                {
                    db.Theses.Add(new Thesis
                    {
                        thesis_id = "",
                        title = title,
                        describe = describe,
                        instructor = instructor,
                        executor1 = executor1,
                        executor2 = executor2,
                        //course = year,
                        filename = httpPostedFile.FileName,
                        date_upload = date_upload,
                        user_id = user_id,
                        sub_id = sub_id,
                    });
                    db.SaveChanges();
                }

                var loadthesis = db.Theses.Where(x => x.title.Equals(title)).FirstOrDefault();

                //MyHub.Post(sub_ebook.id, sub_ebook.name);
                return new ThesisCreationResult
                {
                    IsSuccess = true,
                    Id = loadthesis.id,
                    Thesis_Id = loadthesis.thesis_id,
                    Title = loadthesis.title,
                    Describe = loadthesis.describe,
                    Instructor = loadthesis.instructor,
                    Executor1 = loadthesis.executor1,
                    Executor2 = loadthesis.executor2,
                    FileName = loadthesis.filename,
                  //  Date_Upload = loadthesis.date_upload
                };
            }
        }
        //xoá thesis
        [Route("api/ManageOfAdminAPI/DeleteFileUploadThesis")]
        [HttpPost]
        public string DeleteFileUploadThesis(Thesis subject)
        {
            //var ebook = db.Ebooks.Where(x => x.sub_id == subject.id).ToList();
            //foreach (var item in ebook)
            //{
            //    db.Ebooks.Remove(item);
            //    db.SaveChanges();
            //}
            var sub = db.Theses.Where(x => x.id == subject.id).FirstOrDefault();
            db.Theses.Remove(sub);
            db.SaveChanges();

            return "Xóa thành công";
        }

        // lấy ds User
        // lấy ds thesis quyền admin
        [Route("api/ManageOfAdminAPI/GetUsers")]
        [HttpGet]
        public IEnumerable<User> GetUsers()
        {

            return db.Users.ToList(); ;
        }
        //xoá User
        [Route("api/ManageOfAdminAPI/DeleteUser")]
        [HttpPost]
        public string DeleteUser(User user)
        {
            var sub = db.Users.Where(x => x.id == user.id).FirstOrDefault();
            db.Users.Remove(sub);
            db.SaveChanges();

            return "Xóa thành công";
        }

        [Route("api/ManageOfAdminAPI/AddUsers")]
        [HttpPost]
        public UserCreationResult AddUsers()
        {
            var username = HttpContext.Current.Request["email"];
            var password = HttpContext.Current.Request["password"];
            var role = HttpContext.Current.Request["role"];
            var studentid = HttpContext.Current.Request["studentid"];
            var name = HttpContext.Current.Request["name"];
            var classid = HttpContext.Current.Request["classid"];

            int role_id = Convert.ToInt32(role);

            var sub = db.Users.Where(x => x.mssv.Equals(studentid)).FirstOrDefault();//này làm gì làm lấy so sánh đồ giống cái m làm t sửa lại

            if (sub != null)
            {
                return new UserCreationResult
                {
                    IsSuccess = false
                };
            }
            else
            {
               
                    db.Users.Add(new User
                    {
                        
                        username = username,
                        password = password,
                        role_id = role_id,
                        mssv = studentid,
                        fullname = name,
                        
                        class_id = classid,
                       
                    });
                    db.SaveChanges();
                

                var loaduser = db.Users.Where(x => x.mssv.Equals(studentid)).FirstOrDefault();

                //MyHub.Post(sub_ebook.id, sub_ebook.name);
                return new UserCreationResult
                {
                    IsSuccess = true,
                    Id = loaduser.id,
                    UserName = loaduser.username,
                    PassWord = loaduser.password,
                    Role_Id = loaduser.role_id,
                    Mssv = loaduser.mssv,
                    FullName = loaduser.fullname,
                    Class_Id = loaduser.class_id
                  
                };
            }
        }
    }
}
