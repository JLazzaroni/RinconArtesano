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
    
    public partial class MessagesHijos
    {
        public int IdMessageHijo { get; set; }
        public int IdMessagePadre { get; set; }
        public string UsersId { get; set; }
        public string Message { get; set; }
        public Nullable<int> DenounceCount { get; set; }
        public System.DateTime DateAdd { get; set; }
        public Nullable<System.DateTime> DateNull { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual MessagesPadres MessagesPadres { get; set; }
    }
}
