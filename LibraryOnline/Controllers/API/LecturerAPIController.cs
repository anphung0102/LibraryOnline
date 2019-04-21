using LibraryOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LibraryOnline.Controllers.API
{
    public class LecturerAPIController : ApiController
    {
        private LibraryEntities db = new LibraryEntities();
        //lấy ds ebook 
        [Route("api/LecturerAPI/GetEbook_Lecturer")]
        [HttpGet]
        public IEnumerable<Ebook> GetEbookDetail(int  id)
        {
            return db.Ebooks.Where(x => x.user_id == id).ToList();
        }
    }
}
