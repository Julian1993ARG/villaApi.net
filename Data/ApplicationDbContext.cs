using MagicVilla.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }
        // Comando para ejecutar Migrate : Add-Migration InitialCreate
        // Comando para ejecutar Migrate : Update-Database
    }
}
