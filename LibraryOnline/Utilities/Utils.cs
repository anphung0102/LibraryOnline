using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LibraryOnline.Utilities
{
    public static class Utils
    {
        public static string CreateAttachment(string userId, HttpPostedFileBase file)
        {
            //userId để biết ai tạo nha
            string folderName = "Attachment";
            string virtualPath = "~/Content/file/" + folderName + userId;
            string FolderPath = HttpContext.Current.Server.MapPath(virtualPath);
            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);
            //string FilePath = Path.Combine(FolderPath, file.FileName);
            file.SaveAs(Path.Combine(FolderPath, file.FileName));
            return virtualPath + "/" + file.FileName;
        }
        //cách hay nhất: e tạo 1 table FileUpload
        //trong đó chỉ có các field: id(int), Link (string), 
        //FileName (string), FileSize (int), .. à đúng rồi phải có cái khóa ngoại,
        // để biết nó thuộc cái nào.
        //trong đó chưa các thông tin file.
        // còn file thì lưu trong project

        //đợi a chut
    }
}