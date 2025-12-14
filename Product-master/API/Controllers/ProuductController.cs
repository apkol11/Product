using Business.Interfcae.Handler;
using Domain.Request;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductHandler _productHandler;

        public ProductController(IProductHandler productHandler)
        {
            _productHandler = productHandler;
        }

        [HttpPost]
        public async Task<int> Create(ProductRequest productRequest)
        {
           return await  _productHandler.AddProduct(productRequest);
        }
    }
}
