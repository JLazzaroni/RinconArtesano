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
        [Key]
        [Display(Name = "Nombre Usuario")]
        public string UserName { get; set; }
        public string Email { get; set; }
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