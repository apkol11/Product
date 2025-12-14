using Domain.EntityModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.EntityModel;


namespace Business.Interfcae.Repository
{
    public interface IProductRepository
    {
        public Task<int> AddProduct(ProductModel product);

        public Task<int> UpdateProduct(ProductModel product);

        public Task<int> DeleteProduct(int productId);

        public Task<ProductModel> GetProductById(int productId);

        public Task<IEnumerable<ProductModel>> GetAllProducts();
    }
}
