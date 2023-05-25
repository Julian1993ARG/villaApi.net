using MagicVilla.Models;

namespace MagicVilla.Reposiory.IRepository
{
    public interface IVillaNumberRepository : IRepository<VillaNumber>
    {
        Task<VillaNumber> Update(VillaNumber entidad);
    }
}
