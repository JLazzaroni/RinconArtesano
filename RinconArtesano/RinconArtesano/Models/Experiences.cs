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
    
    public partial class Experiences
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Experiences()
        {
            this.Files = new HashSet<Files>();
        }
    
        public int ExperienceId { get; set; }
        public string UsersId { get; set; }
        public string ExperienceTitle { get; set; }
        public string ExperienceDescription { get; set; }
        public Nullable<System.DateTime> DateNull { get; set; }
        public System.DateTime DateAdd { get; set; }
        public Nullable<System.DateTime> DateModification { get; set; }
        public string YoutubePath { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Files> Files { get; set; }
    }
}
