using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RinconArtesano.Models
{
    public class ProductDetailsViewModel
    {
        public int ProductId { get; set; }
        public string UsersId { get; set; }
        public string ProductTitle { get; set; }
        public string ProductDescription { get; set; }
        public int IdCategory { get; set; }
        public Nullable<System.DateTime> DateNull { get; set; }
        public System.DateTime DateAdd { get; set; }
        public Nullable<System.DateTime> DateModification { get; set; }
        public bool IsBlocked { get; set; }
        //public string ArtesanoFoto { get; set; }
        //public string ArtesanoNombre { get; set; }
        //public string ArtesanoApellido { get; set; }
        public virtual UsersInfo Artesano { get; set; }
        public virtual ICollection<Files> Files { get; set; }
        public virtual ProductsCategories ProductsCategories { get; set; }
    }
}