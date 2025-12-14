using Domain.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Interfcae.Handler
{
    public interface IProductHandler
    {
       public   Task<int> AddProduct(ProductRequest product);
    }
}
