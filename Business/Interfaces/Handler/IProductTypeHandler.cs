using Domain.Request;

namespace Business.Interfaces.Handler
{
    public interface IProductTypeHandler
    {
        public Task<int> AddProductType(ProductTypeRequest request);
        //Task<IEnumerable<ProductType>> GetAllProductTypes();
        public Task<IEnumerable<Domain.EntityModel.ProductType>> GetAllProductTypes();
    }
}
