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
    
    public partial class UsersInfo
    {
        public string UsersId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public Nullable<System.DateTime> FechaNacimiento { get; set; }
        public Nullable<int> Edad { get; set; }
        public string Sexo { get; set; }
        public string Pais { get; set; }
        public string Provincia { get; set; }
        public string Localidad { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Intereses { get; set; }
        public string Rubro { get; set; }
        public string FilePath { get; set; }
        public Nullable<System.DateTime> DateNull { get; set; }
        public System.DateTime DateAdd { get; set; }
        public Nullable<System.DateTime> DateModification { get; set; }
        public bool IsBlocked { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
    }
}
