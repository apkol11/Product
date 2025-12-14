using AutoMapper;
using Business.Interfcae.Handler;
using Business.Interfcae.Repository;
using Domain.Request;
using Domain.EntityModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.EntityModel;

namespace Business.Handler
{
    public class ProductHandler : IProductHandler
    {
        private IProductRepository _productRepository;
        private IMapper _mapper;

        public ProductHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public Task<int> AddProduct(ProductRequest product)
        {
            var productModel = _mapper.Map<ProductModel>(product);
            return _productRepository.AddProduct(productModel);
        }
    }
}
