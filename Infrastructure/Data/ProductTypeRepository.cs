using Business.Interfaces.Repository;
using Domain.EntityModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
   
    /// Provides data access operations for product type entities.
    
    public class ProductTypeRepository : IProductTypeRepository
    {
        private readonly ApplicationDbContext _context;

       
        /// Initializes a new instance of the <see cref="ProductTypeRepository"/> class.
        
        /// <param name="context">The database context instance.</param>
        public ProductTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

       
        /// Adds a new product type to the database.
        
        /// <param name="productType">The product type entity to add.</param>
        /// <returns>The identifier of the newly created product type.</returns>
        public async Task<int> AddProductType(ProductType productType)
        {
            _context.ProductTypes.Add(productType);
            await _context.SaveChangesAsync();
            return productType.ProductTypeId;
        }

       
        /// Retrieves all product types from the database.
        
        /// <returns>A collection of all product type entities.</returns>
        public async Task<IEnumerable<ProductType>> GetAllProductTypes()
        {
            return await _context.ProductTypes.ToListAsync();
        }
    }
}