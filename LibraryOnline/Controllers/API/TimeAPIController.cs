using LibraryOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LibraryOnline.Controllers.API
{
    public class TimeAPIController : ApiController
    {
        private LibraryOnlineFinalEntities db = new LibraryOnlineFinalEntities();
        public class Model
        {
            public int UserId { get; set; }
            public string BookId { get; set; }
            public string Time { get; set; }
        }
        [Route("api/TimeAPI/SaveTime")]
        [HttpPost]
        public IHttpActionResult SaveTime(Model model)
        {
            var ebook = db.Ebooks.Where(x => x.ebook_id == model.BookId).Select(x=>x.title).FirstOrDefault();
            var essay = db.Essays.Where(x => x.essay_id == model.BookId).Select(x => x.title).FirstOrDefault();
            var thesis = db.Theses.Where(x => x.thesis_id == model.BookId).Select(x => x.title).FirstOrDefault();
            var bookname = "";
            if(ebook != null)
            {
                bookname = ebook;
            }
            if (essay != null)
            {
                bookname = essay;
            }
            if (thesis != null)
            {
                bookname = thesis;
            }
            db.Times.Add(new Time
            {
                userid = model.UserId,
                bookid = model.BookId,
                bookname = bookname,
                time1 = model.Time,
                date = DateTime.Now
            });
            db.SaveChanges();
            return Ok();
        }

        [Route("api/TimeAPI/ManageStudentReadBook")]
        [HttpGet]
        public IHttpActionResult ManageStudentReadBook()
        {
            var data = from user in db.Users
                       from time in db.Times
                       where user.id == time.userid
                       select new
                       {
                           time.id,
                           user.fullname,
                           user.class_id,
                           time.bookname,
                           time.time1,
                           time.date

                       };
            
            return Ok(data);
        }

        [Route("api/TimeAPI/StudentView")]
        [HttpGet]
        public IEnumerable<Time> StudentView(int id)
        {

            return db.Times.Where(x => x.userid == id).ToList();
        }
    }
}
