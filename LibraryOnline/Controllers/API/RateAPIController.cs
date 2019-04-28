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
        [Route("api/RateAPIAPI/GetRateStar")]
        [HttpGet]
        public object GetRateStar()
        {
            var oneStarCount = db.RateStars.Where(x => x.rate == 1).Count();
            var listRateOneStar = (from r in db.RateStars
                                   from u in db.Users
                                   where r.user_id == u.id && r.rate == 1
                                   select u.fullname).ToList();
            var twoStarCount = db.RateStars.Where(x => x.rate == 2).Count();
            var listRateTwoStar = (from r in db.RateStars
                                   from u in db.Users
                                   where r.user_id == u.id && r.rate == 2
                                   select u.fullname).ToList();
            var threeStarCount = db.RateStars.Where(x => x.rate == 3).Count();
            var listRateThreeStar = (from r in db.RateStars
                                   from u in db.Users
                                   where r.user_id == u.id && r.rate == 3
                                     select u.fullname).ToList();
            var fourStarCount = db.RateStars.Where(x => x.rate == 4).Count();
            var listRateFourStar = (from r in db.RateStars
                                   from u in db.Users
                                   where r.user_id == u.id && r.rate == 4
                                    select u.fullname).ToList();
            var fiveStarCount = db.RateStars.Where(x => x.rate == 5).Count();
            var listRateFiveStar = (from r in db.RateStars
                                    from u in db.Users
                                    where r.user_id == u.id && r.rate == 5
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
