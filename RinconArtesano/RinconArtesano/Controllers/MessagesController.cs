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
            if (String.IsNullOrWhiteSpace(messagePadre.Message))
                ModelState.AddModelError("Comentario", "Error en el campo Comentario, debe ingresar un texto.");
            else if (messagePadre.Message.Length > 1000)
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
                    mess.Message = messagePadre.Message;

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
            if (String.IsNullOrWhiteSpace(messageHijo.Message))
                ModelState.AddModelError("Comentario", "Error en el campo Comentario, debe ingresar un texto.");
            else if (messageHijo.Message.Length > 1000)
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
                    mess.Message = messageHijo.Message;
                    mess.IdMessagePadre = messageHijo.IdComentarioPadre.Value;
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


        [Authorize]
        [HttpPost]
        public ActionResult CreateMessage(MessageViewModels model)
        {
            //Validaciones
            if (String.IsNullOrWhiteSpace(model.Message))
                ModelState.AddModelError("Comentario", "Error en el campo Comentario, debe ingresar un texto.");
            else if (model.Message.Length > 1000)
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

                    if (!model.IdComentarioPadre.HasValue)
                    {
                        Messages _messages = new Messages();
                        _messages.UsersId = userId;
                        _messages.Message = model.Message;
                        if (model.ProductId.HasValue)
                        {
                            _messages.Category = 1;
                            _messages.CategoryId = model.ProductId.Value;
                        }
                        else
                        {
                            _messages.Category = 2;
                            _messages.CategoryId = model.ExperienceId.Value;
                        }

                        _messages.IsBlocked = false;
                        _messages.DateAdd = DateTime.Now;

                        db.Messages.Add(_messages);
                        db.SaveChanges();
                        return Json(new { result = "OK" });
                    }
                    else
                    {
                        Messages _messages = new Messages();
                        _messages.IdComentarioPadre = model.IdComentarioPadre;
                        _messages.UsersId = userId;
                        _messages.Message = model.Message;
                        if (model.ProductId.HasValue)
                        {
                            _messages.Category = 1;
                            _messages.CategoryId = model.ProductId.Value;
                        }
                        else
                        {
                            _messages.Category = 2;
                            _messages.CategoryId = model.ExperienceId.Value;
                        }

                        _messages.IsBlocked = false;
                        _messages.DateAdd = DateTime.Now;

                        db.Messages.Add(_messages);
                        db.SaveChanges();
                        return Json(new { result = "OK" });
                    }
                }
                catch
                {
                    return Json(new { result = "ERROR" });
                }
            }
        }

        [Authorize]
        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Messages messages = db.Messages.Find(id);
            if (messages == null)
            {
                return HttpNotFound();
            }
            else if (messages.UsersId != User.Identity.GetUserId())
            {
                return View("PermissionsError");
            }
            return View(messages);
        }

        [Authorize]
        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Messages messages = db.Messages.Find(id);
            messages.DateNull = DateTime.Now;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrador, Moderador")]
        [HttpPost]
        public JsonResult manualDelete(int id)
        {
            Messages messages = db.Messages.Find(id);
            messages.DateNull = DateTime.Now;
            db.SaveChanges();
            return Json(new { message = "Comentario eliminado exitosamente.", status = "OK" });
        }
        [Authorize(Roles = "Administrador, Moderador")]
        [HttpPost]
        public JsonResult manualActivate(int id)
        {
            Messages messages = db.Messages.Find(id);
            messages.DateNull = null;
            messages.IsBlocked = false;

            db.SaveChanges();
            return Json(new { message = "Comentario activado exitosamente.", status = "OK" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}