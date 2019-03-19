using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryOnline.Models
{
    public class EbookViewModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Year { get; set; }
        public string Describe { get; set; }
        public HttpPostedFileBase FileReferences { get; set; }
    }

    public class Post
    {
        public EbookViewModel ebook { get; set; }
        //public HttpPostedFileBase file { get; set; }
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
    }
}