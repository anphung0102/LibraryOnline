using LibraryOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace LibraryOnline.Controllers.API
{
    public class SearchAPIController : ApiController
    {
        private LibraryOnlineFinalEntities db = new LibraryOnlineFinalEntities();
        //private string ConvertToUnSign(string input)
        //{
        //    input = input.Trim();
        //    for (int i = 0x20; i < 0x30; i++)
        //    {
        //        input = input.Replace(((char)i).ToString(), " ");
        //    }
        //    System.Text.RegularExpressions.Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
        //    string str = input.Normalize(NormalizationForm.FormD);
        //    string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
        //    while (str2.IndexOf("?") >= 0)
        //    {
        //        str2 = str2.Remove(str2.IndexOf("?"), 1);
        //    }
        //    return str2;
        //}
        public static string ConvertToUnSign(string text)
        {
            for (int i = 33; i < 48; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }
            for (int i = 58; i < 65; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }
            for (int i = 91; i < 97; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }
            for (int i = 123; i < 127; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }
            //text = text.Replace(" ", "-"); //Comment lại để không đưa khoảng trắng thành ký tự -
            
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            
            string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);
            return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        [Route("api/SearchAPI/Search")]
        [HttpGet]
        public IHttpActionResult Search(string search)
        {
            string strSearch = ConvertToUnSign(search).ToLower();
            var query = db.SearchFiles.Where(delegate (SearchFile s)
            {
                if (ConvertToUnSign(s.title.ToLower()).Contains(strSearch) ||
                ConvertToUnSign(s.username.ToLower()).Contains(strSearch) ||
                ConvertToUnSign(s.describe.ToLower()).Contains(strSearch) )
                    return true;
                else
                    return false;
            });
            return Ok(query);
        }
        //var query = db.Categories.Where(delegate (Category c)
        //{
        //    if (ConvertToUnSign(c.Name).IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) >= 0)
        //        return true;
        //    else
        //        return false;
        //}).AsQueryable();
        //[Route("api/SearchAPI/Search")]
        //[HttpGet]
        //public IEnumerable<SearchFile> Search(string search)
        //{
        //    var data = db.SearchFiles.Where(x => x.title.Contains(search) ||
        //     x.author.Contains(search) ||
        //    x.describe.Contains(search)).ToList();
        //    return data;
        //}

    }
}
