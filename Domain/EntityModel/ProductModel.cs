using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.EntityModel
{
    public class ProductModel
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
         [Required]
        [ForeignKey(nameof(ProductType))]
        public int? ProductTypeId { get; set; }
        [ForeignKey(nameof(Colour))] public int? ColourID { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

       public virtual ProductType ProductType { get; set; }
       public virtual Colour Colour { get; set; }  

    }
}
