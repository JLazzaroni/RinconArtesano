//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RinconArtesano.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductsCategories
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductsCategories()
        {
            this.Products = new HashSet<Products>();
        }
    
        public int ProductCategoryId { get; set; }
        public string ProductCategoryDescriptions { get; set; }
        public string ProductCategoryName { get; set; }
        public System.DateTime DateAdd { get; set; }
        public Nullable<System.DateTime> DateNull { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Products> Products { get; set; }
    }
}
