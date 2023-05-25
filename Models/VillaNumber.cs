using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla.Models
{
    public class VillaNumber
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VillaNo { get; set; }
        [Required]
        public int VillaId { get; set; }
        [ForeignKey("VillaId")]
        public Villa Villa { get; set; }
        [MaxLength(400)]
        public string SpecialDetails { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
