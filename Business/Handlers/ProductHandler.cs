using Business.Interfaces.Handler;
using Business.Interfaces.Repository;
using Domain.EntityModel;
using Domain.Exceptions;
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
        private readonly IColourRepository _colourRepository;
        private readonly IProductTypeRepository _productTypeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductHandler"/> class.
        /// </summary>
        public ProductHandler(
            IProductRepository repository,
            IColourRepository colourRepository,
            IProductTypeRepository productTypeRepository)
        {
            _repository = repository;
            _colourRepository = colourRepository;
            _productTypeRepository = productTypeRepository;
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

            // Validate colour IDs exist
            if (request.ColourIds == null || !request.ColourIds.Any())
            {
                throw new BadRequestException("At least one colour must be selected.");
            }

            // Check for invalid colour IDs (0 or negative)
            if (request.ColourIds.Any(id => id <= 0))
            {
                throw new BadRequestException("Invalid colour ID. Colour IDs must be positive numbers.");
            }

            // Verify all colours exist
            var allColours = await _colourRepository.GetAllColours();
            var existingColourIds = allColours.Select(c => c.ColourId).ToList();
            var invalidColourIds = request.ColourIds.Where(id => !existingColourIds.Contains(id)).ToList();

            if (invalidColourIds.Any())
            {
                throw new BadRequestException(
                    $"The following colour IDs do not exist: {string.Join(", ", invalidColourIds)}. " +
                    $"Please create these colours first or use existing colour IDs.");
            }

            // Verify product type exists
            var allProductTypes = await _productTypeRepository.GetAllProductTypes();
            if (!allProductTypes.Any(pt => pt.ProductTypeId == request.ProductTypeId))
            {
                throw new BadRequestException(
                    $"Product type with ID '{request.ProductTypeId}' does not exist. " +
                    $"Please create the product type first or use an existing product type ID.");
            }

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


