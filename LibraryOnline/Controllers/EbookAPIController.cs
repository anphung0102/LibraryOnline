using LibraryOnline.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LibraryOnline.Controllers
{
    public class EbookAPIController : ApiController
    {
        private LibraryEntities db = new LibraryEntities();
        [Route("api/EbookAPI/GetSubjectEbook")]
        [HttpGet]
        public IEnumerable<Subject_Ebook> GetSubjectEbook()
        {
            var a = db.Subject_Ebook.ToList();
            return a;
        }

        //sửa ebook
        [Route("api/EbookAPI/EditSubjectById")]
        [HttpPost]
        public string EditSubjectById(Subject_Ebook subject)
        {
            var sub = db.Subject_Ebook.Where(x => x.id == subject.id).FirstOrDefault();

            if (sub != null)
            {
                sub.name = subject.name;
                db.SaveChanges();
                return "Sửa thành công";
            }
            else
            {
                return "Sửa không thành công";
            }
        }

        ////Lấy môn học của ebook
        [Route("api/EbookAPI/LoadSubjectEbook")]
        [HttpGet]
        public IEnumerable<Subject_Ebook> LoadSubjectEbook()
        {
            var a = db.Subject_Ebook.ToList();
            return a;
        }

        //xoá ebook
        [Route("api/EbookAPI/DeleteSubjectById")]
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
            db.Subject_Ebook.Remove(sub);
            db.SaveChanges();

            return "Xóa thành công";
        }
        

    }
}
