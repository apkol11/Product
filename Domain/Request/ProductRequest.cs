using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;    
using System.Text;

namespace Domain.Request
{
        public class ProductRequest
    {
        public string Name { get; set; }
       public int ProductTypeId { get; set; }
        public List<int> ColourIds { get; set; } = new List<int>();
        public string? CreatedBy { get; set; }
    }

}
