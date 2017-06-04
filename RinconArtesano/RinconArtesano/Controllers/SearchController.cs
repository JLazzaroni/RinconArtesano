using Datos;
using RinconArtesano.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace RinconArtesano.Controllers
{
    public class SearchController : Controller
    {
        private RinconArtesanoEntities db = new RinconArtesanoEntities();
        // GET: Search
        [HttpPost]
        public ActionResult Search(string searchTerm)
        {
            return Json(
                new
                {
                    url = Url.Action("Index", "Search"),
                    searchTerm = searchTerm
                },
                JsonRequestBehavior.AllowGet);
        }

        // GET: Search
        public ActionResult Index(string searchTerm)
        {
            try
            {
                searchTerm = Regex.Replace(searchTerm, @"[^\sa-zA-Z]", string.Empty).Trim();
                var param1 = new SqlParameter("@SearchTerm", searchTerm);
                var productsResult = db.Database.SqlQuery<SearchProductsViewModel>("SearchProducts @SearchTerm", param1).ToList();
                var param2 = new SqlParameter("@SearchTerm", searchTerm);
                var productsCategoriesResult = db.Database.SqlQuery<SearchProductsCategoriesViewModel>("SearchProductsCategories @SearchTerm", param2).ToList();
                var param3 = new SqlParameter("@SearchTerm", searchTerm);
                var experiencesResult = db.Database.SqlQuery<SearchExperiencesViewModel>("SearchExperiences @SearchTerm", param3).ToList();
                var param4 = new SqlParameter("@SearchTerm", searchTerm);
                var aspNetUsersResult = db.Database.SqlQuery<SearchAspNetUsersViewModel>("SearchAspNetUsers @SearchTerm", param4).ToList();
                var param5 = new SqlParameter("@SearchTerm", searchTerm);
                var usersInfoResult = db.Database.SqlQuery<SearchUsersInfoViewModel>("SearchUsersInfo @SearchTerm", param5).ToList();
                var param6 = new SqlParameter("@SearchTerm", searchTerm);
                var messagesResult = db.Database.SqlQuery<SearchMessagesViewModel>("SearchMessages @SearchTerm", param6).ToList();


                SearchViewModel s = new SearchViewModel();

                //Productos
                List<SearchProductsViewModel> listProducts = new List<SearchProductsViewModel>();
                if (productsResult != null && productsResult.Count >= 0)
                {
                    foreach (var item in productsResult)
                    {
                        var sProducts = new SearchProductsViewModel
                        {
                            ProductId = item.ProductId,
                            ProductTitle = item.ProductTitle,
                            ProductDescription = item.ProductDescription,
                            Rank = item.Rank,
                            Categories = item.Categories,
                            TotalRecords = item.TotalRecords
                        };
                        listProducts.Add(sProducts);
                    }
                }
                s.Products = listProducts;

                //ProductosCategorias
                List<SearchProductsCategoriesViewModel> listProductsCategories = new List<SearchProductsCategoriesViewModel>();
                if (productsCategoriesResult != null && productsCategoriesResult.Count >= 0)
                {
                    foreach (var item in productsCategoriesResult)
                    {
                        var sProductsCategories = new SearchProductsCategoriesViewModel
                        {
                            ProductCategoryId = item.ProductCategoryId,
                            ProductCategoryName = item.ProductCategoryName,
                            ProductCategoryDescriptions = item.ProductCategoryDescriptions,
                            Rank = item.Rank,
                            TotalRecords = item.TotalRecords
                        };
                        listProductsCategories.Add(sProductsCategories);
                    }
                }
                s.ProductsCategories = listProductsCategories;

                //Experiencias
                List<SearchExperiencesViewModel> listExperiences = new List<SearchExperiencesViewModel>();
                if (experiencesResult != null && experiencesResult.Count >= 0)
                {
                    foreach (var item in experiencesResult)
                    {
                        var sExperiences = new SearchExperiencesViewModel
                        {
                            ExperienceId = item.ExperienceId,
                            ExperienceTitle = item.ExperienceTitle,
                            ExperienceDescription = item.ExperienceDescription,
                            Rank = item.Rank,
                            TotalRecords = item.TotalRecords
                        };
                        listExperiences.Add(sExperiences);
                    }
                }
                s.Experiences = listExperiences;

                //AspNetUsers
                List<SearchAspNetUsersViewModel> listAspNetUsers = new List<SearchAspNetUsersViewModel>();
                if (aspNetUsersResult != null && aspNetUsersResult.Count >= 0)
                {
                    foreach (var item in aspNetUsersResult)
                    {
                        var sAspNetUsers = new SearchAspNetUsersViewModel
                        {
                            Id = item.Id,
                            UserName = item.UserName,
                            Rank = item.Rank,
                            TotalRecords = item.TotalRecords
                        };
                        listAspNetUsers.Add(sAspNetUsers);
                    }
                }
                s.AspNetUsers = listAspNetUsers;

                //UsersInfo
                List<SearchUsersInfoViewModel> listUsersInfo = new List<SearchUsersInfoViewModel>();
                if (usersInfoResult != null && usersInfoResult.Count > 0)
                {
                    foreach (var item in usersInfoResult)
                    {
                        var sUsersInfo = new SearchUsersInfoViewModel
                        {
                            UsersId = item.UsersId,
                            UserName = item.UserName,
                            Nombre = item.Nombre,
                            Apellido = item.Apellido,
                            Edad = item.Edad,
                            Sexo = item.Sexo,
                            Pais = item.Pais,
                            Provincia = item.Provincia,
                            Localidad = item.Localidad,
                            Direccion = item.Direccion,
                            Telefono = item.Telefono,
                            Intereses = item.Intereses,
                            Rubro = item.Rubro,
                            Rank = item.Rank,
                            TotalRecords = item.TotalRecords
                        };
                        listUsersInfo.Add(sUsersInfo);
                    }
                }
                s.UsersInfo = listUsersInfo;

                //Comentarios
                List<SearchMessagesViewModel> listMessages = new List<SearchMessagesViewModel>();
                if (messagesResult != null && messagesResult.Count >= 0)
                {
                    foreach (var item in messagesResult)
                    {
                        var sMessages = new SearchMessagesViewModel
                        {
                            IdComentario = item.IdComentario,
                            IdComentarioPadre = item.IdComentarioPadre,
                            UsersId = item.UsersId,
                            Category = item.Category,
                            CategoryId = item.CategoryId,
                            Message = item.Message,
                            Rank = item.Rank,
                            TotalRecords = item.TotalRecords
                        };
                        listMessages.Add(sMessages);
                    }
                }
                s.Messages = listMessages;


                //
                //
                List<SearchMessagesProducts> mProd = new List<SearchMessagesProducts>();
                foreach (var x in s.Messages.Where(x => x.Category == 1))
                {
                    SearchMessagesProducts mp = new SearchMessagesProducts
                    {
                        Id = x.CategoryId,
                        Title = db.Products.Find(x.CategoryId).ProductTitle,
                        UserName = db.Products.Find(x.CategoryId).AspNetUsers.UserName
                    };
                    mProd.Add(mp);
                }

                List<SearchMessagesExperiences> mExp = new List<SearchMessagesExperiences>();
                foreach (var x in s.Messages.Where(x => x.Category == 2))
                {
                    SearchMessagesExperiences me = new SearchMessagesExperiences
                    {
                        Id = x.CategoryId,
                        Title = db.Products.Find(x.CategoryId).ProductTitle,
                        UserName = db.Products.Find(x.CategoryId).AspNetUsers.UserName
                    };
                    mExp.Add(me);
                }

                ViewBag.MessagesProducts = mProd;
                ViewBag.MessagesExperiences = mExp;

                return View(s);
            }
            catch
            {
                SearchViewModel s = new SearchViewModel();

                //Products
                List<SearchProductsViewModel> listProducts = new List<SearchProductsViewModel>();
                s.Products = listProducts;

                //ProductosCategorias
                List<SearchProductsCategoriesViewModel> listProductsCategories = new List<SearchProductsCategoriesViewModel>();
                s.ProductsCategories = listProductsCategories;

                //Experiencias
                List<SearchExperiencesViewModel> listExperiences = new List<SearchExperiencesViewModel>();
                s.Experiences = listExperiences;

                //AspNetUsers
                List<SearchAspNetUsersViewModel> listAspNetUsers = new List<SearchAspNetUsersViewModel>();
                s.AspNetUsers = listAspNetUsers;

                //UsersInfo
                List<SearchUsersInfoViewModel> listUsersInfo = new List<SearchUsersInfoViewModel>();
                s.UsersInfo = listUsersInfo;

                //Comentarios
                List<SearchMessagesViewModel> listMessages = new List<SearchMessagesViewModel>();
                s.Messages = listMessages;

                //
                //
                List<SearchMessagesProducts> mProd = new List<SearchMessagesProducts>();
                List<SearchMessagesExperiences> mExp = new List<SearchMessagesExperiences>();

                ViewBag.MessagesProducts = mProd;
                ViewBag.MessagesExperiences = mExp;
                return View(s);
            }
        }
    }
}
