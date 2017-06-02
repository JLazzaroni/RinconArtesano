using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Datos;
using System.IO;
using RinconArtesano.Models;
using Microsoft.AspNet.Identity;

namespace RinconArtesano.Controllers
{
    public class UsersInfoController : Controller
    {
        private RinconArtesanoEntities db = new RinconArtesanoEntities();

        // GET: UsersInfo
        public ActionResult Index()
        {
            var usersInfo = db.UsersInfo.Include(u => u.AspNetUsers);
            return View(usersInfo.ToList());
        }

        // GET: UsersInfo/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsersInfo usersInfo = db.UsersInfo.Find(id);
            
            ViewBag.InfoUsuario = db.UsersInfo.Find(id);
            if (usersInfo == null)
            {
                return HttpNotFound();
            }
            return View(usersInfo);
        }

        public ActionResult Denunciar(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsersInfo usersInfo = db.UsersInfo.Find(id);

            ViewBag.InfoUsuario = db.UsersInfo.Find(id);
            DenunciasViewModel d = new DenunciasViewModel();
            if (usersInfo == null)
            {
                return HttpNotFound();
            }
            string userId = User.Identity.GetUserId();
            if (db.Denuncias.Where(x => x.DateNull == null && x.UsersId.Equals(userId) && x.UsersIdDenunciado.Equals(id)).Any())
                ViewBag.Mensaje = "Su denuncia fue enviada exitosamente, nuestro equipo la analizara y tomara las medidas necesarias.";

            return View(d);
        }

        [Authorize]
        // POST: Experiences/Create
        [HttpPost]
        public ActionResult Denunciar(DenunciasViewModel denuncias, string userIdPerfil)
        {
            //Validaciones
            if (String.IsNullOrWhiteSpace(denuncias.Comentario))
                ModelState.AddModelError("Comentario", "Error en el campo Comentario, debe ingresar un texto.");
            else if (denuncias.Comentario.Length > 1000)
                ModelState.AddModelError("Comentario", "Error en el campo Comentario, el mismo no puede superar los 1000 caracteres.");
            
            ViewBag.InfoUsuario = db.UsersInfo.Find(userIdPerfil);
            
            if (!ModelState.IsValid)
            {
                return View(denuncias);
            }
            else
            {
                denuncias.UserIdDenunciado = userIdPerfil;
                string userId = User.Identity.GetUserId();
                Denuncias _denuncia = new Denuncias();
                _denuncia.UsersId = userId;
                _denuncia.Comentario = denuncias.Comentario;
                _denuncia.UsersIdDenunciado = denuncias.UserIdDenunciado;
                _denuncia.DateAdd = DateTime.Now;

                db.Denuncias.Add(_denuncia);

                if (checkIfItsNeedToBeBlocked(denuncias))
                    automaticBlock(denuncias);

                db.SaveChanges();

                return RedirectToAction("Denunciar");
            }
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
                cant = db.Denuncias.Where(d => d.UsersIdDenunciado == denuncia.UserIdDenunciado && !d.DateNull.HasValue).Count();

            cant++;
            if (cant >= cantBloqueo)
                return true;
            else
                return false;
        }

        private string contentType(DenunciasViewModel denuncia)
        {
            string _tipoContenido = "";
            if (denuncia.ExperienceId != 0)
                _tipoContenido = "experiencia";
            else if (denuncia.ProductId != 0)
                _tipoContenido = "producto";
            else if (!String.IsNullOrWhiteSpace(denuncia.UserIdDenunciado))
                _tipoContenido = "usuario";
            return (_tipoContenido);
        }

        private void automaticBlock(DenunciasViewModel denuncia)
        {
            Experiences e = new Experiences();
            Products p = new Products();
            UsersInfo u = new UsersInfo();
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
        }

        // GET: UsersInfo/Create
        public ActionResult Create()
        {
            ViewBag.UsersId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: UsersInfo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UsersId,Nombre,Apellido,FechaNacimiento,Edad,Sexo,Pais,Provincia,Localidad,Direccion,Telefono,Intereses,Rubro,FilePath,DateNull,DateAdd,DateModification,IsBlocked")] UsersInfo usersInfo)
        {
            if (ModelState.IsValid)
            {
                db.UsersInfo.Add(usersInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UsersId = new SelectList(db.AspNetUsers, "Id", "Email", usersInfo.UsersId);
            return View(usersInfo);
        }


        public static void CreateEmptyUsersInfo(string id)
        {
            RinconArtesanoEntities db = new RinconArtesanoEntities();
            UsersInfo u = new UsersInfo() { 
                UsersId = id,
                FechaNacimiento = DateTime.Now,
                FilePath = "~/Content/Images/defaultUser.gif",
                DateAdd = DateTime.Now
            };
            
            db.UsersInfo.Add(u);
            db.SaveChanges();
        }

        // GET: UsersInfo/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsersInfo usersInfo = db.UsersInfo.Find(id);
            if (usersInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsersId = new SelectList(db.AspNetUsers, "Id", "Email", usersInfo.UsersId);
            return View(usersInfo);
        }

        // POST: UsersInfo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UsersInfo usersInfo, HttpPostedFileBase uploadedFile)
        {
            if (ModelState.IsValid)
            {
                var u = db.UsersInfo.Where(x => x.UsersId.Equals(usersInfo.UsersId)).FirstOrDefault();
                if (uploadedFile != null && uploadedFile.ContentLength > 0)
                {
                    string rutaServer = @"~\Content\Images";
                    string rutaSource = @"~/Content/Images/";
                    var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(uploadedFile.FileName);
                    var pathServer = Path.Combine(Server.MapPath(rutaServer), nombreArchivo);
                    var pathSource = rutaSource + nombreArchivo;
                    uploadedFile.SaveAs(pathServer);

                    u.FilePath = pathSource;
                }

                u.Nombre = usersInfo.Nombre;
                u.Apellido = usersInfo.Apellido;
                u.FechaNacimiento = usersInfo.FechaNacimiento;
                u.Edad = usersInfo.Edad;
                u.Sexo = usersInfo.Sexo;
                u.Pais = usersInfo.Pais;
                u.Provincia = usersInfo.Provincia;
                u.Localidad = usersInfo.Localidad;
                u.Direccion = usersInfo.Direccion;
                u.Telefono = usersInfo.Telefono;
                u.Intereses = usersInfo.Intereses;
                u.Rubro = usersInfo.Rubro;
                u.DateModification = DateTime.Now;

                db.SaveChanges();
                return RedirectToAction("Index","Manage");
            }
            ViewBag.UsersId = new SelectList(db.AspNetUsers, "Id", "Email", usersInfo.UsersId);
            return View(usersInfo);
        }

        // GET: UsersInfo/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsersInfo usersInfo = db.UsersInfo.Find(id);
            if (usersInfo == null)
            {
                return HttpNotFound();
            }
            return View(usersInfo);
        }

        // POST: UsersInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UsersInfo usersInfo = db.UsersInfo.Find(id);
            db.UsersInfo.Remove(usersInfo);
            db.SaveChanges();
            return RedirectToAction("Index");
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
