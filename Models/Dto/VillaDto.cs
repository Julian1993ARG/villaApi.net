using System.ComponentModel.DataAnnotations;

namespace MagicVilla.Models.Dto
{
    public class VillaDto
    {
        public VillaDto(int Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }
        public int Id { get; set; }
        [Required]
        [MaxLength(100), MinLength(5)]
        public string Name { get; set; }
    }
}
