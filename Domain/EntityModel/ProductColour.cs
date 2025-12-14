using System.ComponentModel.DataAnnotations;

namespace Domain.EntityModel
{
    public class ProductColour
    {
        [Key]
        public int ProductColourId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int ColourId { get; set; }

        // Navigation properties
        public Product Product { get; set; }
        public Colour Colour { get; set; }
    }
}
