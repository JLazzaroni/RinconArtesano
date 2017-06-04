using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RinconArtesano.Models
{
    public class DenunciasViewModel
    {
        public string Comentario { get; set; }
        public int ProductId { get; set; }
        public int ExperienceId { get; set; }
        public int MessageId { get; set; }

        public string UserIdDenunciado { get; set; }
    }
}