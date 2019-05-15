using LibraryOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace LibraryOnline.Controllers.API
{
    public class LoginAPIController : ApiController
    {
        public class LoginInfo
        {
            public string User { get; set; }
            public string Pass { get; set; }
        }
        private LibraryOnlineFinalEntities db = new LibraryOnlineFinalEntities();
        [Route("api/LoginAPI/Login")]
        [HttpPost]
        public string Login(LoginInfo loginInfo)
        {
            using (LibraryOnlineFinalEntities db = new LibraryOnlineFinalEntities())
            {
                var user = db.Users.Where(x => x.username == loginInfo.User && x.password == loginInfo.Pass)
                          .FirstOrDefault();
                //var user_id = db.Users.Where(x => x.username == loginInfo.User && x.password == loginInfo.Pass).FirstOrDefault();
                if(user != null)
                {
                    HttpContext.Current.Session["username"] = loginInfo.User;
                    HttpContext.Current.Session["user_id"] = user.id;
                    HttpContext.Current.Session["fullname"] = user.fullname;
                    HttpContext.Current.Session["role_id"] = user.role_id;
                    if (user.role_id == 1)
                        return "/Admin/Admin";
                    else if (user.role_id == 2)
                    {
                        return "/Lecturers/Index";
                    }
                    else if (user.role_id == 3)
                    {
                        return "/Student/Index";
                    }
                }
            }
            return "";
        }

        [Route("api/LoginAPI/Logout")]
        [HttpGet]
        public string Logout()
        {
            HttpContext.Current.Session["username"] = "";
            HttpContext.Current.Session["user_id"] = "";
            HttpContext.Current.Session["fullname"] = "";
            HttpContext.Current.Session["role_id"] = "";
            return "/Login/Index";
        }
    }
}
