using Domain.EntityModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces.Repository
{
    public interface IProductRepository
    {
        Task<int> AddProduct(Product product, List<int> colourIds);
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
    }
}
