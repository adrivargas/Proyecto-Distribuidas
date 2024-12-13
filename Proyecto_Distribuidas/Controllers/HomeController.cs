using Microsoft.AspNetCore.Mvc;
using Proyecto_Distribuidas.Models;
using Proyecto_Distribuidas.Services;  // Aseg�rate de agregar esta l�nea
using System.Diagnostics;

namespace Proyecto_Distribuidas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AuthServices _authServices;  // Declarar el servicio AuthServices

        // Inyecci�n de dependencias
        public HomeController(ILogger<HomeController> logger, AuthServices authServices)
        {
            _logger = logger;
            _authServices = authServices;  // Asignar el servicio AuthServices
        }

        // Acci�n para la vista de inicio
        public IActionResult Index()
        {
            // Aqu� puedes usar el servicio AuthServices para obtener informaci�n sobre el usuario, verificar roles, etc.
            var userEmail = User.Identity?.Name;  // Obtener el correo electr�nico del usuario
            if (!string.IsNullOrEmpty(userEmail))
            {
                var user = _authServices.GetUserByEmailAsync(userEmail).Result;
                if (user != null)
                {
                    // Aqu� puedes hacer alguna l�gica, por ejemplo, verificar roles
                    var isAdmin = _authServices.UserHasRoleAsync(user, "Admin").Result;
                    if (isAdmin)
                    {
                        // L�gica para usuarios con rol Admin
                    }
                }
            }

            return View();
        }

        // Acci�n para la vista de privacidad
        public IActionResult Privacy()
        {
            return View();
        }

        // Acci�n para manejar errores
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
