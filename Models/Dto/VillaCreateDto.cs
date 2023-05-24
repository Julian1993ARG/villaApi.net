using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MagicVilla.Models.Dto
{
    public class VillaCreateDto
    {
        [Required]
        [MaxLength(100), MinLength(5)]
        public string Name { get; set; }
        [Required]
        public string Details { get; set; }
        [Required]
        public double Rate { get; set; }
        [Required]
        public int Occupants { get; set; }
        public double SquareMeter { get; set; }
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
    }
}
