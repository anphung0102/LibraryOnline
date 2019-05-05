using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryOnline.Models
{
    public class SubjectViewModel
    {
        public int Id { get; set; }
        public string Subessay_Id { get; set; }
        public string Subthesis_Id { get; set; }
        public string Subebook_Id { get; set; }
        public string Name { get; set; }
    }

    public class SubjectCreationResult : SubjectViewModel
    {
        public bool IsSuccess { get; set; }
    }

}