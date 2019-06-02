using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryOnline.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public int Role_Id { get; set; }
        public string FullName { get; set; }
        public string Mssv { get; set; }
        public string Class_Id { get; set; }
        public string RoleName { get; set; } 

    }
    public class UserCreationResult : UserViewModel
    {
        public bool IsSuccess { get; set; }
    }
}