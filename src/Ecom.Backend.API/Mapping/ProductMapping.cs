using AutoMapper;
using Ecom.Backend.API.Helper;
using Ecom.Backend.Core.Dtos;
using Ecom.Backend.Core.Entities;

namespace Ecom.Backend.API.Mapping
{
    public class ProductMapping: Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductDto>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
            .ForMember(p => p.ProductPicture, o => o.MapFrom<ProductURLResolver>())
                .ReverseMap();
                CreateMap<CreateProductDto,Product>().ReverseMap();
            CreateMap<UpdateProductDto,Product>().ReverseMap();
        }
    }
}
