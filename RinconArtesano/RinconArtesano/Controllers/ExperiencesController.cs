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
    public class ExperiencesController : Controller
    {
        private RinconArtesanoEntities db = new RinconArtesanoEntities();

        public ActionResult Home()
        {
            List<Experiences> exper = db.Experiences.Where(x => x.DateNull == null & x.IsBlocked == false).OrderBy(x => x.DateAdd).ToList();
            ViewBag.Experiences = exper;
            return View();
        }

        [Authorize]
        public ActionResult ExperiencesDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Experiences experiences = db.Experiences.SingleOrDefault(s => s.ExperienceId == id);
            ViewBag.Messages = db.MessagesPadres.Where(x => x.Category == 2 && x.CategoryId == id && x.DateNull == null).ToList();
            string userId = User.Identity.GetUserId();
            ViewBag.UsuarioDenuncio = db.Denuncias.Where(x => x.UsersId == userId && x.ExperienceId == id).Any();
            if (experiences == null)
            {
                return HttpNotFound();
            }
            return View(experiences);
        }

        // GET: Experiences
        [Authorize]
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            return View(db.Experiences.Where(x => x.UsersId == userId).ToList());
        }

        // GET: Experiences/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Experiences Experiences = db.Experiences.SingleOrDefault(s => s.ExperienceId == id);
            if (Experiences == null)
            {
                return HttpNotFound();
            }
            else if (Experiences.UsersId != User.Identity.GetUserId())
            {
                return View("PermissionsError");
            }
            else
            {
                return View(Experiences);
            }
        }

        [Authorize]
        // GET: Experiences/Create
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        // POST: Experiences/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Experiences experiences, IEnumerable<HttpPostedFileBase> uploadedFiles)
        {
            //Validaciones
            if (String.IsNullOrWhiteSpace(experiences.ExperienceTitle))
                ModelState.AddModelError("ExperienceTitle", "Error en el campo Titulo.");
            if (String.IsNullOrWhiteSpace(experiences.ExperienceDescription))
                ModelState.AddModelError("ExperienceDescription", "Error en el campo Descripción.");
            if (checkNullCollection(uploadedFiles))
                ModelState.AddModelError("UploadedFiles", "Error debe cargarse al menos una Imagen o Video.");

            if (!ModelState.IsValid)
            {
                return View(experiences);
            }
            else
            {
                string userId = User.Identity.GetUserId();
                foreach (var file in uploadedFiles)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        string rutaServer = @"~\Content\Images";
                        string rutaSource = @"~/Content/Images/";
                        var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var pathServer = Path.Combine(Server.MapPath(rutaServer), nombreArchivo);
                        var pathSource = rutaSource + nombreArchivo;
                        file.SaveAs(pathServer);

                        var arc = new Files
                        {
                            FileName = nombreArchivo,
                            FileType = Convert.ToInt16(FileTypes.Image),
                            ContentType = file.ContentType,
                            FilePath = pathSource,
                            UsersId = userId,
                            DateAdd = DateTime.Now
                        };
                        experiences.Files.Add(arc);
                    }
                }


                experiences.UsersId = userId;
                experiences.DateAdd = DateTime.Now;
                experiences.IsBlocked = false;

                db.Experiences.Add(experiences);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        private bool checkNullCollection(IEnumerable<HttpPostedFileBase> files)
        {
            //True si todos son null.
            int total = files.Count();
            int cantNull = 0;
            foreach (var file in files)
            {
                if (file == null || file.ContentLength == 0)
                {
                    cantNull++;
                }
            }
            if (cantNull == total)
                return true;
            else
                return false;
        }

        [Authorize]
        // GET: Experiences/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Experiences experiences = db.Experiences.SingleOrDefault(s => s.ExperienceId == id);
            if (experiences == null)
            {
                return HttpNotFound();
            }
            else if (experiences.UsersId != User.Identity.GetUserId())
            {
                return View("PermissionsError");
            }
            else
            {
                return View(experiences);
            }
        }

        [Authorize]
        // POST: Experiences/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Experiences experiences, IEnumerable<HttpPostedFileBase> uploadedFiles)
        {
            //Validaciones
            if (String.IsNullOrWhiteSpace(experiences.ExperienceTitle))
                ModelState.AddModelError("experienceTitle", "Error en el campo Titulo.");
            if (String.IsNullOrWhiteSpace(experiences.ExperienceDescription))
                ModelState.AddModelError("experienceDescription", "Error en el campo Descripción.");

            if (!ModelState.IsValid)
            {
                return View(experiences);
            }
            else
            {
                string userId = User.Identity.GetUserId();
                var exper = db.Experiences.Find(experiences.ExperienceId);
                exper.ExperienceTitle = experiences.ExperienceTitle;
                exper.ExperienceDescription = experiences.ExperienceDescription;
                exper.DateModification = DateTime.Now;

                //Si se subio algun archivo, que haga todo el mambo para Files
                if (!checkNullCollection(uploadedFiles))
                {
                    int cont = 0;
                    foreach (var file in uploadedFiles)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            string rutaServer = @"~\Content\Images";
                            string rutaSource = @"~/Content/Images/";
                            var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            var pathServer = Path.Combine(Server.MapPath(rutaServer), nombreArchivo);
                            var pathSource = rutaSource + nombreArchivo;
                            file.SaveAs(pathServer);


                            List<Files> bdFile = exper.Files.OrderBy(f => f.FileId).ToList();

                            if (cont + 1 <= bdFile.Count())  //Si existe
                            {
                                //Modificar imagen
                                var arc = new
                                {
                                    FileName = nombreArchivo,
                                    FileType = Convert.ToInt16(FileTypes.Image),
                                    ContentType = file.ContentType,
                                    FilePath = pathSource,
                                    UsersId = userId,
                                    DateAdd = DateTime.Now
                                };
                                db.Entry(bdFile[cont]).CurrentValues.SetValues(arc);
                            }

                            else
                            {
                                //Nueva imagen
                                var arc = new Files
                                {
                                    FileName = nombreArchivo,
                                    FileType = Convert.ToInt16(FileTypes.Image),
                                    ContentType = file.ContentType,
                                    FilePath = pathSource,
                                    UsersId = userId,
                                    DateAdd = DateTime.Now
                                };
                                exper.Files.Add(arc);
                            }
                        }
                        cont++;
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        [Authorize]
        // GET: Experiences/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Experiences Experiences = db.Experiences.Find(id);
            if (Experiences == null)
            {
                return HttpNotFound();
            }
            return View(Experiences);
        }

        [Authorize]
        // POST: Experiences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Experiences experiences = db.Experiences.Find(id);
            experiences.DateNull = DateTime.Now;

            List<Files> listaFiles = experiences.Files.ToList();
            foreach (var file in listaFiles)
            {
                file.DateNull = DateTime.Now;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrador, Moderador")]
        [HttpPost]
        public JsonResult manualDelete(int id)
        {
            Experiences experiences = db.Experiences.Find(id);
            experiences.DateNull = DateTime.Now;
            List<Files> listaFiles = experiences.Files.ToList();
            foreach (var file in listaFiles)
            {
                file.DateNull = DateTime.Now;
            }
            db.SaveChanges();
            return Json(new { message = "Experiencia eliminada exitosamente.", status = "OK" });
        }
        [Authorize(Roles = "Administrador, Moderador")]
        [HttpPost]
        public JsonResult manualActivate(int id)
        {
            Experiences experiences = db.Experiences.Find(id);
            experiences.DateNull = null;
            experiences.IsBlocked = false;

            db.SaveChanges();
            return Json(new { message = "Experiencia activada exitosamente.", status = "OK" });
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
