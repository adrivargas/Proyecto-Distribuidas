using Microsoft.AspNetCore.Mvc;
using Proyecto_Distribuidas.Models;
using Proyecto_Distribuidas.Services;  // Asegúrate de agregar esta línea
using System.Diagnostics;

namespace Proyecto_Distribuidas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AuthServices _authServices;  // Declarar el servicio AuthServices

        // Inyección de dependencias
        public HomeController(ILogger<HomeController> logger, AuthServices authServices)
        {
            _logger = logger;
            _authServices = authServices;  // Asignar el servicio AuthServices
        }

        // Acción para la vista de inicio
        public IActionResult Index()
        {
            // Aquí puedes usar el servicio AuthServices para obtener información sobre el usuario, verificar roles, etc.
            var userEmail = User.Identity?.Name;  // Obtener el correo electrónico del usuario
            if (!string.IsNullOrEmpty(userEmail))
            {
                var user = _authServices.GetUserByEmailAsync(userEmail).Result;
                if (user != null)
                {
                    // Aquí puedes hacer alguna lógica, por ejemplo, verificar roles
                    var isAdmin = _authServices.UserHasRoleAsync(user, "Admin").Result;
                    if (isAdmin)
                    {
                        // Lógica para usuarios con rol Admin
                    }
                }
            }

            return View();
        }

        // Acción para la vista de privacidad
        public IActionResult Privacy()
        {
            return View();
        }

        // Acción para manejar errores
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(errorModel);
        }
    }
}
