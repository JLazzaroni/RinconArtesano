﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RinconArtesano.Models
{
    public class MessagePadreViewModel
    {
        public int? MessagePadreId { get; set; }
        public string Comentario { get; set; }
        public int? ProductId { get; set; }
        public int? ExperienceId { get; set; }
    }
}