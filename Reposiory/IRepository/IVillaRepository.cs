using MagicVilla.Models;

namespace MagicVilla.Reposiory.IRepository
{
    public interface IVillaRepository : IRepository<Villa>
    {
        Task<Villa> Update(Villa entidad);
    }
}
