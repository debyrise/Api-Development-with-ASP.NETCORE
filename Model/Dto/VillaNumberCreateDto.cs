using System.ComponentModel.DataAnnotations;

namespace WebApiDemo.Model.Dto
{
    public class VillaNumberCreateDto
    {
        [Required]
        public int VillaNo { get; set; }
        [Required]
        public int VillaID { get; set; }

        public string SpecilDetail { get; set; }
    }
}
