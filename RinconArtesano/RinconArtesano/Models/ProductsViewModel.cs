using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Datos;

namespace RinconArtesano.Models
{
    public class ProductsViewModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductsViewModel()
        {
            this.Files = new HashSet<Files>();
            this.Denuncias = new HashSet<Denuncias>();
        }
        public int ProductId { get; set; }
        public string UsersId { get; set; }
        public string ProductTitle { get; set; }
        public string ProductDescription { get; set; }
        public int IdCategory { get; set; }
        public Nullable<System.DateTime> DateNull { get; set; }
        public System.DateTime DateAdd { get; set; }
        public Nullable<System.DateTime> DateModification { get; set; }
        public bool IsBlocked { get; set; }
        public bool Denuncia { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual ProductsCategories ProductsCategories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Files> Files { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Denuncias> Denuncias { get; set; }
    }
}