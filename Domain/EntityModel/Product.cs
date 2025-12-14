using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.EntityModel
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int ProductTypeId { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        // Navigation properties
        public ProductType ProductType { get; set; }
        public ICollection<ProductColour> ProductColours { get; set; } = new List<ProductColour>();
    }
}
