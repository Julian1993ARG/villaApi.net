using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla.Models
{
    public class Villa
    {
        public Villa()
        {
        }

        public Villa(string name, string details, int ocupants, double price, double squareMeter, string imageUrl, string amenity)
        {
            this.Name = name;
            this.Details = details;
            this.Occupants = ocupants;
            this.Rate = price;
            this.SquareMeter = squareMeter;
            this.ImageUrl = imageUrl;
            this.Amenity = amenity;
        }
        public Villa(int id,string name, string details, int ocupants, double price, double squareMeter, string imageUrl, string amenity)
        {
            this.Id = id;
            this.Name = name;
            this.Details = details;
            this.Occupants = ocupants;
            this.Rate = price;
            this.SquareMeter = squareMeter;
            this.ImageUrl = imageUrl;
            this.Amenity = amenity;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        [Required]
        public double Rate { get; set; }
        public int Occupants { get; set; }
        public double SquareMeter { get; set; }
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

    }
}
