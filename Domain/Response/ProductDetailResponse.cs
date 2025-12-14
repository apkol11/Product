using System.Collections.Generic;

namespace Domain.Response
{
    public class ProductDetailResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductType { get; set; }
        public List<string> Colours { get; set; } = new List<string>();
    }
}
