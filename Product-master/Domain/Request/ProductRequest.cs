using Domain.EntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Request
{
    public class ProductRequest
    {
      
        public int ProductId { get; set; }

      
        public string Name { get; set; }
        public decimal Price { get; set; }
        
        public int ProdcutTypeId { get; set; }

     
        public int ColourID { get; set; }
    }
}
