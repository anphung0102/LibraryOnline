using LibraryOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LibraryOnline.Controllers.API
{
    public class RateAPIController : ApiController
    {
        private LibraryOnlineFinalEntities db = new LibraryOnlineFinalEntities();
        [Route("api/RateAPI/GetRateStar")]
        [HttpGet]
        public object GetRateStar(string book_id)
        {
            var oneStarCount = db.RateStars.Where(x => x.rate == 1 && x.book_id == book_id).Count();
            var listRateOneStar = (from r in db.RateStars
                                   from u in db.Users
                                   where r.user_id == u.id && r.rate == 1 && r.book_id == book_id
                                   select u.fullname).ToList();
            var twoStarCount = db.RateStars.Where(x => x.rate == 2 && x.book_id == book_id).Count();
            var listRateTwoStar = (from r in db.RateStars
                                   from u in db.Users
                                   where r.user_id == u.id && r.rate == 2 && r.book_id == book_id
                                   select u.fullname).ToList();
            var threeStarCount = db.RateStars.Where(x => x.rate == 3 && x.book_id == book_id).Count();
            var listRateThreeStar = (from r in db.RateStars
                                   from u in db.Users
                                   where r.user_id == u.id && r.rate == 3 && r.book_id == book_id
                                     select u.fullname).ToList();
            var fourStarCount = db.RateStars.Where(x => x.rate == 4 && x.book_id == book_id).Count();
            var listRateFourStar = (from r in db.RateStars
                                   from u in db.Users
                                   where r.user_id == u.id && r.rate == 4 && r.book_id == book_id
                                    select u.fullname).ToList();
            var fiveStarCount = db.RateStars.Where(x => x.rate == 5 && x.book_id == book_id).Count();
            var listRateFiveStar = (from r in db.RateStars
                                    from u in db.Users
                                    where r.user_id == u.id && r.rate == 5 && r.book_id == book_id
                                    select u.fullname).ToList();
            MyHub.LoadNumberStar(oneStarCount, twoStarCount, threeStarCount,
                fourStarCount, fiveStarCount);
            return new {
                oneStarCount,
                listRateOneStar,
                twoStarCount,
                listRateTwoStar,
                threeStarCount,
                listRateThreeStar,
                fourStarCount,
                listRateFourStar,
                fiveStarCount,
                listRateFiveStar,
            };
        }
    }
}
