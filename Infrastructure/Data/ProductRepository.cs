using Business.Interfaces.Repository;
using Domain.EntityModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddProduct(Product product, List<int> colourIds)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Add product-colour associations
            foreach (var colourId in colourIds)
            {
                _context.ProductColours.Add(new ProductColour
                {
                    ProductId = product.ProductId,
                    ColourId = colourId
                });
            }

            await _context.SaveChangesAsync();
            return product.ProductId;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products
                .Include(p => p.ProductType)
                .Include(p => p.ProductColours)
                    .ThenInclude(pc => pc.Colour)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }
    }
}
