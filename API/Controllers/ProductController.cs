using Business.Interfaces.Handler;
using Domain.Request;
using Domain.Response;
using Microsoft.AspNetCore.Mvc;

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
                Location = Url.Action(nameof(GetById), new { id })
            };

            return CreatedAtAction(nameof(GetById), new { id }, response);
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
        /// <param name="id">The unique identifier of the product. Must be a positive integer.</param>
        /// <returns>Detailed product information including type and colours.</returns>
        /// <response code="200">Returns the product details.</response>
        /// <response code="400">Invalid product ID.</response>
        /// <response code="404">Product not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDetailResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        public async Task<ActionResult<ProductDetailResponse>> GetById([FromRoute] string id)
        {
            // Validate if id is a valid integer
            if (!int.TryParse(id, out int productId))
            {
                return BadRequest(new ErrorResponse
                {
                    Message = $"Invalid product ID format. '{id}' is not a valid number. Please provide a valid positive integer.",
                    StatusCode = 400,
                    TraceId = HttpContext.TraceIdentifier,
                    Timestamp = DateTime.UtcNow
                });
            }

            // Validate if id is positive
            if (productId <= 0)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = $"Invalid product ID. Product ID must be a positive number greater than 0. Received: {productId}",
                    StatusCode = 400,
                    TraceId = HttpContext.TraceIdentifier,
                    Timestamp = DateTime.UtcNow
                });
            }

            var product = await _handler.GetProductById(productId);

            if (product == null)
            {
                return NotFound(new ErrorResponse
                {
                    Message = $"Product with ID {productId} was not found.",
                    StatusCode = 404,
                    TraceId = HttpContext.TraceIdentifier,
                    Timestamp = DateTime.UtcNow
                });
            }

            return Ok(product);
        }
    }
}
