using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RinconArtesano.Models
{
    public class RatingViewModel
    {
        public int Rating { get; set; }
        public int ProductId { get; set; }
        public int ExperienceId { get; set; }
        public string UsersId { get; set; }
        public Decimal? RatingPromedio { get; set; }
        public int? RatingSelect { get; set; }
    }
}