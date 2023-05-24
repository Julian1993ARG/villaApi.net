using MagicVilla.Data;
using MagicVilla.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla.Services
{
    public class VillasServices
    {
        private readonly ApplicationDbContext _dbcontext;

        public VillasServices(ApplicationDbContext context)
        {
            _dbcontext = context;
        }

        public async Task<IEnumerable<Villa>> GetVillas() => await _dbcontext.Villas.ToListAsync();

        public async Task<Villa?> GetVillaById(int id) => await _dbcontext.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);

        public async Task<Villa?> GetVillaByName(string name) => await _dbcontext.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Name.ToLower() == name.ToLower());

        public async Task CreateVilla(Villa villa)
        {
            _dbcontext.Villas.Add(villa);
            var newVilla = await _dbcontext.SaveChangesAsync();
        }

        public async Task DeleteVilla(Villa villa)
        {
            _dbcontext.Villas.Remove(villa);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task UpdateVilla(Villa villa)
        {
            _dbcontext.Entry(villa).State = EntityState.Modified;
            await _dbcontext.SaveChangesAsync();
        }

        
    }
}
