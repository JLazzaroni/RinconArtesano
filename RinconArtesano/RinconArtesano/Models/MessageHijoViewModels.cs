using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RinconArtesano.Models
{
    public class MessageHijoViewModel
    {
        public int? MessageHijoId { get; set; }
        public int MessagePadreId { get; set; }
        public string Comentario { get; set; }        
    }
}