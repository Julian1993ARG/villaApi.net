using System.ComponentModel.DataAnnotations;

namespace MagicVilla.Models.Dto
{
    public class DBVillaDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100), MinLength(5)]
        public string Name { get; set; }
        public string Details { get; set; }
        [Required]
        public double Rate { get; set; }
        public int Occupants { get; set; }
        public double SquareMeter { get; set; }
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
    }
}
