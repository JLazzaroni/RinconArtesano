using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RinconArtesano.Models
{
    public class SearchViewModel
    {
        public List<SearchProductsViewModel> Products { get; set; }
        public List<SearchProductsCategoriesViewModel> ProductsCategories { get; set; }
        public List<SearchExperiencesViewModel> Experiences { get; set; }
        public List<SearchAspNetUsersViewModel> AspNetUsers { get; set; }
        public List<SearchUsersInfoViewModel> UsersInfo { get; set; }
        public List<SearchMessagesViewModel> Messages { get; set; }
        
    }

    public class SearchProductsViewModel
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string ProductDescription { get; set; }
        //
        public int Rank { get; set; }
        public string Categories { get; set; }
        public int TotalRecords { get; set; }
    }
    public class SearchProductsCategoriesViewModel
    {
        public int ProductCategoryId { get; set; }
		public string ProductCategoryName { get; set; }
        public string ProductCategoryDescriptions { get; set; }
        //
        public int Rank { get; set; }
        public int TotalRecords { get; set; }
    }
    public class SearchExperiencesViewModel
    {
        public int ExperienceId { get; set; }
        public string ExperienceTitle { get; set; }
        public string ExperienceDescription { get; set; }
        //
        public int Rank { get; set; }
        public int TotalRecords { get; set; }
    }
    public class SearchAspNetUsersViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        //
        public int Rank { get; set; }
        public int TotalRecords { get; set; }
    }
    public class SearchUsersInfoViewModel
    {
        public string UsersId { get; set; }
        public string UserName { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Edad { get; set; }
        public string Sexo { get; set; }
        public string Pais { get; set; }
        public string Provincia { get; set; }
        public string Localidad { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Intereses { get; set; }
        public string Rubro { get; set; }
        //
        public int Rank { get; set; }
        public int TotalRecords { get; set; }
    }
    public class SearchMessagesViewModel
    {
        public int IdComentario { get; set; }
		public int? IdComentarioPadre { get; set; }
		public string UsersId { get; set; }
		public int Category { get; set; }   //	1-Productos / 2-Experiencias
		public int CategoryId { get; set; }	//	Id de la entidad (Producto o Experiencia)
        public string Message { get; set; }
        //
        public int Rank { get; set; }
        public int TotalRecords { get; set; }
    }

    //

    public class SearchMessagesProducts
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string UserName { get; set; }
    }

    public class SearchMessagesExperiences
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string UserName { get; set; }
    }
}