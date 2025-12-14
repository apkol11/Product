using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Response
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductType { get; set; }
        public List<string> Colours { get; set; }
    }
}
