using Ecom.Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Backend.Core.Interfaces
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetCustomerBasket(string basketId);
        Task<CustomerBasket> UpdateCustomerBasketAsync(CustomerBasket customerBasket);
        Task<bool> DeleteCustomerBasketAsync(string basketId);
    }
}
