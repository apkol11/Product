using Business.Interfaces.Handler;
using Business.Interfaces.Repository;
using Domain.EntityModel;
using Domain.Request;
using Domain.Response;

namespace Business.Handlers
{
    /// <summary>
    /// Handles business logic operations related to products.
    /// </summary>
    public class ProductHandler : IProductHandler
    {
        private readonly IProductRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductHandler"/> class.
        /// </summary>
        /// <param name="repository">The product repository instance.</param>
        public ProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Adds a new product with associated colours to the system.
        /// </summary>
        /// <param name="request">The product creation request containing product details and colour associations.</param>
        /// <returns>The identifier of the newly created product.</returns>
        public async Task<int> AddProduct(ProductRequest request)
        {
            // Basic null check (data annotations handle the rest)
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            // Map request to entity with proper defaults
            var product = new Product
            {
                Name = request.Name,
                ProductTypeId = request.ProductTypeId,
                CreatedBy = "User",
                UpdatedBy = "User",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                DeletedDate = null
            };

            // Persist product and return generated identifier
            return await _repository.AddProduct(product, request.ColourIds);
        }

        /// <summary>
        /// Retrieves all products from the system.
        /// </summary>
        /// <returns>A collection of product list response objects.</returns>
        public async Task<IEnumerable<ProductListResponse>> GetAllProducts()
        {
            var products = await _repository.GetAllProducts();

            // Map entities to response DTOs
            return products.Select(p => new ProductListResponse
            {
                Id = p.ProductId,
                Name = p.Name
            });
        }

        /// <summary>
        /// Retrieves detailed information about a specific product.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>
        /// A <see cref="ProductDetailResponse"/> containing product details if found; otherwise, null.
        /// </returns>
        public async Task<ProductDetailResponse> GetProductById(int id)
        {
            var product = await _repository.GetProductById(id);

            // Return null if product not found
            if (product == null) return null;

            // Map entity to detailed response DTO
            return new ProductDetailResponse
            {
                Id = product.ProductId,
                Name = product.Name,
                ProductType = product.ProductType?.ProductTypeName,
                Colours = product.ProductColours?.Select(pc => pc.Colour?.ColourName).ToList() ?? new List<string>()
            };
        }
    }
}


