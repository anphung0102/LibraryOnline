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
        ////sủa môn học 
        //public static void EditSubject()
        //{
        //    hubContext.Clients.All.postToClientEditSubject();
        //}
       
        //sủa môn học 
        public static void EditSubject(int id, string name)
        {
            hubContext.Clients.All.postToClientEditSubject(id,name);
        }
        //load môn học khi thêm mới
        public static void Post(int id, string name)
        {
            hubContext.Clients.All.postToClient(id, name);
        }
        //load file khi thêm mới
        public static void PostFileEbook(int id,string title,string author,string describe,string year, string filename,
            string date_upload)
        {
            hubContext.Clients.All.postFileEbookToClient(id, title, author, describe, year, filename,
            date_upload);
        }
    }
}