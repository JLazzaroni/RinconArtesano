//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Datos
{
    using System;
    using System.Collections.Generic;
    
    public partial class Files
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string FilePath { get; set; }
        public Nullable<short> FileType { get; set; }
        public string UsersId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Nullable<int> ExperienceId { get; set; }
        public Nullable<System.DateTime> DateNull { get; set; }
        public System.DateTime DateAdd { get; set; }
        public Nullable<System.DateTime> DateModification { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Experiences Experiences { get; set; }
        public virtual Products Products { get; set; }
    }
}
