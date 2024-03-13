using AutoMapper;
using Ecom.Backend.Core.Dtos;
using Ecom.Backend.Core.Entities;

namespace Ecom.Backend.API.Mapping
{
    public class BasketMapping :Profile
    {
        public BasketMapping()
        {
            CreateMap<BasketItem,BasketItemsDto>().ReverseMap();
            CreateMap<CustomerBasket,CustomerBasketDto>().ReverseMap();
        }
    }
}
