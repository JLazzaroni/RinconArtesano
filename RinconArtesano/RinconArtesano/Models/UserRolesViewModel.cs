using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Datos;

namespace RinconArtesano.Models
{
    public class ExpandedUserViewModel
    {
        [System.Web.Mvc.Remote("existUserName", "Account", "", ErrorMessage = "Este nombre de usuario ya existe.", HttpMethod = "POST")]
        [Required]
        [StringLength(256, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 4)]
        [Display(Name = "Nombre de usuario")]
        //[Key]
        //[Display(Name = "Nombre Usuario")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
        [Display(Name = "Fecha de finalización del bloqueo Utc")]
        public DateTime? LockoutEndDateUtc { get; set; }
        public int AccessFailedCount { get; set; }
        public string PhoneNumber { get; set; }
        [Display(Name = "Rol")]
        public string RoleName { get; set; }
        public IEnumerable<UserRolesViewModel> Roles { get; set; }
    }

    public class UserRolesViewModel
    {
        [Key]
        [Display(Name = "Nombre Rol")]
        public string RoleName { get; set; }
    }

    public class UserRoleViewModel
    {
        [Key]
        [Display(Name = "Nombre Usuario")]
        public string UserName { get; set; }
        [Display(Name = "Nombre Rol")]
        public string RoleName { get; set; }
    }

    public class RoleViewModel
    {
        [Key]
        public string Id { get; set; }
        [Display(Name = "Nombre Rol")]
        public string RoleName { get; set; }
    }

    public class UserAndRolesViewModel
    {
        [Key]
        [Display(Name = "Nombre Usuario")]
        public string UserName { get; set; }
        public List<UserRoleViewModel> listaUserRoleViewModel { get; set; }
    }
}