using Business.Interfaces.Repository;
using Domain.EntityModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    /// <summary>
    /// Provides data access operations for product type entities.
    /// </summary>
    public class ProductTypeRepository : IProductTypeRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductTypeRepository"/> class.
        /// </summary>
        /// <param name="context">The database context instance.</param>
        public ProductTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new product type to the database.
        /// </summary>
        /// <param name="productType">The product type entity to add.</param>
        /// <returns>The identifier of the newly created product type.</returns>
        public async Task<int> AddProductType(ProductType productType)
        {
            _context.ProductTypes.Add(productType);
            await _context.SaveChangesAsync();
            return productType.ProductTypeId;
        }

        /// <summary>
        /// Retrieves all product types from the database.
        /// </summary>
        /// <returns>A collection of all product type entities.</returns>
        public async Task<IEnumerable<ProductType>> GetAllProductTypes()
        {
            return await _context.ProductTypes.ToListAsync();
        }
    }
}