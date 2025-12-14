using Business.Interfaces.Handler;
using Domain.Request;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductHandler _productHandler;

        public ProductController(IProductHandler productHandler)
        {
            _productHandler = productHandler;
        }

        [HttpPost]
        public async Task<int> Create([FromBody] ProductRequest productRequest)
        {
            return await _productHandler.AddProduct(productRequest);
        }
    }
}
