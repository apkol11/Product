using Business.Interfcae.Repository;
using Domain.EntityModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        public ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;

        }
        public Task<int> AddProduct(ProductModel product)
        {
            // Ensure required DB column values are populated to avoid SQLite NOT NULL constraint failures
            if (string.IsNullOrWhiteSpace(product.CreatedBy))
            {
                product.CreatedBy = "system"; // or set to current user if available
            }

            // Ensure UpdatedBy is also set to avoid NOT NULL constraint if DB requires it
            if (string.IsNullOrWhiteSpace(product.UpdatedBy))
            {
                product.UpdatedBy = product.CreatedBy ?? "system";
            }

            // Ensure CreatedDate is set
            if (product.CreatedDate == default)
            {
                product.CreatedDate = DateTime.UtcNow;
            }

            _dbContext.Add(product);
            _dbContext.SaveChanges();
            return Task.FromResult(product.ProductId);


        }

        public Task<int> DeleteProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProductModel>> GetAllProducts()
        {
            return await _dbContext.products.ToListAsync();
        }

        public Task<ProductModel> GetProductById(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateProduct(ProductModel product)
        {
            throw new NotImplementedException();
        }
    }
}
