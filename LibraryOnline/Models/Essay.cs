//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LibraryOnline.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ESSAY
    {
        public int id { get; set; }
        public string essay_id { get; set; }
        public string title { get; set; }
        public string instructor { get; set; }
        public string executor1 { get; set; }
        public string executor2 { get; set; }
        public string describe { get; set; }
        public string filename { get; set; }
        public Nullable<System.DateTime> date_upload { get; set; }
        public Nullable<int> user_id { get; set; }
        public Nullable<int> sub_id { get; set; }
    
        public virtual SUBJECTESSAY SUBJECTESSAY { get; set; }
        public virtual USER USER { get; set; }
    }
}
