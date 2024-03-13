using AutoMapper;
using Ecom.Backend.Core.Dtos;
using Ecom.Backend.Core.Entities.Orders;

namespace Ecom.Backend.API.Helper
{
    public class OrderItemResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.ProductItemsOrdered.ProductItemPicture)) 
            {
                return _configuration["APIURL"] + source.ProductItemsOrdered.ProductItemPicture;
            }
            return null;
        }
    }
}
