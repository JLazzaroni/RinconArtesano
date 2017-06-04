using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RinconArtesano.Models
{
    public class MessageHijoViewModel
    {
        public int IdComentario { get; set; }
        public int? IdComentarioPadre { get; set; }
        public string Message { get; set; }
        public int? ProductId { get; set; }
        public int? ExperienceId { get; set; }
        public string UsersId { get; set; }
        public string UsersName { get; set; }
        public DateTime DateAdd { get; set; }
        public DateTime? DateNull { get; set; }
        public bool IsBlocked { get; set; }
        public bool ComentarioDenuncio { get; set; }
    }
}