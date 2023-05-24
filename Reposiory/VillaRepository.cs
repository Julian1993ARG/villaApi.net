using MagicVilla.Data;
using MagicVilla.Models;
using MagicVilla.Reposiory.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla.Reposiory
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _db;
        public VillaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Villa> Update(Villa entidad)
        {
            entidad.UpdateAt = DateTime.Now;
            _db.Entry(entidad).State = EntityState.Modified;
            await Save();
            return entidad;
        }
    }
}
