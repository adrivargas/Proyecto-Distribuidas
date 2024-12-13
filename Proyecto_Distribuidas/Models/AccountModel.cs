using System.ComponentModel.DataAnnotations;

namespace Proyecto_Distribuidas.Models
{
    // Modelo para la vista de Registro
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Correo electr�nico")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contrase�a")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contrase�a")]
        [Compare("Password", ErrorMessage = "La contrase�a y su confirmaci�n no coinciden.")]
        public string ConfirmPassword { get; set; }
    }

    // Modelo para la vista de Inicio de Sesi�n
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Correo electr�nico")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contrase�a")]
        public string Password { get; set; }

        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }
    }

    // Modelo para la vista de Olvido de Contrase�a
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Correo electr�nico")]
        public string Email { get; set; }
    }

    // Modelo para la vista de Restablecer Contrase�a
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Correo electr�nico")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contrase�a")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contrase�a")]
        [Compare("Password", ErrorMessage = "La contrase�a y su confirmaci�n no coinciden.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    // Modelo para el cambio de contrase�a
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contrase�a Actual")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva Contrase�a")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Nueva Contrase�a")]
        [Compare("NewPassword", ErrorMessage = "La nueva contrase�a y su confirmaci�n no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}
