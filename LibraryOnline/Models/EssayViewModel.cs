using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryOnline.Models
{
    public class EssayViewModel
    {
        public int Id { get; set; }
        public string Essay_Id { get; set; }
        public string Title { get; set; }
        public string Instructor { get; set; }
        public string Executor1 { get; set; }
        public string Executor2 { get; set; }
        public string Describe { get; set; }
        public string FileName { get; set; }
        //public DateTime Date_Upload { get; set; }ToString("dd/MM/yyyy")
        public string Date_Upload { get; set; }
        public int User_Id { get; set; }
        public int Sub_Id { get; set; }
        public string Course { get; set; }
    }
    public class EssayCreationResult : EssayViewModel
    {
        public bool IsSuccess { get; set; }
    }
}