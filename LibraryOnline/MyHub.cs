using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LibraryOnline.Models;
using Microsoft.AspNet.SignalR;

namespace LibraryOnline
{
    public class MyHub : Hub
    {
        //public void Send(string name) 
        //{
        //    // Call the addNewMessageToPage method to update clients.
        //    Clients.All.addNewItemToPage(name);
        //}
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<MyHub>();

        //public void Hello()
        //{
        //    Clients.All.hello();
        //}
        //xóa môn học 
        public static void DeleteSubject(int id)
        {
            hubContext.Clients.All.postToClientDeleteSubject(id);
        }
        //sủa file ebook
        //public static void EditFileEbook(string title,string author,string describe,int year, string filename,string date_upload)
        //{
        //    hubContext.Clients.All.EditFileEbook(title,author,describe,year,filename,date_upload);
        //}

        //sủa môn học 
        public static void EditSubject(int id, string name)
        {
            hubContext.Clients.All.postToClientEditSubject(id, name);
        }
        //load môn học khi thêm mới
        public static void Post(int id, string name)
        {
            hubContext.Clients.All.postToClient(id, name);
        }
        //load file khi thêm mới
        public static void PostFileEbook(int id, string title, string author, string describe, string year, string filename,
            string date_upload, string user, string subject)
        {
            hubContext.Clients.All.postFileEbookToClient(id, title, author, describe, year, filename,
            date_upload, user, subject);
        }

        public static void PostFileEssay(int id, string essay_id, string title, string instructor, string executor1, string executor2, string describe, string filename,
            string date_upload, string user, string subject)
        {
            hubContext.Clients.All.postFileEssayToClient(id, essay_id, title, instructor, executor1, executor2, describe, filename,
            date_upload, user, subject);
        }

        //load file khi thêm mới KHÓA LUẬN
        public static void PostFileThesis(int id,string thesis_id, string title, string instructor, string executor1,
             string executor2, string describe, string filename,
            string date_upload, string user, string subject)
        {
            hubContext.Clients.All.postFileThesisToClient(id, thesis_id, title, instructor, executor1, executor1,
                describe, filename, date_upload, user, subject);
        }
        //load số sao được đánh giá
        public static void LoadNumberStar(int oneStar, int twoStar, int threeStar, 
            int fourStar, int fiveStar)
        {
            hubContext.Clients.All.loadNumberStar(oneStar,twoStar,threeStar,
            fourStar,fiveStar);
        }
    }
}