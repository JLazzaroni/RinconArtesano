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
    
    public partial class Denuncias
    {
        public int DenunciaId { get; set; }
        public string Comentario { get; set; }
        public string UsersId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Nullable<int> ExperienceId { get; set; }
        public Nullable<System.DateTime> DateNull { get; set; }
        public System.DateTime DateAdd { get; set; }
        public string UsersIdDenunciado { get; set; }
        public Nullable<int> ComentarioId { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Experiences Experiences { get; set; }
        public virtual Products Products { get; set; }
        public virtual AspNetUsers AspNetUsers1 { get; set; }
        public virtual Messages Messages { get; set; }
    }
}
