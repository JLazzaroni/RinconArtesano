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
    public class ProductsController : Controller
    {
        private RinconArtesanoEntities db = new RinconArtesanoEntities();

        public ActionResult Home()
        {
            List<ProductsCategories> prodCat = db.ProductsCategories.Where(x => x.DateNull == null).OrderBy(x => x.ProductCategoryName).ToList();
            ViewBag.Categorias = prodCat;
            return View();
        }

        public ActionResult ProductsByCategory(int? id)
        {
            List<Products> prod = db.Products.Where(x => x.IdCategory == id & x.DateNull == null & x.IsBlocked == false).OrderBy(x => x.DateAdd).ToList();
            ViewBag.Productss = prod;
            ViewBag.CategoryName = db.ProductsCategories.Find(id).ProductCategoryName;
            ViewBag.NoHayProductos = (prod.Count() == 0) ? "No hay productos cargados para esta categoría" : "";
            return View();
        }

        [Authorize]
        public ActionResult ProductsDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string userId = User.Identity.GetUserId();

            var datos = db.Ratings.Where(x => x.ProductId == id).Select(x => x.Rating);
            Decimal puntos = datos.Any() ? datos.Sum() : 0;
            int cantidad = db.Ratings.Where(x => x.ProductId == id).Select(x => x.Rating).Count();
            Decimal rating = cantidad > 0 ? (puntos / cantidad) : 0;

            RatingViewModel _rating = new RatingViewModel()
            {
                RatingPromedio = rating,
                RatingSelect = db.Ratings.Where(x => x.ProductId == id && x.UsersId == userId).Select(x => x.Rating).SingleOrDefault()
            };

            Products products = db.Products.SingleOrDefault(s => s.ProductId == id);
            var artesano = db.UsersInfo.Where(x => x.UsersId.Equals(products.UsersId)).ToList();

            ProductDetailsViewModel pd = new ProductDetailsViewModel
            {
                //ArtesanoApellido = artesano[0].Apellido,
                //ArtesanoNombre = artesano[0].Nombre,
                //ArtesanoFoto = artesano[0].FilePath,
                Artesano = artesano[0],
                ProductId = products.ProductId,
                UsersId = products.UsersId,
                ProductTitle = products.ProductTitle,
                ProductDescription = products.ProductDescription,
                IdCategory = products.IdCategory,
                IsBlocked = products.IsBlocked,
                DateNull = products.DateNull,
                DateAdd = products.DateAdd,
                DateModification = products.DateModification,
                Files = products.Files,
                ProductsCategories = products.ProductsCategories,
                Rating = _rating,
                UsuarioDenuncio = db.Denuncias.Where(x => x.UsersId == userId && x.ProductId == id).Any()
            };

            List<MessagePadreViewModel> _messages = (from x in db.Messages
                                                     where x.Category == 1 && x.CategoryId == id && x.IsBlocked == false
                                                      && x.DateNull == null && x.IdComentarioPadre == null
                                                     join u in db.AspNetUsers on x.UsersId equals u.Id
                                                     orderby x.DateAdd descending
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
                                                                          orderby y.DateAdd descending
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

            ViewBag.Messages = _messages;

            if (products == null)
            {
                return HttpNotFound();
            }

            return View(pd);
        }

        // GET: Products
        [Authorize]
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            return View(db.Products.Where(x => x.UsersId == userId && x.DateNull == null).ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.SingleOrDefault(s => s.ProductId == id);
            if (products == null)
            {
                return HttpNotFound();
            }
            else if (products.UsersId != User.Identity.GetUserId())
            {
                return View("PermissionsError");
            }
            else
            {
                return View(products);
            }
        }

        [Authorize]
        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.ProductCategory = new SelectList(db.ProductsCategories.Where(x => x.DateNull == null), "ProductCategoryId", "ProductCategoryName");
            ViewBag.CategoriesDescription = db.ProductsCategories.Where(x => x.DateNull == null).ToList();
            return View();
        }

        [Authorize]
        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Products products, IEnumerable<HttpPostedFileBase> uploadedFiles)
        {
            //Validaciones
            if (String.IsNullOrWhiteSpace(products.ProductTitle))
                ModelState.AddModelError("ProductTitle", "Error en el campo Titulo.");
            if (String.IsNullOrWhiteSpace(products.ProductDescription))
                ModelState.AddModelError("ProductDescription", "Error en el campo Descripción.");
            if (checkNullCollection(uploadedFiles))
                ModelState.AddModelError("UploadedFiles", "Error debe cargarse al menos una Imagen o Video.");

            if (!ModelState.IsValid)
            {
                //ViewBag.ProductCategory = new SelectList(db.ProductsCategories.Where(x => x.DateNull == null), "ProductCategoryId", "ProductCategoryName");

                ViewBag.ProductCategory = new SelectList(db.ProductsCategories.Where(x => x.DateNull == null), "ProductCategoryId", "ProductCategoryName");
                ViewBag.CategoriesDescription = db.ProductsCategories.Where(x => x.DateNull == null).ToList();
            
                return View(products);
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
                        products.Files.Add(arc);
                    }
                }


                products.UsersId = userId;
                products.DateAdd = DateTime.Now;
                products.IsBlocked = false;

                db.Products.Add(products);
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
        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.SingleOrDefault(s => s.ProductId == id);
            if (products == null)
            {
                return HttpNotFound();
            }
            else if (products.UsersId != User.Identity.GetUserId())
            {
                return View("PermissionsError");
            }
            else
            {
                ViewBag.ProductCategory = new SelectList(db.ProductsCategories.Where(x => x.DateNull == null), "ProductCategoryId", "ProductCategoryName");
                ViewBag.CategoriesDescription = db.ProductsCategories.Where(x => x.DateNull == null).ToList();
                return View(products);
            }
        }

        [Authorize]
        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Products products, IEnumerable<HttpPostedFileBase> uploadedFiles)
        {
            //Validaciones
            if (String.IsNullOrWhiteSpace(products.ProductTitle))
                ModelState.AddModelError("ProductTitle", "Error en el campo Titulo.");
            if (String.IsNullOrWhiteSpace(products.ProductDescription))
                ModelState.AddModelError("ProductDescription", "Error en el campo Descripción.");

            if (!ModelState.IsValid)
            {
                ViewBag.ProductCategory = new SelectList(db.ProductsCategories.Where(x => x.DateNull == null), "ProductCategoryId", "ProductCategoryName");
                ViewBag.CategoriesDescription = db.ProductsCategories.Where(x => x.DateNull == null).ToList();
                return View(products);
            }
            else
            {
                string userId = User.Identity.GetUserId();
                var prod = db.Products.Find(products.ProductId);
                prod.ProductTitle = products.ProductTitle;
                prod.ProductDescription = products.ProductDescription;
                prod.IdCategory = products.IdCategory;
                prod.DateModification = DateTime.Now;

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


                            List<Files> bdFile = prod.Files.OrderBy(f => f.FileId).ToList();

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
                                prod.Files.Add(arc);
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
        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            else if (products.UsersId != User.Identity.GetUserId())
            {
                return View("PermissionsError");
            }
            return View(products);
        }

        [Authorize]
        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = db.Products.Find(id);
            products.DateNull = DateTime.Now;

            List<Files> listaFiles = products.Files.ToList();
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
            Products products = db.Products.Find(id);
            products.DateNull = DateTime.Now;
            List<Files> listaFiles = products.Files.ToList();
            foreach (var file in listaFiles)
            {
                file.DateNull = DateTime.Now;
            }
            db.SaveChanges();
            return Json(new { message = "Producto eliminado exitosamente.", status = "OK" });
        }
        [Authorize(Roles = "Administrador, Moderador")]
        [HttpPost]
        public JsonResult manualActivate(int id)
        {
            Products products = db.Products.Find(id);
            products.DateNull = null;
            products.IsBlocked = false;

            db.SaveChanges();
            return Json(new { message = "Producto activado exitosamente.", status = "OK" });
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
