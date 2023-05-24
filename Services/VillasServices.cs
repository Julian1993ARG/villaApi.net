using AutoMapper;
using MagicVilla.Data;
using MagicVilla.Models;
using MagicVilla.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla.Services
{
    public class VillasServices
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IMapper _mapper;

        public Villa RetVilla(VillaDto villa) => _mapper.Map<Villa>(villa);
        public Villa RetVilla(VillaUpdateDto villa) => _mapper.Map<Villa>(villa);
        public Villa RetVilla(VillaCreateDto villa) => _mapper.Map<Villa>(villa);
        public VillaDto RetVilla(Villa villa) => _mapper.Map<VillaDto>(villa);

        public IEnumerable<VillaDto> RetVillas(IEnumerable<Villa> villas) => _mapper.Map<IEnumerable<VillaDto>>(villas);


        public VillasServices(ApplicationDbContext context, IMapper mapper)
        {
            _dbcontext = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VillaDto>> GetVillas()
        {
          IEnumerable<Villa> villas = await _dbcontext.Villas.ToListAsync();
          return RetVillas(villas);
        }

        public async Task<VillaDto?> GetVillaById(int id)
        {
           var villa = await _dbcontext.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
            if (villa != null) return RetVilla(villa);
            else return null;
        }

        public async Task<Villa?> GetVillaByName(string name) => await _dbcontext.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Name.ToLower() == name.ToLower());

        public async Task<int> CreateVilla(VillaCreateDto villa)
        {
            var newVilla =  RetVilla(villa);
            _dbcontext.Villas.Add(newVilla);
            await _dbcontext.SaveChangesAsync();
            return newVilla.Id;
        }

        public async Task DeleteVilla(VillaDto villadto)
        {
            var villa = RetVilla(villadto);
            _dbcontext.Entry(villa).State = EntityState.Deleted;
            //_dbcontext.Villas.Remove(villa);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task UpdateVilla(int id, VillaUpdateDto newVilla)
        {
            var villa = RetVilla(newVilla);
            villa.Id = id;
            _dbcontext.Entry(villa).State = EntityState.Modified;
            await _dbcontext.SaveChangesAsync();
        }
        public async Task UpdateVilla(Villa newVilla)
        {
            _dbcontext.Entry(newVilla).State = EntityState.Modified;
            await _dbcontext.SaveChangesAsync();
        }
    }
}
