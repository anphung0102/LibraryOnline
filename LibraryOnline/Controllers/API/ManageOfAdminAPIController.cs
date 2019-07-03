using LibraryOnline.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Security.Cryptography;
using System.Text;

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
                    subessay_id = subject.Subessay_Id,
                    name = subject.Name,
                });
                db.SaveChanges();
                // var sub_essay = db.Subject_Essay.Where(x => x.name.Equals(subject.Name)).FirstOrDefault();
                //var sub_essay = db.Subject_Essay.OrderByDescending(x => x.id).Take(1).FirstOrDefault();
                var sub_essay = db.Subject_Essay.Where(x => x.subessay_id == subject.Subessay_Id).FirstOrDefault();
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
                    subthesis_id = subject.Subthesis_Id,
                    name = subject.Name,
                });
                db.SaveChanges();
                //var sub_thesis = db.Subject_Thesis.Where(x => x.name.Equals(subject.Name)).FirstOrDefault();
                //var sub_thesis = db.Subject_Thesis.OrderByDescending(x => x.id).Take(1).FirstOrDefault();
                var sub_thesis = db.Subject_Thesis.Where(x => x.subthesis_id == subject.Subthesis_Id).FirstOrDefault();
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
                sub.subessay_id = subject.subessay_id;
                sub.name = subject.name;
                db.SaveChanges();
                return new SubjectCreationResult
                {
                    IsSuccess = true,
                    Subessay_Id = sub.subessay_id,
                    Name = sub.name
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
                sub.subthesis_id = subject.subthesis_id;
                sub.name = subject.name;
                db.SaveChanges();
                return new SubjectCreationResult
                {
                    IsSuccess = true,
                    Subthesis_Id = sub.subthesis_id,
                    Name = sub.name
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
            var sub = db.Subject_Thesis.Where(x => x.id == subject.id).FirstOrDefault();
            db.Subject_Thesis.Remove(sub);
            db.SaveChanges();

            return "Xóa thành công";
        }
        // lấy ds essay quyền admin
        [Route("api/ManageOfAdminAPI/GetEssayByAdmin")]
        [HttpGet]
        public object GetEssayByAdmin()
        {
            var data = (from e in db.Essays
                        from s in db.Subject_Essay
                        where e.sub_id == s.id
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
            // return db.Essays.ToList(); ;
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
        public string DeleteFileUploadById(Essay essay)
        {
            //var ebook = db.Ebooks.Where(x => x.sub_id == subject.id).ToList();
            //foreach (var item in ebook)
            //{
            //    db.Ebooks.Remove(item);
            //    db.SaveChanges();
            //}
            var ess = db.Essays.Where(x => x.id == essay.id).FirstOrDefault();
            var search = db.SearchFiles.Where(x => x.book_id == ess.essay_id).FirstOrDefault();
            var rate = db.RateStars.Where(x => x.book_id == ess.essay_id).ToList();
            var time = db.Times.Where(x => x.bookid == ess.essay_id).ToList();
            if(ess != null)
            {
                db.Essays.Remove(ess);
                db.SaveChanges();
            }
            if (search != null)
            {
                db.SearchFiles.Remove(search);
                db.SaveChanges();
            }
            if (rate != null)
            {
                foreach(var item in rate)
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

        // lấy ds thesis quyền admin
        [Route("api/ManageOfAdminAPI/GetThesisByAdmin")]
        [HttpGet]
        public object GetThesisByAdmin()
        {
            var data = (from e in db.Theses
                        from s in db.Subject_Thesis
                        where e.sub_id == s.id
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
            // return db.Theses.ToList(); ;
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
        public string DeleteFileUploadThesis(Thesis thesis)
        {
            var the = db.Theses.Where(x => x.id == thesis.id).FirstOrDefault();
            var search = db.SearchFiles.Where(x => x.book_id == the.thesis_id).FirstOrDefault();
            var rate = db.RateStars.Where(x => x.book_id == the.thesis_id).ToList();
            var time = db.Times.Where(x => x.bookid == the.thesis_id).ToList();
           
            if (the != null)
            {
                db.Theses.Remove(the);
                db.SaveChanges();
            }
            if (search != null)
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

        // lấy ds User
        // lấy ds thesis quyền admin
        [Route("api/ManageOfAdminAPI/GetUsers")]
        [HttpGet]
        public object GetUsers()
        {
            var data = (from u in db.Users
                        from r in db.Roles
                        where u.role_id == r.id
                        select new
                        {
                            id = u.id,
                            username = u.username,
                            password = u.password,
                            role_id = r.id,
                            fullname = u.fullname,
                            mssv = u.mssv,
                            class_id = u.class_id,
                            role_name = r.name,
                            image = u.image
                        }).ToList();

            return data;
        }
        //xoá User
        [Route("api/ManageOfAdminAPI/DeleteUser")]
        [HttpPost]
        public string DeleteUser(User user)
        {
            var sub = db.Users.Where(x => x.id == user.id).FirstOrDefault();
            if(sub != null)
            {
                db.Users.Remove(sub);
                db.SaveChanges();
            }
            return "Xóa thành công";
        }

        // Mã hóa MK
        public static byte[] encryptData(string data)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashedBytes;
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(data));
            return hashedBytes;
        }
        public static string md5(string data)
        {
            return BitConverter.ToString(encryptData(data)).Replace("-", "").ToLower();
        }

        //mã hóa và giải mã
        static string key { get; set; } = "A!9HHhi%XjjYY4YP2@Nob009X";
        public static string Encrypt(string text)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateEncryptor())
                    {
                        byte[] textBytes = UTF8Encoding.UTF8.GetBytes(text);
                        byte[] bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
                        return Convert.ToBase64String(bytes, 0, bytes.Length);
                    }
                }
            }
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

            var sub = db.Users.Where(x =>x.username == username).FirstOrDefault();
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
                        password = Encrypt(password),
                        role_id = role_id,
                        mssv = studentid,
                        fullname = name,
                        class_id = classid,
                       
                    });
                    db.SaveChanges();
                var loaduser = db.Users.OrderByDescending(x=>x.id).FirstOrDefault();
                var rolename = db.Roles.Where(x => x.id == loaduser.role_id).FirstOrDefault();
                return new UserCreationResult
                {
                    IsSuccess = true,
                    Id = loaduser.id,
                    UserName = loaduser.username,
                    PassWord = loaduser.password,
                    Role_Id = loaduser.role_id,
                    RoleName = rolename.name,
                    Mssv = loaduser.mssv,
                    FullName = loaduser.fullname,
                    Class_Id = loaduser.class_id
                  
                };
            }
        }

        
        [Route("api/ManageOfAdminAPI/UpdateUser")]
        [HttpPost]
        public UserCreationResult UpdateUser()
        {
            var username = HttpContext.Current.Request["email"];
            //var password = HttpContext.Current.Request["password"];
            var role = HttpContext.Current.Request["role"];
            var studentid = HttpContext.Current.Request["studentid"];
            var name = HttpContext.Current.Request["name"];
            var classid = HttpContext.Current.Request["classid"];

            int role_id = Convert.ToInt32(role);

            var user = db.Users.Where(x => x.mssv == studentid || x.username == username).FirstOrDefault();
            if (user == null)
            {
                return new UserCreationResult
                {
                    IsSuccess = false
                };
            }
            else
            {
                user.username = username;
                //user.password = md5(password);
                user.role_id = role_id;
                user.mssv = studentid;
                user.fullname = name;
                user.class_id = classid;
                db.SaveChanges();
                var loaduser = db.Users.Where(x=>x.username == username).FirstOrDefault();
                var rolename = db.Roles.Where(x => x.id == loaduser.role_id).FirstOrDefault();
                return new UserCreationResult
                {
                    IsSuccess = true,
                    Id = loaduser.id,
                    UserName = loaduser.username,
                    //PassWord = loaduser.password,
                    Role_Id = loaduser.role_id,
                    RoleName = rolename.name,
                    Mssv = loaduser.mssv,
                    FullName = loaduser.fullname,
                    Class_Id = loaduser.class_id
                };
            }
        }

        [Route("api/ManageOfAdminAPI/DeleteUserByID")]
        [HttpPost]
        public string DeleteUserByID(int id)
        {
            
            var user_ebook = db.Ebooks.Where(x => x.user_id == id).ToList();
            if (user_ebook != null)
            {
                foreach(var item in user_ebook)
                {
                    db.Ebooks.Remove(item);
                    db.SaveChanges();
                }
            }
            var user_essay = db.Essays.Where(x => x.user_id == id).ToList();
            if (user_essay != null)
            {
                foreach (var item in user_essay)
                {
                    db.Essays.Remove(item);
                    db.SaveChanges();
                }
                
            }
            var user_thesis = db.Theses.Where(x => x.user_id == id).ToList();
            if (user_thesis != null)
            {
                foreach (var item in user_thesis)
                {
                    db.Theses.Remove(item);
                    db.SaveChanges();
                }
                
            }
            var user_search = db.SearchFiles.Where(x => x.user_id == id).ToList();
            if (user_search != null)
            {
                foreach (var item in user_search)
                {
                    db.SearchFiles.Remove(item);
                    db.SaveChanges();
                }
               
            }
            var user_rate = db.RateStars.Where(x => x.user_id == id).ToList();
            if (user_rate != null)
            {
                foreach (var item in user_rate)
                {
                    db.RateStars.Remove(item);
                    db.SaveChanges();
                }

               
            }
            var user_time = db.Times.Where(x => x.userid == id).ToList();
            if (user_time != null)
            {
                foreach (var item in user_time)
                {
                    db.Times.Remove(item);
                    db.SaveChanges();
                }
               
            }
            var user = db.Users.Where(x => x.id == id).FirstOrDefault();
            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();
            }
            db.SaveChanges();
            return "Xóa thành công";
        }
        //tạo kết nối cho SQL Server and OLEDB
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=LibraryOnlineFinal;User ID=sa;Password=123");

        OleDbConnection Econ;

        [Route("api/ManageOfAdminAPI/ImportFile")]
        [HttpPost]
        public IHttpActionResult ImportFile() 
        {
            var httpPostedFile = HttpContext.Current.Request.Files["fileInput"];//lấy file
            if (httpPostedFile != null)
            {
                string filename = Guid.NewGuid() + Path.GetExtension(httpPostedFile.FileName);
                string filepath = "/excelfolder/" + filename;
                httpPostedFile.SaveAs(Path.Combine(HttpContext.Current.Server.MapPath("/excelfolder"), filename));
                InsertExceldata(filepath, filename);
            }
             
            return Ok();
        }

        private void ExcelConn(string filepath)
        {
            string constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", filepath);
            Econ = new OleDbConnection(constr);
        }
        private void InsertExceldata(string fileepath, string filename)
        {
            string fullpath = HttpContext.Current.Server.MapPath("/excelfolder/") + filename;
            ExcelConn(fullpath);
            string query = string.Format("Select * from [{0}]", "Sheet1$");
            OleDbCommand Ecom = new OleDbCommand(query, Econ);
            Econ.Open();
            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            Econ.Close();
            oda.Fill(ds);
            DataTable dt = ds.Tables[0];

            SqlBulkCopy objbulk = new SqlBulkCopy(con);

            objbulk.DestinationTableName = "ImportTemp";
            objbulk.ColumnMappings.Add("username", "username");
            objbulk.ColumnMappings.Add("password", "password");
            objbulk.ColumnMappings.Add("role_id", "role_id");
            objbulk.ColumnMappings.Add("fullname", "fullname");
            objbulk.ColumnMappings.Add("mssv", "mssv");
            objbulk.ColumnMappings.Add("class_id", "class_id");

            var a = objbulk;
            con.Open();
            objbulk.WriteToServer(dt);
            con.Close();
            var importTemp = db.ImportTemps.ToList();
            var userlist = db.Users.ToList();
            foreach(var item in importTemp)
            {
                var check = db.Users.Where(x => x.username == item.username || x.mssv == item.mssv).FirstOrDefault();
                if(check == null)
                {
                    db.Users.Add(new User
                    {
                        username = item.username,
                        password = Encrypt(item.password),
                        role_id = item.role_id.Value,
                        fullname = item.fullname,
                        mssv = item.mssv,
                        class_id = item.class_id

                    });
                    db.SaveChanges();
                }
            }
            foreach (var item in importTemp)
            {
                db.ImportTemps.Remove(item);
                db.SaveChanges();
            }
                
        }
    }
}
