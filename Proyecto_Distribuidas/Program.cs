using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Proyecto_Distribuidas.Data;
using Proyecto_Distribuidas.Models;
using Proyecto_Distribuidas.Services;
using Serilog;
using System.Text;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configuración de Serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("app_logs.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        builder.Host.UseSerilog(); // Usar Serilog como el proveedor de logs

        // Configuración de servicios
        builder.Services.AddRazorPages();
        builder.Services.AddControllersWithViews();

        // Registrar AuthServices en el contenedor de dependencias
        builder.Services.AddScoped<AuthServices>();  // Esto registra AuthServices

        // Configuración de la base de datos
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Configuración de Identity con políticas de contraseñas fuertes y bloqueo de cuentas
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            // Políticas de contraseñas fuertes
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;

            // Configuración de bloqueo de cuentas
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);  // Tiempo de bloqueo
            options.Lockout.MaxFailedAccessAttempts = 5; // Intentos fallidos antes de bloquear
            options.Lockout.AllowedForNewUsers = true;  // Aplicar a nuevos usuarios
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // Configuración de autenticación JWT
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

        var app = builder.Build();  // Crear la aplicación

        // Configuración de middleware
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();  // Habilitar autenticación
        app.UseAuthorization();   // Habilitar autorización

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();  // Ejecutar la aplicación
    }
}
