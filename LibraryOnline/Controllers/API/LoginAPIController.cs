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
        // Mã hóa MK
        public static byte[] encryptData(string data)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashedBytes;
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(data));
            return hashedBytes;
        }
        public static string md5(string data)
        {
            return BitConverter.ToString(encryptData(data)).Replace("-", "").ToLower();
        }

        private LibraryOnlineFinalEntities db = new LibraryOnlineFinalEntities();
        [Route("api/LoginAPI/Login")]
        [HttpPost]
        public string Login(LoginInfo loginInfo)
        {
            loginInfo.Pass = md5(loginInfo.Pass);
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
                    if (user.role_id == 1|| user.role_id == 2)
                        return "/Home/Index";
                    //else if (user.role_id == 2)
                    //{
                    //    return "/Lecturers/Index";
                    //}
                    else if (user.role_id == 3)
                    {
                        return "/Home/Student";
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

        public class PassInfo
        {
            public string email { get; set; }
            public string passOld { get; set; }
            public string passNew { get; set; }
        }
        public class ChangePassModel
        {
            public bool IsSuccess { get; set; }
            public string Message { get; set; }
        }

        [Route("api/LoginAPI/ChangePass")]
        [HttpPost]
        public ChangePassModel ChangePass(PassInfo model)
        {
            model.passOld = md5(model.passOld);
            var user = db.Users.Where(x => x.username == model.email && x.password == model.passOld).FirstOrDefault();
            if(user != null)
            {
                user.password = md5(model.passNew);
                db.SaveChanges();
                return new ChangePassModel {
                    IsSuccess = true,
                    Message = "Đổi mật khẩu thành công!"
                };
            }
            return new ChangePassModel
            {
                IsSuccess = false,
                Message = "Đổi mật khẩu thất bại!"
            };
        }
    }
}
