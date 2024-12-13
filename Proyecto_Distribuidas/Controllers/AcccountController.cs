using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Distribuidas.Models;
using Proyecto_Distribuidas.ViewModels; // Suponiendo que tienes un ViewModel para la vista de Login y Register
using Serilog;
using System;
using System.Threading.Tasks;
using MicrosoftLogger = Microsoft.Extensions.Logging.ILogger<Proyecto_Distribuidas.Controllers.AccountController>;


namespace Proyecto_Distribuidas.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;

        // Inyección de dependencias
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, ILogger logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;  // Ahora el logger está disponible para usarlo
        }

        // Acción para la vista de registro
        public IActionResult Register()
        {
            return View();
        }

        // Acción POST para el registro de usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        // Crear rol "User" si no existe
                        if (!await _roleManager.RoleExistsAsync("User"))
                        {
                            await _roleManager.CreateAsync(new IdentityRole("User"));
                        }

                        // Asignar rol al usuario
                        await _userManager.AddToRoleAsync(user, "User");

                        // Iniciar sesión automáticamente después del registro
                        await _signInManager.SignInAsync(user, isPersistent: false);

                        // Registrar el evento en los logs
                        _logger.Information("Nuevo usuario registrado: {username}", model.Email);

                        return RedirectToAction("Index", "Home"); // Redirigir a la página principal
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error("Error durante el registro del usuario {username}: {message}", model.Email, ex.Message);
                    ModelState.AddModelError(string.Empty, "Hubo un error al registrar el usuario.");
                }
            }

            return View(model);
        }

        // Acción para la vista de inicio de sesión
        public IActionResult Login()
        {
            return View();
        }

        // Acción POST para el inicio de sesión
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        // Registrar el evento de inicio de sesión exitoso
                        _logger.Information("Inicio de sesión exitoso: {username}", model.Email);

                        return RedirectToLocal(returnUrl);
                    }
                    if (result.IsLockedOut)
                    {
                        ModelState.AddModelError(string.Empty, "Cuenta bloqueada.");

                        // Registrar intento de inicio de sesión bloqueado
                        _logger.Warning("Cuenta bloqueada para el usuario: {username}", model.Email);

                        return View(model);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Intento de inicio de sesión no válido.");

                        // Registrar intento fallido de inicio de sesión
                        _logger.Warning("Intento de inicio de sesión fallido para el usuario: {username}", model.Email);

                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error("Error durante el inicio de sesión para {username}: {message}", model.Email, ex.Message);
                    ModelState.AddModelError(string.Empty, "Hubo un error al intentar iniciar sesión.");
                }
            }

            return View(model);
        }

        // Acción para cerrar sesión
        public async Task<IActionResult> Logout()
        {
            var username = User.Identity.Name;
            await _signInManager.SignOutAsync();

            // Registrar evento de cierre de sesión
            _logger.Information("Usuario {username} cerró sesión. IP: {ip}", username, Request.HttpContext.Connection.RemoteIpAddress);

            return RedirectToAction("Index", "Home");
        }

        // Redirigir al usuario a la página de retorno o la principal si no hay una URL de retorno
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
