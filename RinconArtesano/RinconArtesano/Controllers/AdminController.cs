﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using RinconArtesano.Models;
using Datos;
using System.IO;

namespace RinconArtesano.Controllers
{
    public class AdminController : Controller
    {
        private RinconArtesanoEntities db = new RinconArtesanoEntities();
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        // GET: /Admin/
        [Authorize(Roles = "Administrador")]
        public ActionResult Index(string searchStringUserNameOrEmail, string currentFilter, int? page)
        {
            try
            {
                int intPage = 1;
                int intPageSize = 5;
                int intTotalPageCount = 0;

                if (searchStringUserNameOrEmail != null)
                {
                    intPage = 1;
                }
                else
                {
                    if (currentFilter != null)
                    {
                        searchStringUserNameOrEmail = currentFilter;
                        intPage = page ?? 1;
                    }
                    else
                    {
                        searchStringUserNameOrEmail = "";
                        intPage = page ?? 1;
                    }
                }

                ViewBag.CurrentFilter = searchStringUserNameOrEmail;

                List<ExpandedUserViewModel> listaUserViewModel = new List<ExpandedUserViewModel>();
                int intSkip = (intPage - 1) * intPageSize;

                intTotalPageCount = UserManager.Users
                    .Where(x => x.UserName.Contains(searchStringUserNameOrEmail))
                    .Count();

                var result = UserManager.Users
                    .Where(x => x.UserName.Contains(searchStringUserNameOrEmail))
                    .OrderBy(x => x.UserName)
                    .Skip(intSkip)
                    .Take(intPageSize)
                    .ToList();

                foreach (var item in result)
                {
                    ExpandedUserViewModel nUserViewModel = new ExpandedUserViewModel();

                    nUserViewModel.UserName = item.UserName;
                    nUserViewModel.Email = item.Email;
                    nUserViewModel.LockoutEndDateUtc = item.LockoutEndDateUtc;

                    listaUserViewModel.Add(nUserViewModel);
                }

                return View(listaUserViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                List<ExpandedUserViewModel> listaUserViewModel = new List<ExpandedUserViewModel>();

                return View(listaUserViewModel);
            }
        }
        [Authorize(Roles = "Administrador")]
        public ActionResult GestionarExperiencias(string searchStringExperience, string currentFilter, int? page)
        {
            try
            {
                int intPage = 1;
                int intPageSize = 5;
                int intTotalPageCount = 0;

                if (searchStringExperience != null)
                {
                    intPage = 1;
                }
                else
                {
                    if (currentFilter != null)
                    {
                        searchStringExperience = currentFilter;
                        intPage = page ?? 1;
                    }
                    else
                    {
                        searchStringExperience = "";
                        intPage = page ?? 1;
                    }
                }

                ViewBag.CurrentFilter = searchStringExperience;

                int intSkip = (intPage - 1) * intPageSize;

                intTotalPageCount = db.Experiences
                    .Where(x => x.ExperienceTitle.Contains(searchStringExperience) & (x.DateNull.HasValue || x.IsBlocked == true))
                    .Count();

                List<Experiences> result = db.Experiences
                    .Where(x => x.ExperienceTitle.Contains(searchStringExperience) & (x.DateNull.HasValue || x.IsBlocked == true))
                    .OrderBy(x => x.ExperienceTitle)
                    .Skip(intSkip)
                    .Take(intPageSize)
                    .ToList();

                ViewBag.Experiences = result;

                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                List<Experiences> result = new List<Experiences>();
                ViewBag.Experiences = result;

                return View();
            }
        }
        [Authorize(Roles = "Administrador")]
        public ActionResult GestionarProductos(string searchStringProduct, string currentFilter, int? page)
        {
            try
            {
                int intPage = 1;
                int intPageSize = 5;
                int intTotalPageCount = 0;

                if (searchStringProduct != null)
                {
                    intPage = 1;
                }
                else
                {
                    if (currentFilter != null)
                    {
                        searchStringProduct = currentFilter;
                        intPage = page ?? 1;
                    }
                    else
                    {
                        searchStringProduct = "";
                        intPage = page ?? 1;
                    }
                }

                ViewBag.CurrentFilter = searchStringProduct;

                int intSkip = (intPage - 1) * intPageSize;

                intTotalPageCount = db.Products
                    .Where(x => x.ProductTitle.Contains(searchStringProduct) & (x.DateNull.HasValue || x.IsBlocked == true))
                    .Count();

                List<Products> result = db.Products
                    .Where(x => x.ProductTitle.Contains(searchStringProduct) & (x.DateNull.HasValue || x.IsBlocked == true))
                    .OrderBy(x => x.ProductTitle)
                    .Skip(intSkip)
                    .Take(intPageSize)
                    .ToList();

                ViewBag.Products = result;

                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                List<Products> result = new List<Products>();
                ViewBag.Products = result;

                return View();
            }
        }

        [Authorize(Roles = "Administrador, Moderador")]
        public ActionResult GestionarComentarios(string searchStringComentario, string currentFilter, int? page)
        {
            try
            {
                int intPage = 1;
                int intPageSize = 5;
                int intTotalPageCount = 0;

                if (searchStringComentario != null)
                {
                    intPage = 1;
                }
                else
                {
                    if (currentFilter != null)
                    {
                        searchStringComentario = currentFilter;
                        intPage = page ?? 1;
                    }
                    else
                    {
                        searchStringComentario = "";
                        intPage = page ?? 1;
                    }
                }

                ViewBag.CurrentFilter = searchStringComentario;

                int intSkip = (intPage - 1) * intPageSize;

                intTotalPageCount = db.Messages
                    .Where(x => x.Message.Contains(searchStringComentario) & (x.DateNull.HasValue || x.IsBlocked == true))
                    .Count();

                List<Messages> result = db.Messages
                    .Where(x => x.Message.Contains(searchStringComentario) & (x.DateNull.HasValue || x.IsBlocked == true))
                    .OrderBy(x => x.Message)
                    .Skip(intSkip)
                    .Take(intPageSize)
                    .ToList();

                ViewBag.Messages = result;

                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                List<Messages> result = new List<Messages>();
                ViewBag.Messages = result;

                return View();
            }
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult GestionarDenunciasProductos(string searchStringDenuncia, string currentFilter, int? page)
        {
            try
            {
                int intPage = 1;
                int intPageSize = 5;
                int intTotalPageCount = 0;

                if (searchStringDenuncia != null)
                {
                    intPage = 1;
                }
                else
                {
                    if (currentFilter != null)
                    {
                        searchStringDenuncia = currentFilter;
                        intPage = page ?? 1;
                    }
                    else
                    {
                        searchStringDenuncia = "";
                        intPage = page ?? 1;
                    }
                }

                ViewBag.CurrentFilter = searchStringDenuncia;

                int intSkip = (intPage - 1) * intPageSize;

                intTotalPageCount = db.Denuncias
                    .Where(x => x.Comentario.Contains(searchStringDenuncia) & x.ProductId != null & x.DateNull == null)
                    .Count();

                List<Denuncias> result = db.Denuncias
                    .Where(x => x.Comentario.Contains(searchStringDenuncia) & x.ProductId != null & x.DateNull == null)
                    .OrderBy(x => x.Comentario)
                    .Skip(intSkip)
                    .Take(intPageSize)
                    .ToList();

                ViewBag.Denuncias = result;
                ViewBag.TipoDenuncias = "Productos";

                return View("GestionarDenuncias");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                List<Denuncias> result = new List<Denuncias>();
                ViewBag.Denuncias = result;
                ViewBag.TipoDenuncias = "Productos";

                return View("GestionarDenuncias");
            }
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult GestionarDenunciasExperiencias(string searchStringDenuncia, string currentFilter, int? page)
        {
            try
            {
                int intPage = 1;
                int intPageSize = 5;
                int intTotalPageCount = 0;

                if (searchStringDenuncia != null)
                {
                    intPage = 1;
                }
                else
                {
                    if (currentFilter != null)
                    {
                        searchStringDenuncia = currentFilter;
                        intPage = page ?? 1;
                    }
                    else
                    {
                        searchStringDenuncia = "";
                        intPage = page ?? 1;
                    }
                }

                ViewBag.CurrentFilter = searchStringDenuncia;
                ViewBag.TipoDenuncias = "Experiencias";

                int intSkip = (intPage - 1) * intPageSize;

                intTotalPageCount = db.Denuncias
                    .Where(x => x.Comentario.Contains(searchStringDenuncia) & x.ExperienceId != null & x.DateNull == null)
                    .Count();

                List<Denuncias> result = db.Denuncias
                    .Where(x => x.Comentario.Contains(searchStringDenuncia) & x.ExperienceId != null & x.DateNull == null)
                    .OrderBy(x => x.Comentario)
                    .Skip(intSkip)
                    .Take(intPageSize)
                    .ToList();

                ViewBag.Denuncias = result;
                ViewBag.TipoDenuncias = "Experiencias";

                return View("GestionarDenuncias");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                List<Denuncias> result = new List<Denuncias>();
                ViewBag.Denuncias = result;

                return View("GestionarDenuncias");
            }
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult GestionarDenunciasUsuarios(string searchStringDenuncia, string currentFilter, int? page)
        {
            try
            {
                int intPage = 1;
                int intPageSize = 5;
                int intTotalPageCount = 0;

                if (searchStringDenuncia != null)
                {
                    intPage = 1;
                }
                else
                {
                    if (currentFilter != null)
                    {
                        searchStringDenuncia = currentFilter;
                        intPage = page ?? 1;
                    }
                    else
                    {
                        searchStringDenuncia = "";
                        intPage = page ?? 1;
                    }
                }

                ViewBag.CurrentFilter = searchStringDenuncia;

                int intSkip = (intPage - 1) * intPageSize;

                intTotalPageCount = db.Denuncias
                    .Where(x => x.Comentario.Contains(searchStringDenuncia) & x.UsersIdDenunciado != null & x.DateNull == null)
                    .Count();

                List<Denuncias> result = db.Denuncias
                    .Where(x => x.Comentario.Contains(searchStringDenuncia) & x.UsersIdDenunciado != null & x.DateNull == null)
                    .OrderBy(x => x.Comentario)
                    .Skip(intSkip)
                    .Take(intPageSize)
                    .ToList();

                ViewBag.Denuncias = result;
                ViewBag.TipoDenuncias = "Usuarios";

                return View("GestionarDenuncias");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                List<Denuncias> result = new List<Denuncias>();
                ViewBag.Denuncias = result;
                ViewBag.TipoDenuncias = "Experiencias";

                return View("GestionarDenuncias");
            }
        }


        [Authorize(Roles = "Administrador")]
        public ActionResult GestionarDenunciasComentarios(string searchStringDenuncia, string currentFilter, int? page)
        {
            try
            {
                int intPage = 1;
                int intPageSize = 5;
                int intTotalPageCount = 0;

                if (searchStringDenuncia != null)
                {
                    intPage = 1;
                }
                else
                {
                    if (currentFilter != null)
                    {
                        searchStringDenuncia = currentFilter;
                        intPage = page ?? 1;
                    }
                    else
                    {
                        searchStringDenuncia = "";
                        intPage = page ?? 1;
                    }
                }

                ViewBag.CurrentFilter = searchStringDenuncia;

                int intSkip = (intPage - 1) * intPageSize;

                intTotalPageCount = db.Denuncias
                    .Where(x => x.Comentario.Contains(searchStringDenuncia) & x.ComentarioId != null & x.DateNull == null)
                    .Count();

                List<Denuncias> result = db.Denuncias
                    .Where(x => x.Comentario.Contains(searchStringDenuncia) & x.ComentarioId != null & x.DateNull == null)
                    .OrderBy(x => x.Comentario)
                    .Skip(intSkip)
                    .Take(intPageSize)
                    .ToList();

                ViewBag.Denuncias = result;
                ViewBag.TipoDenuncias = "Comentarios";

                return View("GestionarDenuncias");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                List<Denuncias> result = new List<Denuncias>();
                ViewBag.Denuncias = result;
                ViewBag.TipoDenuncias = "Comentarios";

                return View("GestionarDenuncias");
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDenuncia(int id, string actionName)
        {
            Denuncias _denuncias = db.Denuncias.Find(id);
            _denuncias.DateNull = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("GestionarDenunciasProductos", "Admin");
        }

        // Users *****************************

        // GET: /Admin/Edit/Create 
        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            ExpandedUserViewModel nvm = new ExpandedUserViewModel();
            ViewBag.Roles = GetAllRolesAsSelectList();
            return View(nvm);
        }


        // PUT: /Admin/Create
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExpandedUserViewModel vm)
        {
            try
            {
                if (vm == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                //Validaciones
                if (String.IsNullOrWhiteSpace(vm.UserName))
                    ModelState.AddModelError("UserName", "Error en el campo Nombre de usuario.");
                if (AccountController.existUserNameBoolean(vm.UserName))
                    ModelState.AddModelError("UserName", "Este nombre de usuario ya existe.");
                if (String.IsNullOrWhiteSpace(vm.Email))
                    ModelState.AddModelError("Email", "Error en el campo Email.");
                if (AccountController.existEmailBoolean(vm.Email))
                    ModelState.AddModelError("Email", "Este Email ya existe.");
                if (String.IsNullOrWhiteSpace(vm.Password))
                    ModelState.AddModelError("Password", "Error en el campo Contraseña.");
                else
                {
                    string validacionPass = AccountController.IsValidPassword(vm.Password);
                    if (validacionPass != "")
                        ModelState.AddModelError("Password", validacionPass);
                }
                if (String.IsNullOrWhiteSpace(vm.RoleName) || vm.RoleName == "0")
                    ModelState.AddModelError("RoleName", "Error en el campo Role.");

                if (!ModelState.IsValid)
                {
                    ViewBag.Roles = GetAllRolesAsSelectList();
                    return View(vm);
                }
                else
                {
                    var Email = vm.Email.Trim();
                    var UserName = vm.UserName.Trim();
                    var Password = vm.Password.Trim();

                    // Create de usuario
                    var nAdminUser = new ApplicationUser { UserName = UserName, Email = Email };
                    var AdminUserCreateResult = UserManager.Create(nAdminUser, Password);

                    if (AdminUserCreateResult.Succeeded == true)
                    {
                        string strNewRole = vm.RoleName;

                        if (strNewRole != "0")
                        {
                            // Asignar rol a usuario
                            UserManager.AddToRole(nAdminUser.Id, strNewRole);
                        }
                        UsersInfoController.CreateEmptyUsersInfo(nAdminUser.Id);

                        return Redirect("~/Admin");
                    }
                    else
                    {
                        ViewBag.Roles = GetAllRolesAsSelectList();
                        ModelState.AddModelError(string.Empty,
                            "Error: Error al crear el usuario. Comprobar los requisitos del password.");
                        return View(vm);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Roles = GetAllRolesAsSelectList();
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View(vm);
            }
        }


        // GET: /Admin/Edit/EditUser 
        [Authorize(Roles = "Administrador")]
        public ActionResult EditUser(string UserName)
        {
            if (UserName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpandedUserViewModel nvm = GetUser(UserName);
            if (nvm == null)
            {
                return HttpNotFound();
            }
            return View(nvm);
        }


        // PUT: /Admin/EditUser
        [Authorize(Roles = "Administrador, Moderador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(ExpandedUserViewModel vm)
        {
            try
            {
                if (vm == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ExpandedUserViewModel nvm = UpdateDTOUser(vm);

                if (nvm == null)
                {
                    return HttpNotFound();
                }

                return Redirect("~/Admin");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("EditUser", GetUser(vm.UserName));
            }
        }

        [Authorize(Roles = "Administrador, Moderador")]
        public ActionResult DeleteUser(string UserName)
        {
            try
            {
                if (UserName == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (UserName.ToLower() == this.User.Identity.Name.ToLower())
                {
                    ModelState.AddModelError(
                        string.Empty, "Error: No se pudo eliminar el usuario");

                    return View("EditUser");
                }

                ExpandedUserViewModel nvm = GetUser(UserName);

                if (nvm == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    DeleteUser(nvm);
                }

                return Redirect("~/Admin");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("EditUser", GetUser(UserName));
            }
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult EditRoles(string UserName)
        {
            if (UserName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserName = UserName.ToLower();

            // Comprueba que exista el usuario
            ExpandedUserViewModel neuvm = GetUser(UserName);

            if (neuvm == null)
            {
                return HttpNotFound();
            }

            UserAndRolesViewModel nurvm = GetUserAndRoles(UserName);

            return View(nurvm);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRoles(UserAndRolesViewModel vm)
        {
            try
            {
                if (vm == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                string UserName = vm.UserName;
                string strNewRole = Convert.ToString(Request.Form["AddRole"]);

                if (strNewRole != "No Roles Found")
                {
                    // Buscar el usuario
                    ApplicationUser user = UserManager.FindByName(UserName);

                    // Asignar el rol al usuario
                    UserManager.AddToRole(user.Id, strNewRole);
                }

                ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));

                UserAndRolesViewModel nvm =
                    GetUserAndRoles(UserName);

                return View(nvm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("EditRoles");
            }
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult DeleteRole(string UserName, string RoleName)
        {
            try
            {
                if ((UserName == null) || (RoleName == null))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                UserName = UserName.ToLower();

                // Comprobar que exista el usuario
                ExpandedUserViewModel nvm = GetUser(UserName);

                if (nvm == null)
                {
                    return HttpNotFound();
                }

                if (UserName.ToLower() == this.User.Identity.Name.ToLower() && RoleName == "Administrador")
                {
                    ModelState.AddModelError(string.Empty,
                        "Error: No se pudo eliminar el Rol Administrador para el usuario");
                }

                // Buscar el usuario
                ApplicationUser user = UserManager.FindByName(UserName);
                // Desasignar rol al usuario
                UserManager.RemoveFromRoles(user.Id, RoleName);
                UserManager.Update(user);

                ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));

                return RedirectToAction("EditRoles", new { UserName = UserName });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);

                ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));

                UserAndRolesViewModel nvm =
                    GetUserAndRoles(UserName);

                return View("EditRoles", nvm);
            }
        }


        // Roles *****************************

        // GET: /Admin/ViewAllRoles
        [Authorize(Roles = "Administrador")]
        public ActionResult ViewAllRoles()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            List<RoleViewModel> listaRoleViewModel = (from objRole in roleManager.Roles
                                                      select new RoleViewModel
                                                      {
                                                          Id = objRole.Id,
                                                          RoleName = objRole.Name
                                                      }).ToList();

            return View(listaRoleViewModel);
        }


        // GET: /Admin/AddRole
        [Authorize(Roles = "Administrador")]
        public ActionResult AddRole()
        {
            RoleViewModel nvm = new RoleViewModel();

            return View(nvm);
        }


        // PUT: /Admin/AddRole
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRole(RoleViewModel vm)
        {
            try
            {
                if (vm == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var RoleName = vm.RoleName.Trim();

                if (RoleName == "")
                {
                    throw new Exception("No hay nombre de Rol");
                }

                // Crear Rol
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

                if (!roleManager.RoleExists(RoleName))
                {
                    roleManager.Create(new IdentityRole(RoleName));
                }

                return Redirect("~/Admin/ViewAllRoles");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("AddRole");
            }
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult DeleteUserRole(string RoleName)
        {
            try
            {
                if (RoleName == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (RoleName.ToLower() == "administrador")
                {
                    throw new Exception(String.Format("No se pudo eliminar el Rol {0}.", RoleName));
                }

                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

                var UsersInRole = roleManager.FindByName(RoleName).Users.Count();
                if (UsersInRole > 0)
                {
                    throw new Exception(String.Format("No se pudo eliminar el Rol {0} porque el mismo aun tiene usuarios asignados.", RoleName));
                }

                var objRoleToDelete = (from objRole in roleManager.Roles
                                       where objRole.Name == RoleName
                                       select objRole).FirstOrDefault();
                if (objRoleToDelete != null)
                {
                    roleManager.Delete(objRoleToDelete);
                }
                else
                {
                    throw new Exception(
                        String.Format(
                            "No se pudo eliminar el Rol {0} porque el mismo no existe.",
                            RoleName)
                            );
                }

                List<RoleViewModel> listaRoleViewModel = (from objRole in roleManager.Roles
                                                          select new RoleViewModel
                                                          {
                                                              Id = objRole.Id,
                                                              RoleName = objRole.Name
                                                          }).ToList();

                return View("ViewAllRoles", listaRoleViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);

                var roleManager =
                    new RoleManager<IdentityRole>(
                        new RoleStore<IdentityRole>(new ApplicationDbContext()));

                List<RoleViewModel> listaRoleViewModel = (from objRole in roleManager.Roles
                                                          select new RoleViewModel
                                                          {
                                                              Id = objRole.Id,
                                                              RoleName = objRole.Name
                                                          }).ToList();

                return View("ViewAllRoles", listaRoleViewModel);
            }
        }



        // Utility

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ??
                    HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ??
                    HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }


        private List<SelectListItem> GetAllRolesAsSelectList()
        {
            List<SelectListItem> SelectRoleListItems = new List<SelectListItem>();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            var colRoleSelectList = roleManager.Roles.OrderBy(x => x.Name).ToList();

            SelectRoleListItems.Add(new SelectListItem { Text = "Seleccione un rol", Value = "0" });

            foreach (var item in colRoleSelectList)
            {
                SelectRoleListItems.Add(new SelectListItem { Text = item.Name.ToString(), Value = item.Name.ToString() });
            }

            return SelectRoleListItems;
        }


        private ExpandedUserViewModel GetUser(string paramUserName)
        {
            ExpandedUserViewModel nvm = new ExpandedUserViewModel();

            var result = UserManager.FindByName(paramUserName);

            // Si no se encuentra el usuario, arrojar excepcion
            if (result == null) throw new Exception("No se pudo encontrar el usuario");

            nvm.UserName = result.UserName;
            nvm.Email = result.Email;
            nvm.LockoutEndDateUtc = result.LockoutEndDateUtc;
            nvm.AccessFailedCount = result.AccessFailedCount;
            nvm.PhoneNumber = result.PhoneNumber;
            nvm.IsBlocked = db.UsersInfo.Where(x => x.UsersId == result.Id).Select(x => x.IsBlocked).ToList()[0];

            return nvm;
        }


        private ExpandedUserViewModel UpdateDTOUser(ExpandedUserViewModel vm)
        {
            ApplicationUser result =
                UserManager.FindByName(vm.UserName);

            // Si no se encuentra el usuario, arrojar excepcion
            if (result == null)
            {
                throw new Exception("No se pudo encontrar el usuario");
            }

            result.Email = vm.Email;
            if (vm.LockoutEndDateUtc.HasValue)
                result.LockoutEndDateUtc = vm.LockoutEndDateUtc.Value;

            // Comprueba si la cuenta necesita ser desbloqueada.
            if (UserManager.IsLockedOut(result.Id))
            {
                // Desbloquea el usuario
                UserManager.ResetAccessFailedCountAsync(result.Id);
            }

            UserManager.Update(result);

            // ¿Se envió una contraseña?
            if (!string.IsNullOrEmpty(vm.Password))
            {
                // Eliminar la contraseña actual
                var removePassword = UserManager.RemovePassword(result.Id);
                if (removePassword.Succeeded)
                {
                    // Agregar nueva contraseña
                    var AddPassword = UserManager.AddPassword(result.Id, vm.Password);

                    if (AddPassword.Errors.Count() > 0)
                    {
                        throw new Exception(AddPassword.Errors.FirstOrDefault());
                    }
                }
            }

            UsersInfo u = db.UsersInfo.Find(result.Id);  //.Where(x => x.UsersId == result.Id)

            if (u.IsBlocked == true && vm.IsBlocked == false)
            {
                List<Denuncias> listaDenuncias = db.Denuncias.Where(x => !x.DateNull.HasValue && x.UsersIdDenunciado == u.UsersId).ToList();
                foreach (var denuncia in listaDenuncias)
                {
                    denuncia.DateNull = DateTime.Now;
                }
            }
            
            u.IsBlocked = vm.IsBlocked;

            db.SaveChanges();

            return vm;
        }


        private void DeleteUser(ExpandedUserViewModel vm)
        {
            ApplicationUser user =
                UserManager.FindByName(vm.UserName);

            // Si no se encuentra el usuario, arrojar excepcion
            if (user == null)
            {
                throw new Exception("No se pudo encontrar el usuario");
            }

            UserManager.RemoveFromRoles(user.Id, UserManager.GetRoles(user.Id).ToArray());
            UserManager.Update(user);
            UserManager.Delete(user);
        }


        private UserAndRolesViewModel GetUserAndRoles(string UserName)
        {
            // Buscar el usuario
            ApplicationUser user = UserManager.FindByName(UserName);

            List<UserRoleViewModel> listaUserRoleViewModel =
                (from objRole in UserManager.GetRoles(user.Id)
                 select new UserRoleViewModel
                 {
                     RoleName = objRole,
                     UserName = UserName
                 }).ToList();

            if (listaUserRoleViewModel.Count() == 0)
            {
                listaUserRoleViewModel.Add(new UserRoleViewModel { RoleName = "No Roles Found" });
            }

            ViewBag.AddRole = new SelectList(RolesUserIsNotIn(UserName));

            UserAndRolesViewModel nvm = new UserAndRolesViewModel();
            nvm.UserName = UserName;
            nvm.listaUserRoleViewModel = listaUserRoleViewModel;
            return nvm;
        }


        private List<string> RolesUserIsNotIn(string UserName)
        {
            // Obtiene todos los roles
            var listaAllRoles = RoleManager.Roles.Select(x => x.Name).ToList();

            // Obtiene los roles para el usuario
            ApplicationUser user = UserManager.FindByName(UserName);

            // Si no se encuentra el usuario, arrojar excepcion
            if (user == null)
            {
                throw new Exception("No se pudo encontrar el usuario");
            }

            var listaRolesForUser = UserManager.GetRoles(user.Id).ToList();
            var listaRolesUserInNotIn = (from objRole in listaAllRoles
                                         where !listaRolesForUser.Contains(objRole)
                                         select objRole).ToList();

            if (listaRolesUserInNotIn.Count() == 0)
            {
                listaRolesUserInNotIn.Add("No Roles Found");
            }

            return listaRolesUserInNotIn;
        }

        //CATEGORIAS

        [Authorize(Roles = "Administrador, Moderador")]
        public ActionResult GestionarCategorias()
        {
            List<ProductsCategories> listaProductsCategories = (from category in db.ProductsCategories
                                                                where category.DateNull == null
                                                                select category).ToList();

            return View(listaProductsCategories);
        }

        [Authorize(Roles = "Administrador, Moderador")]
        public ActionResult AddCategory()
        {
            ProductsCategories pc = new ProductsCategories();

            return View(pc);
        }

        [Authorize(Roles = "Administrador, Moderador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCategory(ProductsCategories pc, HttpPostedFileBase uploadedFile)
        {
            try
            {
                if (pc == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if ((from x in db.ProductsCategories where x.ProductCategoryName == pc.ProductCategoryName select x).Any())
                {
                    ModelState.AddModelError("ProductCategoryName", "Ya existe una categoría con este nombre.");
                }
                if (String.IsNullOrWhiteSpace(pc.ProductCategoryName))
                    ModelState.AddModelError("ProductCategoryName", "En el campo Nombre no puede estar vacio.");
                if (String.IsNullOrWhiteSpace(pc.ProductCategoryDescriptions))
                    ModelState.AddModelError("ProductCategoryDescriptions", "En el campo Descripción no puede estar vacio.");
                if (uploadedFile == null || uploadedFile.ContentLength == 0)
                    ModelState.AddModelError("FilePath", "Es obligatoria la carga de una imagen");


                if (!ModelState.IsValid)
                {
                    return View(pc);
                }
                else
                {
                    ProductsCategories productsCategories = new ProductsCategories();
                    if (uploadedFile != null && uploadedFile.ContentLength > 0)
                    {
                        string rutaServer = @"~\Content\Images";
                        string rutaSource = @"~/Content/Images/";
                        var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(uploadedFile.FileName);
                        var pathServer = Path.Combine(Server.MapPath(rutaServer), nombreArchivo);
                        var pathSource = rutaSource + nombreArchivo;
                        uploadedFile.SaveAs(pathServer);

                        productsCategories.FilePath = pathSource;
                    }
                    productsCategories.ProductCategoryName = pc.ProductCategoryName;
                    productsCategories.ProductCategoryDescriptions = pc.ProductCategoryDescriptions;
                    productsCategories.DateAdd = DateTime.Now;
                    db.ProductsCategories.Add(productsCategories);
                    db.SaveChanges();

                    List<ProductsCategories> listaProductsCategories = (from category in db.ProductsCategories
                                                                        where category.DateNull == null
                                                                        select category).ToList();

                    return View("GestionarCategorias", listaProductsCategories);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                ProductsCategories cat = new ProductsCategories();
                return View("AddCategory", cat);
            }
        }

        [Authorize(Roles = "Administrador, Moderador")]
        public ActionResult DeleteCategory(int ProductCategoryId)
        {
            try
            {
                ProductsCategories pc = db.ProductsCategories.Where(x => x.ProductCategoryId == ProductCategoryId).Single();

                if ((from x in db.Products where x.ProductsCategories.ProductCategoryId == ProductCategoryId select x).Any())
                {
                    ModelState.AddModelError("", String.Format("No se pudo eliminar la categoria \"{0}\" ya que esta asignada a una publicación.", pc.ProductCategoryName));
                }

                if (!ModelState.IsValid)
                {
                    List<ProductsCategories> listaProductsCategories = (from category in db.ProductsCategories
                                                                        where category.DateNull == null
                                                                        select category).ToList();

                    return View("GestionarCategorias", listaProductsCategories);
                }
                else
                {
                    pc.DateNull = DateTime.Now;
                    db.SaveChanges();

                    List<ProductsCategories> listaProductsCategories = (from category in db.ProductsCategories
                                                                        where category.DateNull == null
                                                                        select category).ToList();

                    return View("GestionarCategorias", listaProductsCategories);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);

                List<ProductsCategories> listaProductsCategories = (from category in db.ProductsCategories
                                                                    where category.DateNull == null
                                                                    select category).ToList();

                return View("GestionarCategorias", listaProductsCategories);
            }
        }

        [Authorize(Roles = "Administrador, Moderador")]
        public ActionResult EditCategory(int ProductCategoryId)
        {
            ProductsCategories pc = db.ProductsCategories.Where(x => x.ProductCategoryId == ProductCategoryId).Single();
            if (pc == null)
            {
                return HttpNotFound();
            }
            return View(pc);
        }

        [Authorize(Roles = "Administrador, Moderador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory(ProductsCategories pc, HttpPostedFileBase uploadedFile)
        {
            try
            {
                if (pc == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if ((from x in db.ProductsCategories where x.ProductCategoryName == pc.ProductCategoryName && x.ProductCategoryId != pc.ProductCategoryId select x).Any())
                {
                    ModelState.AddModelError("ProductCategoryName", "Ya existe una categoría con este nombre.");
                }
                if (String.IsNullOrWhiteSpace(pc.ProductCategoryName))
                    ModelState.AddModelError("ProductCategoryName", "En el campo Nombre no puede estar vacio.");
                if (String.IsNullOrWhiteSpace(pc.ProductCategoryDescriptions))
                    ModelState.AddModelError("ProductCategoryDescriptions", "En el campo Descripción no puede estar vacio.");

                if (!ModelState.IsValid)
                {
                    return View(pc);
                }
                else
                {

                    ProductsCategories productsCategories = db.ProductsCategories.Where(x => x.ProductCategoryId == pc.ProductCategoryId).Single();
                    if (uploadedFile != null && uploadedFile.ContentLength > 0)
                    {
                        string rutaServer = @"~\Content\Images";
                        string rutaSource = @"~/Content/Images/";
                        var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(uploadedFile.FileName);
                        var pathServer = Path.Combine(Server.MapPath(rutaServer), nombreArchivo);
                        var pathSource = rutaSource + nombreArchivo;
                        uploadedFile.SaveAs(pathServer);

                        productsCategories.FilePath = pathSource;
                    }
                    productsCategories.ProductCategoryName = pc.ProductCategoryName;
                    productsCategories.ProductCategoryDescriptions = pc.ProductCategoryDescriptions;
                    db.SaveChanges();

                    List<ProductsCategories> listaProductsCategories = (from category in db.ProductsCategories
                                                                        where category.DateNull == null
                                                                        select category).ToList();

                    return View("GestionarCategorias", listaProductsCategories);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);

                ProductsCategories category = db.ProductsCategories.Where(x => x.ProductCategoryId == pc.ProductCategoryId).Single();
                return View("EditCategory", category);
            }
        }
    }
}