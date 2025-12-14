using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.EntityModel
{
    public class ProductModel
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }
        public decimal Price { get; set; }

        [Required]
        [ForeignKey(nameof(ProductType))]
        public int? ProdcutTypeId { get; set; }

        [Required]
        [ForeignKey(nameof(Colour))]
        public int? ColourID { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        public virtual ProductType ProductType { get; set; }
        public virtual Colour Colour { get; set; }
    }
}
