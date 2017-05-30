using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RinconArtesano.Models;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using System.IO;
using Datos;

namespace RinconArtesano.Controllers
{
    public class MessagesController : Controller
    {
        private RinconArtesanoEntities db = new RinconArtesanoEntities();

        [Authorize]
        [HttpPost]
        public ActionResult CreateMessagePadre(MessagePadreViewModel messagePadre)
        {
            //Validaciones
            if (String.IsNullOrWhiteSpace(messagePadre.Comentario))
                ModelState.AddModelError("Comentario", "Error en el campo Comentario, debe ingresar un texto.");
            else if (messagePadre.Comentario.Length > 1000)
                ModelState.AddModelError("Comentario", "Error en el campo Comentario, el mismo no puede superar los 1000 caracteres.");

            if (!ModelState.IsValid)
            {
                var m = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { message = m, status = "Error" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {
                    string userId = User.Identity.GetUserId();
                    MessagesPadres mess = new MessagesPadres();
                    mess.UsersId = userId;
                    mess.Message = messagePadre.Comentario;

                    if (messagePadre.ProductId.HasValue)
                    {
                        mess.Category = 1;
                        mess.CategoryId = messagePadre.ProductId.Value;
                    }
                    else
                    {
                        mess.Category = 2;
                        mess.CategoryId = messagePadre.ExperienceId.Value;
                    }
                    mess.DateAdd = DateTime.Now;

                    db.MessagesPadres.Add(mess);
                    db.SaveChanges();
                    return Json(new { result = "OK" });
                }
                catch
                {
                    return Json(new { result = "ERROR" });
                }
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateMessageHijo(MessageHijoViewModel messageHijo)
        {
            //Validaciones
            if (String.IsNullOrWhiteSpace(messageHijo.Comentario))
                ModelState.AddModelError("Comentario", "Error en el campo Comentario, debe ingresar un texto.");
            else if (messageHijo.Comentario.Length > 1000)
                ModelState.AddModelError("Comentario", "Error en el campo Comentario, el mismo no puede superar los 1000 caracteres.");

            if (!ModelState.IsValid)
            {
                var m = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { message = m, status = "Error" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {
                    string userId = User.Identity.GetUserId();
                    MessagesHijos mess = new MessagesHijos();
                    mess.UsersId = userId;
                    mess.Message = messageHijo.Comentario;
                    mess.IdMessagePadre = messageHijo.MessagePadreId;
                    mess.DateAdd = DateTime.Now;

                    db.MessagesHijos.Add(mess);
                    db.SaveChanges();
                    return Json(new { result = "OK" });
                }
                catch
                {
                    return Json(new { result = "ERROR" });
                }
            }
        }
    }
}