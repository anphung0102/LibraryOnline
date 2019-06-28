using LibraryOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.IO;
using System.Security.Cryptography;
using System.Text;

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
        //mã hóa và giải mã
        static string key { get; set; } = "A!9HHhi%XjjYY4YP2@Nob009X";
        public static string Encrypt(string text)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateEncryptor())
                    {
                        byte[] textBytes = UTF8Encoding.UTF8.GetBytes(text);
                        byte[] bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
                        return Convert.ToBase64String(bytes, 0, bytes.Length);
                    }
                }
            }
        }

        public static string Decrypt(string cipher)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateDecryptor())
                    {
                        byte[] cipherBytes = Convert.FromBase64String(cipher);
                        byte[] bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                        return UTF8Encoding.UTF8.GetString(bytes);
                    }
                }
            }
        }
        private LibraryOnlineFinalEntities db = new LibraryOnlineFinalEntities();
        [Route("api/LoginAPI/Login")]
        [HttpPost]
        public string Login(LoginInfo loginInfo)
        {
            //loginInfo.Pass = md5(loginInfo.Pass);
            loginInfo.Pass = Encrypt(loginInfo.Pass);
            using (LibraryOnlineFinalEntities db = new LibraryOnlineFinalEntities())
            {
                var user = db.Users.Where(x => x.username == loginInfo.User && x.password == loginInfo.Pass)
                          .FirstOrDefault();
                //var user_id = db.Users.Where(x => x.username == loginInfo.User && x.password == loginInfo.Pass).FirstOrDefault();
                if(user != null)
                {
                    HttpContext.Current.Session["username"] = loginInfo.User;
                    HttpContext.Current.Session["user_id"] = user.id;
                    //HttpContext.Current.Session["fullname"] = user.fullname;
                    HttpContext.Current.Session["role_id"] = user.role_id;
                    //HttpContext.Current.Session["image"] = user.image;
                    if (user.role_id == 1 || user.role_id == 2)
                        return "/Home/Index";
                    else if (user.role_id == 3)
                    {
                        return "/Home/Student";
                    }
                    //var data = new {
                    //    infouser = user,
                    //    pass = Decrypt(user.password)
                    //};
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

        [Route("api/LoginAPI/GetPassByUserId")]
        [HttpGet]
        public IHttpActionResult GetPassByUserId(int id)
        {
            var user = db.Users.Where(x => x.id == id).FirstOrDefault();
            var data = new {
                username = user.username,
                pass = Decrypt(user.password),
                classid = user.mssv,
                image = user.image,
                fullname = user.fullname,
                userid = user.id
                
            };
            return Ok(data);
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
            //model.passOld = md5(model.passOld);
            model.passOld = Encrypt(model.passOld);
            var user = db.Users.Where(x => x.username == model.email && x.password == model.passOld).FirstOrDefault();
            if(user != null)
            {
                //user.password = md5(model.passNew);
                user.password = Encrypt(model.passNew);
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
