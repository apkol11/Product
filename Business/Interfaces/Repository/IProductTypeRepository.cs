using Domain.EntityModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Interfaces.Repository
{
    public interface IProductTypeRepository
    {
        Task<int> AddProductType(ProductType productType);
        Task<IEnumerable<ProductType>> GetAllProductTypes();
    }
}