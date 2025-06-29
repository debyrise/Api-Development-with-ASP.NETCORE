using System.ComponentModel.DataAnnotations;

namespace WebApiDemo.Model.Dto
{
    public class VillaCreateDto
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public string Details { get; set; }
        [Required]
        public int Rate { get; set; }
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
        public int Occupancy { get; set; }
        public int sqft { get; set; }
    }
}
