using MagicVilla.Data;
using MagicVilla.Reposiory.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla.Reposiory
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public async Task Save() => await _db.SaveChangesAsync();

        public async Task Create(T entidad)
        {
            await dbSet.AddAsync(entidad);
            await Save();
        }

        public async Task Delete(T entidad)
        {
            dbSet.Entry(entidad).State = EntityState.Deleted;
            await Save();
        }

        public async Task<T?> Get(Expression<Func<T, bool>> filter = null, bool tracked = true, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbSet;
            if (!tracked) {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

       
    }
}
