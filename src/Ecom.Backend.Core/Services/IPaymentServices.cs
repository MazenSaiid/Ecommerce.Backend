using Ecom.Backend.Core.Entities;
using Ecom.Backend.Core.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Backend.Core.Services
{
    public interface IPaymentServices
    {
        Task<CustomerBasket> CreateOrUpdatePayment(string basketId);
        Task<Order> UpdateOrderPaymentSuccessStatus(string paymentIntentId);
        Task<Order> UpdateOrderPaymentFailedStatus(string paymentIntentId);
    }
}
