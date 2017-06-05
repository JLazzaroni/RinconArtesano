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
    public class DenunciasController : Controller
    {
        private RinconArtesanoEntities db = new RinconArtesanoEntities();

        [Authorize]
        // POST: Experiences/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(DenunciasViewModel denuncias)
        {
            //Validaciones
            if (String.IsNullOrWhiteSpace(denuncias.Comentario))
                ModelState.AddModelError("Comentario", "Error en el campo Comentario, debe ingresar un texto.");
            else if (denuncias.Comentario.Length > 1000)
                ModelState.AddModelError("Comentario", "Error en el campo Comentario, el mismo no puede superar los 1000 caracteres.");

            if (!ModelState.IsValid)
            {
                var m = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { message = m, status = "Error" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string userId = User.Identity.GetUserId();
                Denuncias _denuncia = new Denuncias();
                _denuncia.UsersId = userId;
                _denuncia.Comentario = denuncias.Comentario;
                if (denuncias.ProductId != 0)
                    _denuncia.ProductId = denuncias.ProductId;
                if (denuncias.ExperienceId != 0)
                    _denuncia.ExperienceId = denuncias.ExperienceId;
                if (denuncias.ComentarioId != 0)
                    _denuncia.ComentarioId = denuncias.ComentarioId;

                _denuncia.DateAdd = DateTime.Now;

                db.Denuncias.Add(_denuncia);

                if (checkIfItsNeedToBeBlocked(denuncias))
                    automaticBlock(denuncias);

                db.SaveChanges();
                return Json(new { message = "Su denuncia fue enviada exitosamente, nuestro equipo la analizara y tomara las medidas necesarias.", status = "OK" });
            }
        }

        private string contentType(DenunciasViewModel denuncia)
        {
            string _tipoContenido = "";
            if (denuncia.ExperienceId != 0)
                _tipoContenido = "experiencia";
            else if (denuncia.ProductId != 0)
                _tipoContenido = "producto";
            else if (denuncia.ComentarioId != 0)
                _tipoContenido = "comentario";
            else if (String.IsNullOrWhiteSpace(denuncia.UserIdDenunciado))
                _tipoContenido = "usuario";
            return (_tipoContenido);
        }

        private void automaticBlock(DenunciasViewModel denuncia)
        {
            Experiences e = new Experiences();
            Products p = new Products();
            UsersInfo u = new UsersInfo();
            Messages m = new Messages();
            string _tipoContenido = contentType(denuncia);
            if (_tipoContenido.Equals("experiencia"))
            {
                e = db.Experiences.Find(denuncia.ExperienceId);
                e.IsBlocked = true;
            }
            else if (_tipoContenido.Equals("producto"))
            {
                p = db.Products.Find(denuncia.ProductId);
                p.IsBlocked = true;
            }
            else if (_tipoContenido.Equals("usuario"))
            {
                u = db.UsersInfo.Find(denuncia.UserIdDenunciado);
                u.IsBlocked = true;
            }
            else if (_tipoContenido.Equals("comentario"))
            {
                m = db.Messages.Find(denuncia.ComentarioId);
                m.IsBlocked = true;
            }
        }

        [Authorize(Roles = "Administrador, Moderador")]
        public JsonResult manualBlock(string tipoContenido, int id)
        {
            Experiences e = new Experiences();
            Products p = new Products();
            Messages m = new Messages();
            string _tipoContenido = tipoContenido;
            if (_tipoContenido.Equals("experiencia"))
            {
                e = db.Experiences.Find(id);
                e.IsBlocked = true;
            }
            else if (_tipoContenido.Equals("producto"))
            {
                p = db.Products.Find(id);
                p.IsBlocked = true;
            }
            else if (_tipoContenido.Equals("comentario"))
            {
                m = db.Messages.Find(id);
                m.IsBlocked = true;
            }
            db.SaveChanges();
            return Json(new { message = "Bloqueo exitoso.", status = "OK" });
        }
        [Authorize(Roles = "Administrador, Moderador")]
        public JsonResult manualUnblock(string tipoContenido, int id)
        {
            Experiences e = new Experiences();
            Products p = new Products();
            Messages m = new Messages();
            string _tipoContenido = tipoContenido;
            if (_tipoContenido.Equals("experiencia"))
            {
                e = db.Experiences.Find(id);
                e.IsBlocked = false;
            }
            else if (_tipoContenido.Equals("producto"))
            {
                p = db.Products.Find(id);
                p.IsBlocked = false;
            }
            else if (_tipoContenido.Equals("comentario"))
            {
                m = db.Messages.Find(id);
                m.IsBlocked = false;
            }
            db.SaveChanges();
            return Json(new { message = "Desbloqueo exitoso.", status = "OK" });
        }

        private bool checkIfItsNeedToBeBlocked(DenunciasViewModel denuncia)
        {
            string _tipoContenido = contentType(denuncia);
            int cantBloqueo = 5;
            int cant = 0;
            if (_tipoContenido.Equals("experiencia"))
                cant = db.Denuncias.Where(d => d.ExperienceId == denuncia.ExperienceId && !d.DateNull.HasValue).Count();
            else if (_tipoContenido.Equals("producto"))
                cant = db.Denuncias.Where(d => d.ProductId == denuncia.ProductId && !d.DateNull.HasValue).Count();
            else if (_tipoContenido.Equals("usuario"))
                cant = db.Denuncias.Where(d => d.ProductId == denuncia.ProductId && !d.DateNull.HasValue).Count();
            else if (_tipoContenido.Equals("comentario"))
                cant = db.Denuncias.Where(d => d.ComentarioId == denuncia.ComentarioId && !d.DateNull.HasValue).Count();

            cant++;
            if (cant >= cantBloqueo)
                return true;
            else
                return false;
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDenuncia(int id)
        {
            Denuncias _denuncias = db.Denuncias.Find(id);
            _denuncias.DateNull = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("GestionarDenunciasProductos", "GestionarDenunciasProductos");
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
