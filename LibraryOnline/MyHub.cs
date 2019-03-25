using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public static void Post(int id, string name)
        {
            hubContext.Clients.All.postToClient(id, name);
        }
        public static void PostFileEbook(int id,string title,string author,string describe,string year, string filename,
            string date_upload)
        {
            hubContext.Clients.All.postFileEbookToClient(id, title, author, describe, year, filename,
            date_upload);
        }
    }
}