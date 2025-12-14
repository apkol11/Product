using AutoMapper;
using Domain.EntityModel;
using Domain.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ProductRequest, ProductModel>();
                CreateMap<ProductModel, ProductRequest>();
            }
        }
    
}
