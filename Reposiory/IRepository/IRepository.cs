using System.Linq.Expressions;

namespace MagicVilla.Reposiory.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task Save();
        Task Create(T entidad);
        Task Delete(T entidad);
        Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null);
        Task<T?> Get(Expression<Func<T, bool>> filter = null, bool tracked=true);

    }
}
