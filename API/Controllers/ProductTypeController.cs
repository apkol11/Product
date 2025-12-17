using Business.Interfaces.Handler;
using Domain.EntityModel;
using Domain.Request;
using Domain.Response;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// API controller for managing product type operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
        private readonly IProductTypeHandler _handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductTypeController"/> class.
        /// </summary>
        /// <param name="handler">The product type handler instance.</param>
        public ProductTypeController(IProductTypeHandler handler)
        {
            _handler = handler;
        }

        /// <summary>
        /// Creates a new product type.
        /// </summary>
        /// <param name="productTypeRequest">The product type creation request.</param>
        /// <returns>A standardized response containing the created product type's identifier.</returns>
        /// <response code="201">Product type created successfully.</response>
        /// <response code="400">Invalid request data.</response>
        [HttpPost]
        [ProducesResponseType(typeof(CreatedResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] ProductTypeRequest productTypeRequest)
        {
            var id = await _handler.AddProductType(productTypeRequest);
            
            var response = new CreatedResponse
            {
                Id = id,
                Message = "Product type created successfully",
                Location = Url.Action(nameof(Get), new { id = id })
            };

            return CreatedAtAction(nameof(Get), new { id = id }, response);
        }

        /// <summary>
        /// Retrieves all product types.
        /// </summary>
        /// <returns>A collection of all product types.</returns>
        /// <response code="200">Returns the list of product types.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductType>), 200)]
        public async Task<ActionResult<IEnumerable<ProductType>>> Get()
        {
            var productTypes = await _handler.GetAllProductTypes();
            return Ok(productTypes);
        }
    }
}