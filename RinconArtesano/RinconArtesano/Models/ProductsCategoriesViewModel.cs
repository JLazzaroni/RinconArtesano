using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RinconArtesano.Models
{
    public class ProductsCategoriesViewModel
    {
        public int ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductCategoryDescriptions { get; set; }
        public string FilePath { get; set; }
    }
}