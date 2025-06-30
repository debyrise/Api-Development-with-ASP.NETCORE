using System.ComponentModel.DataAnnotations;

namespace WebApiDemo.Model.Dto
{
    public class VillaUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public string Details { get; set; }
        [Required]
        public int Rate { get; set; }
        public string ImageUrl { get; set; }
        
        public string Amenity { get; set; }
        [Required]

        public int Occupancy { get; set; }
        [Required]

        public int sqft { get; set; }
    }
}
