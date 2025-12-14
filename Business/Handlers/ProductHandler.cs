using Business.Interfaces.Handler;
using Business.Interfaces.Repository;
using Domain.EntityModel;
using Domain.Request;
using Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Handlers
{
    public class ProductHandler : IProductHandler
    {
        private readonly IProductRepository _repository;

        public ProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> AddProduct(ProductRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Product name is required.", nameof(request.Name));
            if (request.ProductTypeId <= 0)
                throw new ArgumentException("Valid ProductTypeId is required.", nameof(request.ProductTypeId));
            if (request.ColourIds == null || !request.ColourIds.Any())
                throw new ArgumentException("At least one colour is required.", nameof(request.ColourIds));

            var product = new Product
            {
                Name = request.Name,
                ProductTypeId = request.ProductTypeId,
                CreatedBy = request.CreatedBy
            };

            return await _repository.AddProduct(product, request.ColourIds);
        }

        public async Task<IEnumerable<ProductListResponse>> GetAllProducts()
        {
            var products = await _repository.GetAllProducts();
            return products.Select(p => new ProductListResponse
            {
                Id = p.ProductId,
                Name = p.Name
            });
        }

        public async Task<ProductDetailResponse> GetProductById(int id)
        {
            var product = await _repository.GetProductById(id);
            if (product == null) return null;

            return new ProductDetailResponse
            {
                Id = product.ProductId,
                Name = product.Name,
                ProductType = product.ProductType?.ProductTypeName,
                Colours = product.ProductColours?.Select(pc => pc.Colour?.ColourName).ToList() ?? new List<string>()
            };
        }
    }
}


