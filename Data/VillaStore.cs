using MagicVilla.Models.Dto;

namespace MagicVilla.Data
{
    public static class VillaStore
    {
        public static List<VillaDto> VillaList = new List<VillaDto>
        {
            new VillaDto(1, "San Fernando"),
            new VillaDto(2, "San Francisco"),
            new VillaDto(3, "San Miguel"),
            new VillaDto(4, "San Pedro"),
            new VillaDto(5, "San Rafael"),
            new VillaDto(6, "San Sebastian"),
        };

        public static VillaDto? GetVillaById(int id)
        {
            return VillaList.FirstOrDefault(v => v.Id == id);
        }

        public static VillaDto? GetVillaByName(string name)
        {
            return VillaList.FirstOrDefault(v => v.Name.ToLower() == name.ToLower());
        }
    }
}
