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
        private LibraryOnlineEntities db = new LibraryOnlineEntities();
        [Route("api/LoginAPI/Login")]
        [HttpPost]
        public string Login(LoginInfo loginInfo)
        {
            using (LibraryOnlineEntities db = new LibraryOnlineEntities())
            {
                var role = db.USERS.Where(x => x.username == loginInfo.User && x.pass == loginInfo.Pass)
                          .Select(x => x.rode_id).FirstOrDefault();
                var user_id = db.USERS.Where(x => x.username == loginInfo.User && x.pass == loginInfo.Pass).FirstOrDefault();
                if(user_id != null)
                {
                    HttpContext.Current.Session["username"] = loginInfo.User;
                    HttpContext.Current.Session["user_id"] = user_id.id;
                    HttpContext.Current.Session["fullname"] = user_id.fullname;
                }
                if (role == 1)
                    return "/Admin/Admin";

            }
            return "";
        }


    }
}
