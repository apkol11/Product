using Business.Interfaces.Handler;
using Business.Interfaces.Repository;
using Domain.Request;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Handlers
{
    /// <summary>
    /// Handles business logic operations related to product types.
    /// </summary>
    public class ProductTypeHandler : IProductTypeHandler
    {
        private readonly IProductTypeRepository _productTypeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductTypeHandler"/> class.
        /// </summary>
        /// <param name="repository">The product type repository instance.</param>
        public ProductTypeHandler(IProductTypeRepository repository)
        {
            _productTypeRepository = repository;
        }

        /// <summary>
        /// Adds a new product type to the system.
        /// </summary>
        /// <param name="productType">The product type creation request.</param>
        /// <returns>The identifier of the newly created product type.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the product type name is missing or invalid.</exception>
        public async Task<int> AddProductType(ProductTypeRequest productType)
        {
            // Validate request object
            if (productType is null)
                throw new ArgumentNullException(nameof(productType));

            // Validate product type name
            if (string.IsNullOrWhiteSpace(productType.ProductTypeName))
                throw new ArgumentException("Product type name is required.", nameof(productType.ProductTypeName));

            // Map request to entity
            var entity = new Domain.EntityModel.ProductType
            {
                ProductTypeName = productType.ProductTypeName
            };

            // Persist product type and return generated identifier
            return await _productTypeRepository.AddProductType(entity);
        }

        /// <summary>
        /// Retrieves all product types from the system.
        /// </summary>
        /// <returns>A collection of product type entities.</returns>
        public Task<IEnumerable<Domain.EntityModel.ProductType>> GetAllProductTypes()
        {
            return _productTypeRepository.GetAllProductTypes();
        }
    }
}