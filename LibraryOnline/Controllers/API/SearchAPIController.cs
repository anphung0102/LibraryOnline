﻿using LibraryOnline.Models;
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
        private LibraryOnlineFinalEntities db = new LibraryOnlineFinalEntities();
        [Route("api/SearchAPI/Search")]
        [HttpGet]
        public IEnumerable<SearchFile> Search(string search)
        {
            var data = db.SearchFiles.Where(x => x.title.Contains(search) ||
             x.author.Contains(search) ||
            x.describe.Contains(search)).ToList();
            return data;
        }
    }
}