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
                    }

                    List<MessagePadreViewModel> messages = (from x in db.Messages
                                                            where x.Category == (model.ExperienceId.HasValue ? 2 : 1)
                                                            && x.CategoryId == (model.ExperienceId.HasValue ? model.ExperienceId : model.ProductId)
                                                            && x.IsBlocked == false
                                                            && x.DateNull == null && x.IdComentarioPadre == null
                                                            join u in db.AspNetUsers on x.UsersId equals u.Id
                                                            orderby x.DateAdd ascending
                                                            select new MessagePadreViewModel
                                                            {
                                                                IdComentario = x.IdComentario,
                                                                Message = x.Message,
                                                                UsersId = x.UsersId,
                                                                UsersName = u.UserName,
                                                                DateAdd = x.DateAdd,
                                                                DateNull = x.DateNull,
                                                                IsBlocked = x.IsBlocked.HasValue ? x.IsBlocked.Value : false,
                                                                ComentarioDenuncio = db.Denuncias.Where(d => d.UsersId == userId && d.ComentarioId == x.IdComentario).Any(),
                                                                MessagesHijos = (from y in db.Messages
                                                                                 where y.IdComentarioPadre == x.IdComentario && y.DateNull == null && y.IsBlocked == false
                                                                                 join us in db.AspNetUsers on y.UsersId equals us.Id
                                                                                 orderby y.DateAdd ascending
                                                                                 select new MessageHijoViewModel
                                                                                 {
                                                                                     IdComentario = y.IdComentario,
                                                                                     IdComentarioPadre = y.IdComentarioPadre,
                                                                                     Message = y.Message,
                                                                                     UsersId = y.UsersId,
                                                                                     UsersName = us.UserName,
                                                                                     DateNull = y.DateNull,
                                                                                     IsBlocked = y.IsBlocked.HasValue ? y.IsBlocked.Value : false,
                                                                                     ComentarioDenuncio = db.Denuncias.Where(de => de.UsersId == userId && de.ComentarioId == y.IdComentario).Any(),
                                                                                     DateAdd = y.DateAdd
                                                                                 }).ToList()
                                                            }).ToList();

                    return PartialView("_Messages", messages);
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