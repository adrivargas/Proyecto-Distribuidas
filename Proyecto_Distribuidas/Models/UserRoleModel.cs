using System.ComponentModel.DataAnnotations;

namespace Proyecto_Distribuidas.Models
{
    // Modelo para asignación de roles a usuarios
    public class UserRoleModel
    {
        // Propiedad para el ID del usuario
        [Required]
        public string UserId { get; set; }

        // Propiedad para el ID del rol
        [Required]
        public string RoleId { get; set; }

        // Propiedad para el nombre del usuario (opcional)
        [Required]
        [StringLength(100)]
        [Display(Name = "Nombre de Usuario")]
        public string UserName { get; set; }

        // Propiedad para el nombre del rol (opcional)
        [Required]
        [StringLength(100)]
        [Display(Name = "Nombre de Rol")]
        public string RoleName { get; set; }
    }
}
