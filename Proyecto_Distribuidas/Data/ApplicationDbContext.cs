using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Proyecto_Distribuidas.Models;
using System.Collections.Generic;

namespace Proyecto_Distribuidas.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

    
        public DbSet<Product> Products { get; set; }

        // Si tienes una entidad llamada 'Order'
        public DbSet<Order> Orders { get; set; }
    }
}
