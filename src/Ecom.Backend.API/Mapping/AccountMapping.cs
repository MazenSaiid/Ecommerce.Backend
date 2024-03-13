using AutoMapper;
using Ecom.Backend.Core.Dtos;
using Ecom.Backend.Core.Entities;
using Ecom.Backend.Core.Entities.Orders;

namespace Ecom.Backend.API.Mapping
{
    public class AccountMapping: Profile
    {
        public AccountMapping()
        {
            CreateMap<AddressDto, Address>().ReverseMap();
        }
    }
}
