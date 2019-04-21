using LibraryOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LibraryOnline.Controllers.API
{
    public class SearchAPIController : ApiController
    {
        private LibraryOnlineEntities db = new LibraryOnlineEntities();
        [Route("api/SearchAPI/Search")]
        [HttpGet]
        public IEnumerable<EBOOK> Search(string search)//nay gio nhâm cái nay :D
        {
            var data = db.EBOOKS.Where(x => x.title.Contains(search) ||
             x.author.Contains(search) ||
            x.describe.Contains(search)).ToList();
            return data;
        }
    }
}
