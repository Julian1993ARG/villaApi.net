using AutoMapper;
using MagicVilla.Models;
using MagicVilla.Models.Dto;

namespace MagicVilla
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDto>().ReverseMap();
            CreateMap<Villa, VillaUpdateDto>().ReverseMap();
            CreateMap<Villa, VillaCreateDto>().ReverseMap();

            CreateMap<VillaNumber, VillaNumberDto>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberUpdateDto>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberCreateDto>().ReverseMap();
        }
    }
}
