using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryOnline.Models
{
    public class EbookViewModel
    {
        public int Id { get; set; }
        public string Ebook_Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Year { get; set; }
        public string Describe { get; set; }
        public string FileName { get; set; }
        public  DateTime Date_Upload { get; set; }
        public int User_Id { get; set; }
        public int Sub_Id { get; set; }
        //public HttpPostedFileBase FileReferences { get; set; }
    }
    public class EbookCreationResult : EbookViewModel
    {
        public bool IsSuccess { get; set; }
    }
    public class Post
    {
        public EbookViewModel ebook { get; set; }
        //public HttpPostedFileBase file { get; set; }
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
    }
}