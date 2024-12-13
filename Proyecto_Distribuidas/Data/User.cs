using Microsoft.AspNetCore.Identity;

namespace Proyecto_Distribuidas.Models
{
    // La clase ApplicationUser extiende IdentityUser para agregar propiedades adicionales si es necesario
    public class ApplicationUser : IdentityUser
    {
        // Aquí puedes agregar propiedades personalizadas si lo deseas
        // Ejemplo: Primer nombre, apellido, etc.
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
