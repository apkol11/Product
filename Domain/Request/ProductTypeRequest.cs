using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Request
{
    public class ProductTypeRequest
    {
        public int ProductTypeId { get; set; }       
        public string ProductTypeName { get; set; } 
        
    }
}