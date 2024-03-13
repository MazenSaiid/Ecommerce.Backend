using AutoMapper;
using Ecom.Backend.API.Helper;
using Ecom.Backend.Core.Dtos;
using Ecom.Backend.Core.Entities.Orders;

namespace Ecom.Backend.API.Mapping
{
    public class OrderMapping: Profile
    {
        public OrderMapping() 
        {
            CreateMap<Order,OrderResultDto>()
                .ForMember(d=>d.DeliveryMethod,o=>o.MapFrom(s=>s.DeliveryMethod.Name))
                .ForMember(d=>d.ShippingPrice, o=>o.MapFrom(s=>s.DeliveryMethod.Price))
                .ReverseMap();
            CreateMap<OrderItem,OrderItemDto>()
                .ForMember(d=>d.ProductItemId,o=>o.MapFrom(s=>s.Id))
                .ForMember(d => d.ProductItemName, o => o.MapFrom(s => s.ProductItemsOrdered.ProductItemName))
                .ForMember(d => d.ProductItemPicture, o => o.MapFrom(s => s.ProductItemsOrdered.ProductItemPicture))
                .ForMember(d => d.ProductItemPicture, o => o.MapFrom<OrderItemResolver>())
                .ReverseMap();
            CreateMap<ShippingAddress, AddressDto>()
                .ReverseMap();
        }
    }
}
