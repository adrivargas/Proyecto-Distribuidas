using Microsoft.AspNetCore.Identity;
using Proyecto_Distribuidas.Models;

namespace Proyecto_Distribuidas.Services
{
    public class AuthServices
    {
        private readonly UserManager<ApplicationUser> _userManager;

        // Constructor donde se inyecta UserManager
        public AuthServices(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // M�todo para obtener un usuario por correo electr�nico
        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        // M�todo para verificar si un usuario tiene un rol espec�fico
        public async Task<bool> UserHasRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }

        // M�todo para registrar un nuevo usuario
        public async Task<IdentityResult> RegisterUserAsync(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }
    }
}
