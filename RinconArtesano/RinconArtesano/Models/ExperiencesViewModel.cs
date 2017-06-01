using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Datos;


namespace RinconArtesano.Models
{
    public class ExperiencesViewModel
    {
        public int ExperienceId { get; set; }
        public string UsersId { get; set; }
        public string ExperienceTitle { get; set; }
        public string ExperienceDescription { get; set; }
        public Nullable<System.DateTime> DateNull { get; set; }
        public System.DateTime DateAdd { get; set; }
        public Nullable<System.DateTime> DateModification { get; set; }
        public string YoutubePath { get; set; }
        public bool IsBlocked { get; set; }
        public bool UsuarioDenuncio { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual ICollection<Denuncias> Denuncias { get; set; }
        public virtual ICollection<Files> Files { get; set; }
    }
}