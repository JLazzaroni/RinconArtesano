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
