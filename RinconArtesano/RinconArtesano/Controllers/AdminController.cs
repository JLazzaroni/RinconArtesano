using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using RinconArtesano.Models;

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
                    .Where(x => x.ExperienceTitle.Contains(searchStringExperience) & (x.DateNull.HasValue || x.Bloqueado == true))
                    .Count();

                List<Experiences> result = db.Experiences
                    .Where(x => x.ExperienceTitle.Contains(searchStringExperience) & (x.DateNull.HasValue || x.Bloqueado == true))
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
                    .Where(x => x.ProductTitle.Contains(searchStringProduct) & (x.DateNull.HasValue || x.Bloqueado == true))
                    .Count();

                List<Products> result = db.Products
                    .Where(x => x.ProductTitle.Contains(searchStringProduct) & (x.DateNull.HasValue || x.Bloqueado == true))
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

                var Email = vm.Email.Trim();
                var UserName = vm.UserName.Trim();
                var Password = vm.Password.Trim();

                if (Email == "")
                {
                    throw new Exception("No Email");
                }

                if (Password == "")
                {
                    throw new Exception("No Password");
                }

                // UserName es el mail en minusculas
                //UserName = Email.ToLower();

                // Create de usuario

                var nAdminUser = new ApplicationUser { UserName = UserName, Email = Email };
                var AdminUserCreateResult = UserManager.Create(nAdminUser, Password);

                if (AdminUserCreateResult.Succeeded == true)
                {
                    string strNewRole = Convert.ToString(Request.Form["Roles"]);

                    if (strNewRole != "0")
                    {
                        // Asignar rol a usuario
                        UserManager.AddToRole(nAdminUser.Id, strNewRole);
                    }

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
            catch (Exception ex)
            {
                ViewBag.Roles = GetAllRolesAsSelectList();
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                return View("Create");
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

                if (User.IsInRole("Moderador"))
                    return Redirect("~/Moderador");
                else
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

                if (User.IsInRole("Moderador"))
                    return Redirect("~/Moderador");
                else
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
                    throw new Exception( String.Format("No se pudo eliminar el Rol {0} porque el mismo aun tiene usuarios asignados.",RoleName));
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

            SelectRoleListItems.Add( new SelectListItem { Text = "Select", Value = "0" });

            foreach (var item in colRoleSelectList)
            {
                SelectRoleListItems.Add( new SelectListItem { Text = item.Name.ToString(), Value = item.Name.ToString() });
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
                    var AddPassword = UserManager.AddPassword( result.Id, vm.Password );

                    if (AddPassword.Errors.Count() > 0)
                    {
                        throw new Exception(AddPassword.Errors.FirstOrDefault());
                    }
                }
            }

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

    }
}