using System.ComponentModel.DataAnnotations;

namespace Domain.EntityModel
{
    public class Colour
    {
        [Key]
        public int ColourId { get; set; }
        [Required]
        public string ColourName { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        //public virtual ICollection<ProductColour> ProductColours { get; set; } = new List<ProductColour>();

    }
}
