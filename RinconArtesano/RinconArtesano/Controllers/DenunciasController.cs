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
                _denuncia.DateAdd = DateTime.Now;

                db.Denuncias.Add(_denuncia);

                if (checkIfItsNeedToBeBlocked(denuncias))
                    automaticBlock(denuncias);

                db.SaveChanges();
                return Json(new { message = "Su denuncia fue enviada exitosamente.", status = "OK" });
            }
        }

        private string contentType(DenunciasViewModel denuncia)
        {
            string _tipoContenido = "";
            if (denuncia.ExperienceId != 0)
                _tipoContenido = "experiencia";
            else if (denuncia.ProductId != 0)
                _tipoContenido = "producto";
            return (_tipoContenido);
        }

        private void automaticBlock(DenunciasViewModel denuncia)
        {
            Experiences e = new Experiences();
            Products p = new Products();
            string _tipoContenido = contentType(denuncia);
            if (_tipoContenido.Equals("experiencia"))
            {
                e = db.Experiences.Find(denuncia.ExperienceId);
                e.Bloqueado = true;
            }
            else if (_tipoContenido.Equals("producto"))
            {
                p = db.Products.Find(denuncia.ProductId);
                p.Bloqueado = true;
            }
        }

        [Authorize(Roles = "Administrador, Moderador")]
        public JsonResult manualBlock(string tipoContenido, int id)
        {
            Experiences e = new Experiences();
            Products p = new Products();
            string _tipoContenido = tipoContenido;
            if (_tipoContenido.Equals("experiencia"))
            {
                e = db.Experiences.Find(id);
                e.Bloqueado = true;
            }
            else if (_tipoContenido.Equals("producto"))
            {
                p = db.Products.Find(id);
                p.Bloqueado = true;
            }
            db.SaveChanges();
            return Json(new { message = "Bloqueo exitoso.", status = "OK" });
        }
        [Authorize(Roles = "Administrador, Moderador")]
        public JsonResult manualUnblock(string tipoContenido, int id)
        {
            Experiences e = new Experiences();
            Products p = new Products();
            string _tipoContenido = tipoContenido;
            if (_tipoContenido.Equals("experiencia"))
            {
                e = db.Experiences.Find(id);
                e.Bloqueado = false;
            }
            else if (_tipoContenido.Equals("producto"))
            {
                p = db.Products.Find(id);
                p.Bloqueado = false;
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

            cant++;
            if (cant >= cantBloqueo)
                return true;
            else
                return false;
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
