using MagicVilla.Data;
using MagicVilla.Models;
using MagicVilla.Reposiory.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla.Reposiory
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext _db;
        public VillaNumberRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<VillaNumber> Update(VillaNumber entidad)
        {
            entidad.UpdateAt = DateTime.Now;
            _db.Entry(entidad).State = EntityState.Modified;
            await Save();
            return entidad;
        }

        public async Task<VillaNumber?> GetWithVilla(int id)
        {
            return await _db.VillaNumbers.Include(x => x.Villa).FirstOrDefaultAsync(x => x.VillaNo == id);
        }
    }
}
