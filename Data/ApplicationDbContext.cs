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

        public Villa? GetVillaById(int id)
        {
            return Villas.AsNoTracking().FirstOrDefault(x => x.Id == id);
        }

        public Villa? GetVillaByName (string name)
        {
            return Villas.FirstOrDefault(v => v.Name.ToLower() == name.ToLower());
        }
    }
}
