using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.EntityModel
{
    public  class ProductType
    {
        [Key]
        public int ProductTypeId { get; set; }

        [Required]
        public string TypeName { get; set; }

        [Required]
        public string Description { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }

        public DateTime? DeletedDate { get; set; }
    }
}
