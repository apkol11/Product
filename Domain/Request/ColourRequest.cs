using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Request
{
    public class ColourRequest
    {
        public int ColourId { get; set; }        
        public string ColourName { get; set; }
    }
}
