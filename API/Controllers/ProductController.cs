using Business.Interfaces.Handler;
using Domain.Request;
using Domain.Response;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    /// API controller for managing product operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductHandler _handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="handler">The product handler instance.</param>
        public ProductController(IProductHandler handler)
        {
            _handler = handler;
        }

        /// <summary>
        /// Creates a new product with associated colours.
        /// </summary>
        /// <param name="request">The product creation request.</param>
        /// <returns>A standardized response containing the created product's identifier and location.</returns>
        /// <response code="201">Product created successfully.</response>
        /// <response code="400">Invalid request data.</response>
        [HttpPost]
        [ProducesResponseType(typeof(CreatedResponse), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] ProductRequest request)
        {
            var id = await _handler.AddProduct(request);
            
            var response = new CreatedResponse
            {
                Id = id,
                Message = "Product created successfully",
                Location = Url.Action(nameof(GetById), new { id = id })
            };

            return CreatedAtAction(nameof(GetById), new { id = id }, response);
        }

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>A collection of product summaries.</returns>
        /// <response code="200">Returns the list of products.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductListResponse>), 200)]
        public async Task<ActionResult<IEnumerable<ProductListResponse>>> GetAll()
        {
            var products = await _handler.GetAllProducts();
            return Ok(products);
        }

        /// <summary>
        /// Retrieves detailed information about a specific product.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>Detailed product information including type and colours.</returns>
        /// <response code="200">Returns the product details.</response>
        /// <response code="404">Product not found.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProductDetailResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProductDetailResponse>> GetById(int id)
        {
            var product = await _handler.GetProductById(id);
            if (product == null) return NotFound();
            return Ok(product);
        }
    }
}
