using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryOnline.Models
{
    public class ThesisViewModel
    {
        public int Id { get; set; }
        public string Thesis_Id { get; set; }
        public string Title { get; set; }
        public string Instructor { get; set; }
        public string Executor1 { get; set; }
        public string Executor2 { get; set; }
        public string Describe { get; set; }
        public string FileName { get; set; }
        public DateTime Date_Upload { get; set; }
        public int User_Id { get; set; }
        public int Sub_Id { get; set; }
    }
    public class ThesisCreationResult : ThesisViewModel
    {
        public bool IsSuccess { get; set; }
    }
}