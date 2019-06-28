using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryOnline.Models
{
    public class RateStarModel
    {
        public int Id { get; set; }
        public string BookId { get; set; }
        public string Username { get; set; }
        public int UserId { get; set; }
        public int Rate { get; set; }
        public int SubId { get; set; } 
    }

    public class RateStarResult : RateStarModel 
    {
        public bool IsSuccess { get; set; }
    }

}