using Domain.Request;
using Domain.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces.Handler
{
    public interface IProductHandler
    {
        Task<int> AddProduct(ProductRequest request);
        Task<IEnumerable<ProductListResponse>> GetAllProducts();
        Task<ProductDetailResponse> GetProductById(int id);
    }
}

