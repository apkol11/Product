using Business.Interfaces.Handler;
using Domain.EntityModel;
using Domain.Request;
using Domain.Response;
using Domain.Response;
using Domain.Response;
    using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
        private readonly IProductTypeHandler _handler;

        public ProductTypeController(IProductTypeHandler handler)
        {
            _handler = handler;
        }

        [HttpPost]
        public async Task<int> Create([FromBody] ProductTypeRequest productyperequest)
        {
            return await _handler.AddProductType(productyperequest);
        }

        [HttpGet]
        public async Task<IEnumerable<ProductType>> Get()
        {
            return await _handler.GetAllProductTypes();
        }
    }
}