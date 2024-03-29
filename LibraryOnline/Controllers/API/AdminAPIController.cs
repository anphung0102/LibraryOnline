﻿using LibraryOnline.Models;
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
        public EbookCreationResult UploadFiles()
        {
            string strExtexsion = "";
            var httpPostedFile = HttpContext.Current.Request.Files["fileInput"];//lấy file
            if (httpPostedFile != null)
            {
                //đường dẫn lưu file
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Upload/"), httpPostedFile.FileName);//tên file
                //lưu file vào đường dẫn
                strExtexsion = Path.GetExtension(httpPostedFile.FileName).Trim();//lấy đuôi file
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

            if (strExtexsion == ".pdf")//chỉ cho up pdf
            {
                var filename = db.Ebooks.Where(x => x.filename == httpPostedFile.FileName).FirstOrDefault();
                if (filename != null)
                {
                    return new EbookCreationResult
                    {
                        IsSuccess = false,
                        Message = "Tên file đã bị trùng!"
                    };
                }
                else
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
                                countView = 0,
                                countDowload = 0
                            });
                        db.SaveChanges();
                        var book_id = db.Ebooks.OrderByDescending(x => x.id).Select(x => x.ebook_id).FirstOrDefault();
                        var userName = db.Users.Where(x => x.id == user_id).FirstOrDefault();
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
                              username = userName.fullname,
                              type = "ebook"
                          });
                        db.SaveChanges();//lưu dât thôi cái này t chưa chạy t mới test gửi data từ  ajax qua thôi
                        var user = db.Users.Where(x => x.id == user_id).Select(x => x.username).FirstOrDefault();
                        var subject = db.Subject_Ebook.Where(x => x.id == sub_id).Select(x => x.name).FirstOrDefault();
                        var fileinfo = db.Ebooks.OrderByDescending(x => x.id).FirstOrDefault();
                        var date_up = date_upload.ToString("MM/dd/yyyy");
                        MyHub.PostFileEbook(fileinfo.id, fileinfo.title, fileinfo.author, fileinfo.describe,
                            fileinfo.year, fileinfo.filename, date_up, user, subject);
                        var loadebook = db.Ebooks.OrderByDescending(x => x.id).Take(1).FirstOrDefault();
                        var ebook_id = db.Ebooks.Where(x => x.id == loadebook.id).Select(x => x.ebook_id).FirstOrDefault();
                        var sub = db.Subject_Ebook.Where(x => x.id == sub_id).FirstOrDefault();

                        return new EbookCreationResult
                        {
                            IsSuccess = true,
                            Id = loadebook.id,
                            Ebook_Id = ebook_id,
                            Title = loadebook.title,
                            Describe = loadebook.describe,
                            Author = loadebook.author,
                            Year = loadebook.year,
                            FileName = loadebook.filename,
                            Date_Upload = loadebook.date_upload.Value,
                            Sub_Name = sub.name,
                            Sub_Id = sub.id,
                            User_Name = userName.fullname
                        };
                    }
                }

            }
            else
            {
                return new EbookCreationResult
                {
                    IsSuccess = false,
                    Message = "Vui lòng chọn file pdf!"
                };
            }
            //return new EbookCreationResult
            //{
            //    IsSuccess = false,
            //};
        }
        //Upload file cho Ebook 
        [Route("api/AdminAPI/EditEbookFiles")]
        [HttpPost]
        public EbookCreationResult EditEbookFiles()
        {
            string strExtexsion = "";
            var httpPostedFile = HttpContext.Current.Request.Files["fileInput"];//lấy file
            if (httpPostedFile != null)
            {
                //đường dẫn lưu file
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Upload/"), httpPostedFile.FileName);//tên file
                //lưu file vào đường dẫn
                strExtexsion = Path.GetExtension(httpPostedFile.FileName).Trim();//lấy đuôi file
                httpPostedFile.SaveAs(fileSavePath);
            }
            var ebook_id = HttpContext.Current.Request["ebook_id"];
            var title = HttpContext.Current.Request["title"];
            var describe = HttpContext.Current.Request["describe"];
            var author = HttpContext.Current.Request["author"];
            var year = HttpContext.Current.Request["year"];
            var userid = HttpContext.Current.Request["userid"];
            var subid = HttpContext.Current.Request["subid"];
            var date_upload = DateTime.Now;
            int user_id = Convert.ToInt32(userid);
            int sub_id = Convert.ToInt32(subid);

            var temp = db.Ebooks.Where(x => x.ebook_id == ebook_id).FirstOrDefault();
            var search = db.SearchFiles.Where(x => x.book_id == ebook_id).FirstOrDefault();
            if (temp != null)
            {
                if (httpPostedFile != null)
                {
                    if (strExtexsion == ".pdf")//chỉ cho up pdf
                    {
                        var filename = db.Ebooks.Where(x => x.filename == httpPostedFile.FileName).FirstOrDefault();
                        if (filename != null)
                        {
                            return new EbookCreationResult
                            {
                                IsSuccess = false,
                                Message = "Tên file đã bị trùng!"
                            };

                        }
                        else
                        {
                            //update table ebook
                            temp.title = title;
                            temp.author = author;
                            temp.describe = describe;
                            temp.year = year;
                            temp.sub_id = sub_id;
                            temp.filename = httpPostedFile.FileName;
                            //db.SaveChanges();
                            //update table search file
                            search.title = title;
                            search.author = author;
                            search.describe = describe;
                            search.year = year;
                            search.sub_id = sub_id;
                            search.filename = httpPostedFile.FileName;
                            db.SaveChanges();
                            return new EbookCreationResult
                            {
                                IsSuccess = true,
                                Id = temp.id,
                                Ebook_Id = temp.ebook_id,
                                Title = temp.title,
                                Describe = temp.describe,
                                Author = temp.author,
                                Year = temp.year,
                                FileName = temp.filename,
                                Date_Upload = temp.date_upload.Value
                            };
                        }
                    }

                }
                else
                {
                    //update table ebook
                    temp.title = title;
                    temp.author = author;
                    temp.describe = describe;
                    temp.year = year;
                    //db.SaveChanges();
                    //update table search file
                    search.title = title;
                    search.author = author;
                    search.describe = describe;
                    search.year = year;
                    db.SaveChanges();

                    return new EbookCreationResult
                    {
                        IsSuccess = true,
                        Id = temp.id,
                        Ebook_Id = temp.ebook_id,
                        Title = temp.title,
                        Describe = temp.describe,
                        Author = temp.author,
                        Year = temp.year,
                        FileName = temp.filename,
                        Date_Upload = temp.date_upload.Value
                    };

                }
            }

            return new EbookCreationResult
            {
                IsSuccess = false,
            };

        }


        //Edit file cho Essay
        [Route("api/AdminAPI/EditEssayFiles")]
        [HttpPost]
        public EssayCreationResult EditEssayFiles()
        {
            string strExtexsion = "";
            var httpPostedFile = HttpContext.Current.Request.Files["fileInput"];//lấy file
            if (httpPostedFile != null)
            {
                //đường dẫn lưu file
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Upload/"), httpPostedFile.FileName);//tên file
                strExtexsion = Path.GetExtension(httpPostedFile.FileName).Trim();//lấy đuôi file
                //lưu file vào đường dẫn
                httpPostedFile.SaveAs(fileSavePath);
            }
            var essay_id = HttpContext.Current.Request["essay_id"];
            var title = HttpContext.Current.Request["title"];
            var describe = HttpContext.Current.Request["describe"];
            var instructor = HttpContext.Current.Request["instructor"];
            var executor1 = HttpContext.Current.Request["student1"];
            var executor2 = HttpContext.Current.Request["student2"];
            var course = HttpContext.Current.Request["course"];
            var userid = HttpContext.Current.Request["userid"];
            var subid = HttpContext.Current.Request["subid"];
            var date_upload = DateTime.Now;
            int user_id = Convert.ToInt32(userid);
            int sub_id = Convert.ToInt32(subid);


            var temp = db.Essays.Where(x => x.essay_id == essay_id).FirstOrDefault();
            var search = db.SearchFiles.Where(x => x.book_id == essay_id).FirstOrDefault();
            var sub = db.Subject_Essay.Where(x => x.id == sub_id).FirstOrDefault();
            if (temp != null)
            {
                if (httpPostedFile != null)
                {
                    if (strExtexsion == ".pdf")//chỉ cho up pdf
                    {
                        var filename = db.Essays.Where(x => x.filename == httpPostedFile.FileName).FirstOrDefault();
                        if (filename != null)
                        {
                            return new EssayCreationResult
                            {
                                IsSuccess = false,
                                Message = "Tên file đã bị trùng!"
                            };

                        }
                        else
                        {
                            //update table essay 
                            temp.title = title;
                            temp.instructor = instructor;
                            temp.executor1 = executor1;
                            temp.executor2 = executor2;
                            temp.course = course;
                            temp.filename = httpPostedFile.FileName;
                            temp.describe = describe;
                            temp.sub_id = sub_id;

                            //db.SaveChanges();
                            //update table search file
                            search.title = title;
                            search.instructor = instructor;
                            search.executor1 = executor1;
                            search.executor2 = executor2;
                            search.describe = describe;
                            search.year = course;
                            search.filename = httpPostedFile.FileName;
                            search.sub_id = sub_id;
                            db.SaveChanges();
                            return new EssayCreationResult
                            {
                                IsSuccess = true,
                                Id = temp.id,
                                Essay_Id = essay_id,
                                Title = temp.title,
                                Describe = temp.describe,
                                Instructor = temp.instructor,
                                Executor1 = temp.executor1,
                                Executor2 = temp.executor2,
                                FileName = temp.filename,
                                Date_Upload = temp.date_upload.Value,
                                Course = temp.course,
                                Sub_Id = sub.id,
                                Sub_Name = sub.name
                            };
                        }
                    }

                }
                else
                {
                    //update table essay 
                    temp.title = title;
                    temp.instructor = instructor;
                    temp.executor1 = executor1;
                    temp.executor2 = executor2;
                    temp.course = course;
                    temp.describe = describe;
                    temp.sub_id = sub_id;

                    //db.SaveChanges();
                    //update table search file
                    search.title = title;
                    search.instructor = instructor;
                    search.executor1 = executor1;
                    search.executor2 = executor2;
                    search.describe = describe;
                    search.year = course;
                    search.sub_id = sub_id;
                    db.SaveChanges();

                    return new EssayCreationResult
                    {
                        IsSuccess = true,
                        Id = temp.id,
                        Essay_Id = essay_id,
                        Title = temp.title,
                        Describe = temp.describe,
                        Instructor = temp.instructor,
                        Executor1 = temp.executor1,
                        Executor2 = temp.executor2,
                        FileName = temp.filename,
                        Date_Upload = temp.date_upload.Value,
                        Course = temp.course,
                        Sub_Id = sub.id,
                        Sub_Name = sub.name
                    };

                }
            }

            return new EssayCreationResult
            {
                IsSuccess = false,
            };
            // return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi!!!");

        }


        //Edit file cho Essay
        [Route("api/AdminAPI/EditThesisFiles")]
        [HttpPost]
        public ThesisCreationResult EditThesisFiles()
        {
            string strExtexsion = "";
            var httpPostedFile = HttpContext.Current.Request.Files["fileInput"];//lấy file
            if (httpPostedFile != null)
            {
                //đường dẫn lưu file
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Upload/"), httpPostedFile.FileName);//tên file
                strExtexsion = Path.GetExtension(httpPostedFile.FileName).Trim();//lấy đuôi file
                //lưu file vào đường dẫn
                httpPostedFile.SaveAs(fileSavePath);
            }
            var thesis_id = HttpContext.Current.Request["thesis_id"];
            var title = HttpContext.Current.Request["title"];
            var describe = HttpContext.Current.Request["describe"];
            var instructor = HttpContext.Current.Request["instructor"];
            var executor1 = HttpContext.Current.Request["student1"];
            var executor2 = HttpContext.Current.Request["student2"];
            var course = HttpContext.Current.Request["course"];
            var userid = HttpContext.Current.Request["userid"];
            var subid = HttpContext.Current.Request["subid"];
            var date_upload = DateTime.Now;
            int user_id = Convert.ToInt32(userid);
            int sub_id = Convert.ToInt32(subid);


            var temp = db.Theses.Where(x => x.thesis_id == thesis_id).FirstOrDefault();
            var search = db.SearchFiles.Where(x => x.book_id == thesis_id).FirstOrDefault();
            var sub = db.Subject_Thesis.Where(x => x.id == sub_id).FirstOrDefault();
            if (temp != null)
            {
                if (httpPostedFile != null)
                {
                    if (strExtexsion == ".pdf")//chỉ cho up pdf
                    {
                        var filename = db.Theses.Where(x => x.filename == httpPostedFile.FileName).FirstOrDefault();
                        if (filename != null)
                        {
                            return new ThesisCreationResult
                            {
                                IsSuccess = false,
                                Message = "Tên file đã bị trùng!"
                            };

                        }
                        else
                        {
                            //update table essay 
                            temp.title = title;
                            temp.instructor = instructor;
                            temp.executor1 = executor1;
                            temp.executor2 = executor2;
                            temp.cource = course;
                            temp.filename = httpPostedFile.FileName;
                            temp.describe = describe;
                            temp.sub_id = sub_id;

                            //db.SaveChanges();
                            //update table search file
                            search.title = title;
                            search.instructor = instructor;
                            search.executor1 = executor1;
                            search.executor2 = executor2;
                            search.describe = describe;
                            search.year = course;
                            search.filename = httpPostedFile.FileName;
                            search.sub_id = sub_id;
                            db.SaveChanges();
                            return new ThesisCreationResult
                            {
                                IsSuccess = true,
                                Id = temp.id,
                                Thesis_Id = thesis_id,
                                Title = temp.title,
                                Describe = temp.describe,
                                Instructor = temp.instructor,
                                Executor1 = temp.executor1,
                                Executor2 = temp.executor2,
                                FileName = temp.filename,
                                Date_Upload = temp.date_upload.Value,
                                Course = temp.cource,
                                Sub_Id = sub.id,
                                Sub_Name = sub.name
                            };
                        }
                    }

                }
                else
                {
                    //update table essay 
                    temp.title = title;
                    temp.instructor = instructor;
                    temp.executor1 = executor1;
                    temp.executor2 = executor2;
                    temp.cource = course;
                    temp.describe = describe;
                    temp.sub_id = sub_id;

                    //db.SaveChanges();
                    //update table search file
                    search.title = title;
                    search.instructor = instructor;
                    search.executor1 = executor1;
                    search.executor2 = executor2;
                    search.describe = describe;
                    search.year = course;
                    search.sub_id = sub_id;
                    db.SaveChanges();

                    return new ThesisCreationResult
                    {
                        IsSuccess = true,
                        Id = temp.id,
                        Thesis_Id = thesis_id,
                        Title = temp.title,
                        Describe = temp.describe,
                        Instructor = temp.instructor,
                        Executor1 = temp.executor1,
                        Executor2 = temp.executor2,
                        FileName = temp.filename,
                        Date_Upload = temp.date_upload.Value,
                        Course = temp.cource,
                        Sub_Id = sub.id,
                        Sub_Name = sub.name
                    };

                }
            }

            return new ThesisCreationResult
            {
                IsSuccess = false,
            };
            // return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi!!!");

        }
        //Upload file cho Ebook 
        [Route("api/AdminAPI/UploadFileEssay")]
        [HttpPost]
        public EssayCreationResult UploadFileEssay()
        {
            string strExtexsion = "";
            var httpPostedFile = HttpContext.Current.Request.Files["fileInput"];//lấy file
            if (httpPostedFile != null)
            {
                //đường dẫn lưu file
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Upload/"), httpPostedFile.FileName);//tên file
                strExtexsion = Path.GetExtension(httpPostedFile.FileName).Trim();//lấy đuôi file
                //lưu file vào đường dẫn
                httpPostedFile.SaveAs(fileSavePath);
            }

            var title = HttpContext.Current.Request["title"];
            var describe = HttpContext.Current.Request["describe"];
            var instructor = HttpContext.Current.Request["instructor"];
            var executor1 = HttpContext.Current.Request["student1"];
            var executor2 = HttpContext.Current.Request["student2"];
            var course = HttpContext.Current.Request["course"];
            var userid = HttpContext.Current.Request["userid"];
            var subid = HttpContext.Current.Request["subid"];
            var date_upload = DateTime.Now;
            int user_id = Convert.ToInt32(userid);
            int sub_id = Convert.ToInt32(subid);

            if (strExtexsion == ".pdf")//chỉ cho up pdf
            {
                var filename = db.Essays.Where(x => x.filename == httpPostedFile.FileName).FirstOrDefault();
                if (filename != null)
                {
                    return new EssayCreationResult
                    {
                        IsSuccess = false,
                        Message = "Tên file đã bị trùng!"
                    };
                }
                else
                {
                    using (LibraryOnlineFinalEntities db = new LibraryOnlineFinalEntities())
                    {
                        //Add vô bảng ebook 
                        db.Essays.Add(
                            new Essay
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
                                course = course,
                                countView = 0,
                                countDowload = 0
                            });
                        db.SaveChanges();
                        var book_id = db.Essays.OrderByDescending(x => x.id).Select(x => x.essay_id).FirstOrDefault();
                        var userName = db.Users.Where(x => x.id == user_id).FirstOrDefault();
                        db.SearchFiles.Add(
                          new SearchFile
                          {
                              book_id = book_id,
                              title = title,
                              author = "",
                              year = "",
                              instructor = instructor,
                              executor1 = executor1,
                              executor2 = executor2,
                              describe = describe,
                              filename = httpPostedFile.FileName,
                              date_upload = date_upload,
                              user_id = user_id,
                              sub_id = sub_id,
                              username = userName.fullname,
                              type = "essay"
                          });
                        db.SaveChanges();//lưu dât thôi cái này t chưa chạy t mới test gửi data từ  ajax qua thôi
                        var user = db.Users.Where(x => x.id == user_id).Select(x => x.username).FirstOrDefault();
                        var subject = db.Subject_Essay.Where(x => x.id == sub_id).Select(x => x.name).FirstOrDefault();
                        var fileinfo = db.Essays.OrderByDescending(x => x.id).FirstOrDefault();
                        var date_up = date_upload.ToString("MM/dd/yyyy");
                        MyHub.PostFileEssay(fileinfo.id, fileinfo.essay_id, fileinfo.title, fileinfo.instructor, fileinfo.executor1, fileinfo.executor2, fileinfo.describe, fileinfo.filename, date_up, user, subject);

                        var loadessay = db.Essays.OrderByDescending(x => x.id).Take(1).FirstOrDefault();
                        var essay_id = db.Essays.Where(x => x.id == loadessay.id).Select(x => x.essay_id).FirstOrDefault();
                        var sub = db.Subject_Essay.Where(x => x.id == sub_id).FirstOrDefault();
                        //MyHub.Post(sub_ebook.id, sub_ebook.name);

                        return new EssayCreationResult
                        {
                            IsSuccess = true,
                            Id = loadessay.id,
                            Essay_Id = essay_id,
                            Title = loadessay.title,
                            Describe = loadessay.describe,
                            Instructor = loadessay.instructor,
                            Executor1 = loadessay.executor1,
                            Executor2 = loadessay.executor2,
                            FileName = loadessay.filename,
                            Date_Upload = loadessay.date_upload.Value,
                            Course = loadessay.course,
                            Sub_Id = sub.id,
                            Sub_Name = sub.name,
                            User_Name = userName.fullname
                        };
                        //return Request.CreateResponse("Thành công");
                    }
                }
            }
            else
            {
                return new EssayCreationResult
                {
                    IsSuccess = false,
                    Message = "Bạn phải chọn file pdf"
                };
            }
            // return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi!!!");

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
            var sub = db.Subject_Ebook.Where(x => x.name == subject.Name.Trim()|| x.subebook_id == subject.Subebook_Id.Trim()).FirstOrDefault();
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
                    //subebook_id = "",
                    subebook_id = subject.Subebook_Id.Trim(),
                    name = subject.Name,
                });
                db.SaveChanges();
                //var sub_ebook = db.Subject_Ebook.Where(x => x.name.Equals(subject.Name)).FirstOrDefault();
                //var sub_ebook = db.Subject_Ebook.OrderByDescending(x => x.id).Take(1).FirstOrDefault();
                var sub_ebook = db.Subject_Ebook.Where(x => x.subebook_id == subject.Subebook_Id).FirstOrDefault();
                var subebook_id = db.Subject_Ebook.Where(x => x.id == sub_ebook.id).Select(x => x.subebook_id).FirstOrDefault();

                return new SubjectCreationResult
                {
                    IsSuccess = true,
                    Id = sub_ebook.id,
                    Subebook_Id = subebook_id,
                    Name = sub_ebook.name
                };
            }
        }

        //lấy ebook
        //[Route("api/AdminAPI/GetEbook")]
        //[HttpGet]
        //public IEnumerable<Ebook> GetEbook(int id)
        //{
        //    return db.Ebooks.Where(x => x.sub_id == id).OrderByDescending(x=>x.date_upload).ToList();
        //}
        [Route("api/AdminAPI/GetEbook")]
        [HttpGet]
        public IHttpActionResult GetEbook(int id)
        {
            var data = (from e in db.Ebooks
                        from u in db.Users
                        where e.user_id == u.id && e.sub_id == id
                        select new
                        {
                            Title = e.title,
                            Ebook_Id = e.ebook_id,
                            Poster = u.fullname
                        }).ToList();
            return Ok(data);
        }

        //lấy ebookpaging
        //[Route("api/AdminAPI/GetEbookPaging")]
        //[HttpGet]
        //public IEnumerable<Ebook> GetEbookPaging(int id)
        //{
        //    return db.Ebooks.Where(x => x.sub_id == id).OrderByDescending(x => x.date_upload).ToArray();
        //}
        [Route("api/AdminAPI/GetEbookPaging")]
        [HttpGet]
        public IHttpActionResult GetEbookPaging(int id)
        {
            var data = (from e in db.Ebooks
                        from u in db.Users
                        where e.user_id == u.id && e.sub_id == id
                        select new
                        {
                            Title = e.title,
                            Ebook_Id = e.ebook_id,
                            Poster = u.fullname
                        }).ToList();
            return Ok(data);
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
        public IHttpActionResult GetFileById(string ebook_id)
        {
            var data = from e in db.Ebooks
                       from u in db.Users
                       from s in db.Subject_Ebook
                       where e.user_id == u.id && e.sub_id == s.id && e.ebook_id == ebook_id
                       select new
                       {
                           title = e.title,
                           author = e.author,
                           userupload = u.fullname,
                           subname = s.name,
                           year = e.year,
                           countView = e.countView,
                           countDowload = e.countDowload,
                           filename = e.filename,
                           describe = e.describe

                       };
            return Ok(data);
            //return db.Ebooks.Where(x => x.ebook_id == ebook_id).FirstOrDefault();
        }
        //lấy file theo id của tiểu luận
        [Route("api/AdminAPI/GetFileEssayById")]
        [HttpGet]
        public IHttpActionResult GetFileEssayById(string essay_id)
        {
            var data = from es in db.Essays
                       from u in db.Users
                       from s in db.Subject_Essay
                       where es.user_id == u.id && es.sub_id == s.id && es.essay_id == essay_id
                       select new
                       {
                           title = es.title,
                           instructor = es.instructor,
                           executor1 = es.executor1,
                           executor2 = es.executor2,
                           userupload = u.fullname,
                           subname = s.name,
                           course = es.course,
                           countView = es.countView,
                           countDowload = es.countDowload,
                           filename = es.filename,
                           describe = es.describe

                       };
            return Ok(data);
            //return db.Essays.Where(x => x.essay_id == essay_id).FirstOrDefault();
        }
        //lấy file theo id của khóa luận
        [Route("api/AdminAPI/GetFileThesisById")]
        [HttpGet]
        public IHttpActionResult GetFileThesisById(string thesis_id)
        {
            var data = from th in db.Theses
                       from u in db.Users
                       from s in db.Subject_Thesis
                       where th.user_id == u.id && th.sub_id == s.id && th.thesis_id == thesis_id
                       select new
                       {
                           title = th.title,
                           instructor = th.instructor,
                           executor1 = th.executor1,
                           executor2 = th.executor2,
                           userupload = u.fullname,
                           subname = s.name,
                           course = th.cource,
                           countView = th.countView,
                           countDowload = th.countDowload,
                           filename = th.filename,
                           describe = th.describe

                       };
            return Ok(data);
            //return db.Theses.Where(x => x.thesis_id == thesis_id).FirstOrDefault();
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
        public object GetEbookByAdmin()
        {
            var data = (from e in db.Ebooks
                        from s in db.Subject_Ebook
                        where e.sub_id == s.id
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
            // return db.Ebooks.ToList(); ;
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

            var a = db.Essays.Where(x => x.sub_id == id).OrderByDescending(x => x.date_upload).ToList();
            return a;
        }

        // lấy ds tiểu luận phân trang
        //[Route("api/AdminAPI/GetEssayPaging")]
        //[HttpGet]
        //public IEnumerable<Essay> GetEssayPaging(int id)
        //{
        //    return db.Essays.Where(x => x.sub_id == id).OrderByDescending(x => x.date_upload).ToArray();
        //}
        [Route("api/AdminAPI/GetEssayPaging")]
        [HttpGet]
        public IHttpActionResult GetEssayPaging(int id)
        {
            var data = (from e in db.Essays
                        from u in db.Users
                        where e.user_id == u.id && e.sub_id == id
                        select new
                        {
                            Title = e.title,
                            Essay_Id = e.essay_id,
                            Poster = u.fullname
                        }).ToList();
            return Ok(data);
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

            var a = db.Theses.Where(x => x.sub_id == id).OrderByDescending(x => x.date_upload).ToList();
            return a;
        }

        // lấy ds tiểu luận phân trang
        //[Route("api/AdminAPI/GetThesisPaging")]
        //[HttpGet]
        //public IEnumerable<Thesis> GetThesisPaging(int id)
        //{
        //    return db.Theses.Where(x => x.sub_id == id).OrderByDescending(x => x.date_upload).ToArray();
        //}
        [Route("api/AdminAPI/GetThesisPaging")]
        [HttpGet]
        public IHttpActionResult GetThesisPaging(int id)
        {
            var data = (from e in db.Theses
                        from u in db.Users
                        where e.user_id == u.id && e.sub_id == id
                        select new
                        {
                            Title = e.title,
                            Thesis_Id = e.thesis_id,
                            Poster = u.fullname
                        }).ToList();
            return Ok(data);
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
                    rate = ratemodel.Rate,
                    sub_id = ratemodel.SubId
                });
                db.SaveChanges();

                return new RateStarResult
                {
                    IsSuccess = true
                };
            }
        }
        [Route("api/AdminAPI/GetSubByBookID")]
        [HttpGet]
        public IHttpActionResult GetSubByBookID(string bookid)
        {
            var sub = bookid.Substring(0, 2);
            int? data = 0;
            if (sub == "EB")
            {
                data = db.Ebooks.Where(x => x.ebook_id == bookid).Select(x => x.sub_id).FirstOrDefault();
            }
            if (sub == "ES")
            {
                data = db.Essays.Where(x => x.essay_id == bookid).Select(x => x.sub_id).FirstOrDefault();
            }
            if (sub == "TH")
            {
                data = db.Theses.Where(x => x.thesis_id == bookid).Select(x => x.sub_id).FirstOrDefault();
            }
            return Ok(data);
        }

        //Upload Khóa Luận
        [Route("api/AdminAPI/UploadFilesThesis")]
        [HttpPost]
        public ThesisCreationResult UploadFilesThesis()
        {
            string strExtexsion = "";
            var httpPostedFile = HttpContext.Current.Request.Files["fileInput"];//lấy file
            if (httpPostedFile != null)
            {
                //đường dẫn lưu file
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Upload/"), httpPostedFile.FileName);//tên file
                strExtexsion = Path.GetExtension(httpPostedFile.FileName).Trim();//lấy đuôi file
                //lưu file vào đường dẫn
                httpPostedFile.SaveAs(fileSavePath);
            }

            var title = HttpContext.Current.Request["title"];
            var describe = HttpContext.Current.Request["describe"];
            var instructor = HttpContext.Current.Request["instructor"];
            var executor1 = HttpContext.Current.Request["student1"];
            var executor2 = HttpContext.Current.Request["student2"];
            var course = HttpContext.Current.Request["course"];
            var userid = HttpContext.Current.Request["userid"];
            var subid = HttpContext.Current.Request["subid"];
            var date_upload = DateTime.Now;
            int user_id = Convert.ToInt32(userid);
            int sub_id = Convert.ToInt32(subid);

            if (strExtexsion == ".pdf")//chỉ cho up pdf
            {
                var filename = db.Theses.Where(x => x.filename == httpPostedFile.FileName).FirstOrDefault();
                if (filename != null)
                {
                    return new ThesisCreationResult
                    {
                        IsSuccess = false,
                        Message = "Tên file đã bị trùng!"
                    };
                }
                else
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
                                executor1 = executor1,
                                executor2 = executor2,
                                //course = year,
                                filename = httpPostedFile.FileName,
                                date_upload = date_upload,
                                user_id = user_id,
                                sub_id = sub_id,
                                cource = course,
                                countView = 0,
                                countDowload = 0
                            });
                        db.SaveChanges();
                        var book_id = db.Theses.OrderByDescending(x => x.id).Select(x => x.thesis_id).FirstOrDefault();
                        var userName = db.Users.Where(x => x.id == user_id).FirstOrDefault();
                        db.SearchFiles.Add(
                          new SearchFile
                          {
                              book_id = book_id,
                              title = title,
                              author = "",
                              year = "",
                              instructor = instructor,
                              executor1 = executor1,
                              executor2 = executor2,
                              describe = describe,
                              filename = httpPostedFile.FileName,
                              date_upload = date_upload,
                              user_id = user_id,
                              sub_id = sub_id,
                              username = userName.fullname,
                              type = "thesis",
                          });
                        db.SaveChanges();//lưu dât thôi cái này t chưa chạy t mới test gửi data từ  ajax qua thôi
                        var user = db.Users.Where(x => x.id == user_id).Select(x => x.username).FirstOrDefault();
                        var subject = db.Subject_Thesis.Where(x => x.id == sub_id).Select(x => x.name).FirstOrDefault();
                        var fileinfo = db.Theses.OrderByDescending(x => x.id).FirstOrDefault();
                        var date_up = date_upload.ToString("MM/dd/yyyy");
                        // MyHub.PostFileEssay(fileinfo.id, fileinfo.essay_id, fileinfo.title, fileinfo.instructor, fileinfo.executor1, fileinfo.executor2, fileinfo.describe, fileinfo.filename, date_up, user, subject);

                        var loadessay = db.Theses.OrderByDescending(x => x.id).Take(1).FirstOrDefault();
                        var essay_id = db.Theses.Where(x => x.id == loadessay.id).Select(x => x.thesis_id).FirstOrDefault();
                        var sub = db.Subject_Thesis.Where(x => x.id == sub_id).FirstOrDefault();
                        //MyHub.Post(sub_ebook.id, sub_ebook.name);

                        return new ThesisCreationResult
                        {
                            IsSuccess = true,
                            Id = loadessay.id,
                            Thesis_Id = essay_id,
                            Title = loadessay.title,
                            Describe = loadessay.describe,
                            Instructor = loadessay.instructor,
                            Executor1 = loadessay.executor1,
                            Executor2 = loadessay.executor2,
                            FileName = loadessay.filename,
                            Date_Upload = loadessay.date_upload.Value,
                            Course = loadessay.cource,
                            Sub_Id = sub.id,
                            Sub_Name = sub.name,
                            User_Name = userName.fullname
                        };
                        //return Request.CreateResponse("Thành công");
                    }
                }
            }
            else
            {
                return new ThesisCreationResult
                {
                    IsSuccess = false,
                    Message = "Bạn phải chọn file pdf"
                };
            }
            // return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi!!!");

        }

        //[Route("api/AdminAPI/UploadFilesThesis")]
        //public ThesisCreationResult UploadFilesThesis()
        //{
        //    string strExtexsion = "";
        //    var httpPostedFile = HttpContext.Current.Request.Files["fileInput"];//lấy file
        //    if (httpPostedFile != null)
        //    {
        //        //đường dẫn lưu file
        //        var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Upload/"), httpPostedFile.FileName);//tên file
        //        strExtexsion = Path.GetExtension(httpPostedFile.FileName).Trim();//lấy đuôi file

        //        //lưu file vào đường dẫn
        //        httpPostedFile.SaveAs(fileSavePath);
        //    }

        //    var title = HttpContext.Current.Request["title"];
        //    var describe = HttpContext.Current.Request["describe"];
        //    var instructor = HttpContext.Current.Request["instructor"];
        //    var student1 = HttpContext.Current.Request["student1"];
        //    var student2 = HttpContext.Current.Request["student2"]; 
        //    var course = HttpContext.Current.Request["course"];
        //    var userid = HttpContext.Current.Request["userid"];
        //    var subid = HttpContext.Current.Request["subid"];
        //    var date_upload = DateTime.Now;
        //    int user_id = Convert.ToInt32(userid);
        //    int sub_id = Convert.ToInt32(subid);

        //    if (strExtexsion == ".pdf")//chỉ cho up pdf
        //    {
        //        using (LibraryOnlineFinalEntities db = new LibraryOnlineFinalEntities())
        //        {
        //            //Add vô bảng ebook 
        //            db.Theses.Add(
        //                new Thesis
        //                {
        //                    thesis_id = "",
        //                    title = title,
        //                    describe = describe,
        //                    instructor = instructor,
        //                    executor1 = student1,
        //                    executor2 = student2,
        //                    filename = httpPostedFile.FileName,
        //                    date_upload = date_upload,
        //                    user_id = user_id,
        //                    sub_id = sub_id,
        //                    cource = course,
        //                    countView = 0,
        //                    countDowload = 0
        //                });
        //            db.SaveChanges();
        //            var book_id = db.Theses.OrderByDescending(x => x.id).Select(x => x.thesis_id).FirstOrDefault();

        //            db.SearchFiles.Add(
        //              new SearchFile
        //              {
        //                  book_id = book_id,
        //                  title = title,
        //                  author = "",
        //                  year = "",
        //                  instructor = instructor,
        //                  executor1 = student1,
        //                  executor2 = student2,
        //                  describe = describe,
        //                  filename = httpPostedFile.FileName,
        //                  date_upload = date_upload,
        //                  user_id = user_id,
        //                  sub_id = sub_id,
        //                  type = "thesis"
        //              });
        //            db.SaveChanges();//lưu dât thôi cái này t chưa chạy t mới test gửi data từ  ajax qua thôi
        //            var user = db.Users.Where(x => x.id == user_id).Select(x => x.username).FirstOrDefault();
        //            var subject = db.Subject_Thesis.Where(x => x.id == sub_id).Select(x => x.name).FirstOrDefault();
        //            var fileinfo = db.Theses.OrderByDescending(x => x.id).FirstOrDefault();
        //            var date_up = date_upload.ToString("dd/MM/yyyy");
        //            MyHub.PostFileThesis(fileinfo.id, fileinfo.thesis_id, fileinfo.title, fileinfo.instructor, fileinfo.executor1,
        //                fileinfo.executor1, fileinfo.describe, fileinfo.filename, date_up, user, subject);
        //            var loadthesis = db.Theses.OrderByDescending(x => x.id).Take(1).FirstOrDefault();
        //            var thesis_id = db.Theses.Where(x => x.id == loadthesis.id).Select(x => x.thesis_id).FirstOrDefault();

        //            return new ThesisCreationResult
        //            {
        //                IsSuccess = true,
        //                Id = loadthesis.id,
        //                Thesis_Id = thesis_id,
        //                Title = loadthesis.title,
        //                Describe = loadthesis.describe,
        //                Instructor = loadthesis.instructor,
        //                Executor1 = loadthesis.executor1,
        //                Executor2 = loadthesis.executor2,
        //                FileName = loadthesis.filename,
        //                Date_Upload = loadthesis.date_upload.Value.ToString("dd/MM/yyyy"),
        //                Course = loadthesis.cource
        //            };
        //            //return Request.CreateResponse("Thành công");
        //        }
        //    }
        //    return new ThesisCreationResult
        //    {
        //        IsSuccess = false,
        //    };
        //        // return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lỗi!!!");

        //    }

        public class ViewAndUploadModel
        {
            public string ID { get; set; }
            public int count { get; set; }
        }
        [Route("api/AdminAPI/UpdateViewEbook")]
        [HttpPost]
        public IHttpActionResult UpdateViewEbook(ViewAndUploadModel model)
        {
            var temp = db.Ebooks.Where(x => x.ebook_id == model.ID).FirstOrDefault();
            temp.countView = model.count;
            db.SaveChanges();
            return Ok();
        }
        [Route("api/AdminAPI/UpdateDowEbook")]
        [HttpPost]
        public IHttpActionResult UpdateDowEbook(ViewAndUploadModel model)
        {
            var temp = db.Ebooks.Where(x => x.ebook_id == model.ID).FirstOrDefault();
            temp.countDowload = model.count;
            db.SaveChanges();
            return Ok();
        }
        [Route("api/AdminAPI/UpdateViewEssay")]
        [HttpPost]
        public IHttpActionResult UpdateViewEssay(ViewAndUploadModel model)
        {
            var temp = db.Essays.Where(x => x.essay_id == model.ID).FirstOrDefault();
            temp.countView = model.count;
            db.SaveChanges();
            return Ok();
        }
        [Route("api/AdminAPI/UpdateDowEssay")]
        [HttpPost]
        public IHttpActionResult UpdateDowEssay(ViewAndUploadModel model)
        {
            var temp = db.Essays.Where(x => x.essay_id == model.ID).FirstOrDefault();
            temp.countDowload = model.count;
            db.SaveChanges();
            return Ok();
        }
        [Route("api/AdminAPI/UpdateViewThesis")]
        [HttpPost]
        public IHttpActionResult UpdateViewThesis(ViewAndUploadModel model)
        {
            var temp = db.Theses.Where(x => x.thesis_id == model.ID).FirstOrDefault();
            temp.countView = model.count;
            db.SaveChanges();
            return Ok();
        }
        [Route("api/AdminAPI/UpdateDowThesis")]
        [HttpPost]
        public IHttpActionResult UpdateDowThesis(ViewAndUploadModel model)
        {
            var temp = db.Theses.Where(x => x.thesis_id == model.ID).FirstOrDefault();
            temp.countDowload = model.count;
            db.SaveChanges();
            return Ok();
        }

        // lấy dữ liệu test
        [Route("api/AdminAPI/GetEssay1")]
        [HttpGet]
        public IEnumerable<Essay> GetEssay1()
        {

            return db.Essays.OrderByDescending(x => x.date_upload).ToList().Take(6);
        }
        // lấy dữ liệu test
        [Route("api/AdminAPI/GetEbook1")]
        [HttpGet]
        public IEnumerable<Ebook> GetEbook1()
        {
            return db.Ebooks.OrderByDescending(x => x.date_upload).ToList().Take(6);
        }

        // lấy dữ liệu test
        [Route("api/AdminAPI/GetThesis1")]
        [HttpGet]
        public IEnumerable<Thesis> GetThesis1()
        {

            return db.Theses.OrderByDescending(x => x.date_upload).ToList().Take(6);
        }
        // lấy dữ liệu test
        [Route("api/AdminAPI/GetRole")]
        [HttpGet]
        public IEnumerable<Role> GetRole()
        {
            return db.Roles.ToList();
        }
        // lấy ebook tiểu luận khóa luận gần nhất
        [Route("api/AdminAPI/GetListRecently")]
        [HttpGet]
        public IHttpActionResult GetListRecently()
        {
            //var ebook = db.Ebooks.OrderByDescending(x => x.date_upload).ToList().Take(6);
            //var essay = db.Essays.OrderByDescending(x => x.date_upload).ToList().Take(6);
            //var thesis = db.Theses.OrderByDescending(x => x.date_upload).ToList().Take(6);
            var ebook = (from e in db.Ebooks
                         from u in db.Users
                         where e.user_id == u.id
                         orderby e.date_upload descending
                         select new
                         {
                             Title = e.title,
                             Ebook_Id = e.ebook_id,
                             Poster = u.fullname
                         }).Take(6);
            var essay = (from e in db.Essays
                         from u in db.Users
                         where e.user_id == u.id
                         orderby e.date_upload descending
                         select new
                         {
                             Title = e.title,
                             Essay_Id = e.essay_id,
                             Poster = u.fullname
                         }).Take(6);
            var thesis = (from e in db.Theses
                          from u in db.Users
                          where e.user_id == u.id
                          orderby e.date_upload descending
                          select new
                          {
                              Title = e.title,
                              Thesis_Id = e.thesis_id,
                              Poster = u.fullname
                          }).Take(6);
            var loadslide = db.SlideImages.OrderBy(x => x.sortid).ToList();
            var data = new
            {
                lstebook = ebook,
                lstessay = essay,
                lstthesis = thesis,
                loadSlide = loadslide 
            };
            return Ok(data);
        }


        // lấy ebook tiểu luận khóa luận gần nhất
        [Route("api/AdminAPI/ChangeInfo")]
        [HttpPost]
        public IHttpActionResult ChangeInfo()
        {
            var httpPostedFile = HttpContext.Current.Request.Files["fileInputImage"];//lấy file
            var userid = HttpContext.Current.Request["userid"];
            int user_id = Convert.ToInt32(userid);
            var fullname = HttpContext.Current.Request["fullname"];
            var user = db.Users.Where(x => x.id == user_id).FirstOrDefault();
            if (httpPostedFile != null)
            {
                user.image = httpPostedFile.FileName;
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/imageUser/"), httpPostedFile.FileName);//tên file
                httpPostedFile.SaveAs(fileSavePath);
            }
            user.fullname = fullname;
            db.SaveChanges();
            var seacrh = db.SearchFiles.Where(x => x.user_id == user_id).ToList();
            foreach (var item in seacrh)
            {
                item.username = fullname;
                db.SaveChanges();
            }
            var userInfo = db.Users.Where(x => x.id == user_id).FirstOrDefault();
            return Ok(userInfo);
        }
        //Load Slide Image
        [Route("api/AdminAPI/LoadSLideImage")]
        [HttpGet]
        public IHttpActionResult LoadSLideImage()
        {
            var data = db.SlideImages.ToList();
            return Ok(data);
        }
        //Thêm Slide Image
        [Route("api/AdminAPI/AddSLideImage")]
        [HttpPost]
        public IHttpActionResult AddSLideImage()
        {
            string strExtexsion = "";
            var httpPostedFile = HttpContext.Current.Request.Files["fileSlideImage"];//lấy file
            if (httpPostedFile != null)
            {
                //đường dẫn lưu file
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/slideImage/"), httpPostedFile.FileName);//tên file
                //lưu file vào đường dẫn
                strExtexsion = Path.GetExtension(httpPostedFile.FileName).Trim();//lấy đuôi file
                httpPostedFile.SaveAs(fileSavePath);
            }

            var title = HttpContext.Current.Request["title"];
            var link = HttpContext.Current.Request["link"];
            var sort = HttpContext.Current.Request["sort"];
            int sort_id = Convert.ToInt32(sort);
            var checkSort = db.SlideImages.Where(x => x.sortid == sort_id).FirstOrDefault();
            if(checkSort == null)
            {
                db.SlideImages.Add(new SlideImage
                {
                    title = title,
                    link = link,
                    sortid = sort_id,
                    image = httpPostedFile.FileName
                });
                db.SaveChanges();
                var slideImg = db.SlideImages.OrderByDescending(x => x.id).FirstOrDefault();
                var data = new
                {
                    Flag = true,
                    Title = slideImg.title,
                    Link = slideImg.link,
                    SortId = slideImg.sortid,
                    Image = slideImg.image,
                    Id = slideImg.id
                };
                return Ok(data);
            }
            var data1 = new
            {
                Flag = false
            };
            return Ok(data1);
        }
        //Sửa Slide Image
        [Route("api/AdminAPI/EditSLideImage")]
        [HttpPost]
        public IHttpActionResult EditSLideImage()
        {
            string strExtexsion = "";
            var httpPostedFile = HttpContext.Current.Request.Files["fileSlideImageEdit"];//lấy file
            if (httpPostedFile != null)
            {
                //đường dẫn lưu file
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/slideImage/"), httpPostedFile.FileName);//tên file
                //lưu file vào đường dẫn
                strExtexsion = Path.GetExtension(httpPostedFile.FileName).Trim();//lấy đuôi file
                httpPostedFile.SaveAs(fileSavePath);
            }

            var title = HttpContext.Current.Request["title"];
            var link = HttpContext.Current.Request["link"];
            var sort = HttpContext.Current.Request["sort"];
            var id = HttpContext.Current.Request["id"];
            int sort_id = Convert.ToInt32(sort);
            int idImg = Convert.ToInt32(id);
            var slide = db.SlideImages.Where(x => x.id == idImg).FirstOrDefault();
            if (slide != null)
            {
                if(slide.sortid == sort_id)
                {
                    slide.title = title;
                    if(httpPostedFile != null)
                    {
                        slide.image = httpPostedFile.FileName;
                    }
                   
                    slide.link = link;
                    db.SaveChanges();
                    var data = new
                    {
                        Flag = true,
                        Title = title,
                        Link = link,
                        SortId = sort,
                        Image = slide.image
                    };
                    return Ok(data);
                }
                else
                {
                    var checkSort = db.SlideImages.Where(x => x.sortid == sort_id && x.id != idImg).FirstOrDefault();
                    if(checkSort != null)
                    {
                        var data2 = new
                        {
                            Flag = false
                        };
                        return Ok(data2);
                    }
                    else
                    {
                        slide.title = title;
                        if (httpPostedFile != null)
                        {
                            slide.image = httpPostedFile.FileName;
                        }
                        slide.link = link;
                        slide.sortid = sort_id;
                        db.SaveChanges();
                        var data3 = new
                        {
                            Flag = true,
                            Title = title,
                            Link = link,
                            SortId = sort,
                            Image = slide.image
                        };
                        return Ok(data3);
                    }
                }
            }
            var data1 = new
            {
                Flag = false
            };
            return Ok(data1);
        }

        //Sửa Slide Image
        [Route("api/AdminAPI/DeleteSLideImage")]
        [HttpPost]
        public IHttpActionResult DeleteSLideImage(int id)
        {
            var slide = db.SlideImages.Where(x => x.id == id).FirstOrDefault();
            if(slide!=null)
            {
                db.SlideImages.Remove(slide);
                db.SaveChanges();
            }
            
            return Ok("Xóa thành công!");
        }
     }
}
