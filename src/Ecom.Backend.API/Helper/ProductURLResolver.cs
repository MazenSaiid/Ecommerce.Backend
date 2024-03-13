using AutoMapper;
using Ecom.Backend.Core.Dtos;
using Ecom.Backend.Core.Entities;

namespace Ecom.Backend.API.Helper
{
    public class ProductURLResolver : IValueResolver<Product, ProductDto, string>
    {
        private readonly IConfiguration _configuration;

        public ProductURLResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
           if(!string.IsNullOrEmpty(source.ProductPicture))
           {
                return _configuration["APIURL"]+source.ProductPicture;
           }
            return null;
        }
    }
}
