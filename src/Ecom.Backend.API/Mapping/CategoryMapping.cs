using AutoMapper;
using Ecom.Backend.Core.Dtos;
using Ecom.Backend.Core.Entities;

namespace Ecom.Backend.API.Mapping
{
    public class CategoryMapping: Profile
    {
        public CategoryMapping()
        {
           
           CreateMap<CategoryDto,Category>().ReverseMap();
           CreateMap<Category,ListingCategoryDto>().ReverseMap();
           CreateMap<UpdateCategoryDto, Category>().ReverseMap();
        }
    }
}
