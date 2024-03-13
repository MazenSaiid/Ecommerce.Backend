using Ecom.Backend.Core.Entities;
using Ecom.Backend.Core.Entities.Orders;
using Ecom.Backend.Core.Interfaces;
using Ecom.Backend.Core.Services;
using Ecom.Backend.InfraStructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Backend.InfraStructure.Repositories
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public PaymentServices(IUnitOfWork unitOfWork,
            IConfiguration configuration, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _context = context;
        }
        public async Task<CustomerBasket> CreateOrUpdatePayment(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:Secretkey"];
            var basket = await _unitOfWork.BasketRepository.GetCustomerBasket(basketId);
            var shippingPrice = 0m;

            if(basket is null)
                return  null;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _context.DeliveryMethods.Where(x => x.Id == basket.DeliveryMethodId.Value).
                    FirstOrDefaultAsync();
                shippingPrice = deliveryMethod.Price;
            }
            foreach(var item in basket.BasketItems)
            {
                var productItem = await _unitOfWork.ProductRepository.GetByIdAsync(item.Id);
                if(item.Price != productItem.Price)
                {
                    item.Price = productItem.Price;
                }
            }

            var service = new PaymentIntentService();
            PaymentIntent intent;
            if(string.IsNullOrEmpty(basket.PaymentIntentId)) 
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long?)basket.BasketItems.Sum(x => (x.Price * 100) * x.Quantity) + (long?)shippingPrice,
                    Currency = "USD",
                    PaymentMethodTypes = new List<string> {"card"}
                };
                intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;

            }
            else
            {
                //update
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long?)basket.BasketItems.Sum(x => (x.Price * 100) * x.Quantity) + (long?)shippingPrice
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);

            }
            await _unitOfWork.BasketRepository.UpdateCustomerBasketAsync(basket);
            return basket;

        }

        public async Task<Order> UpdateOrderPaymentFailedStatus(string paymentIntentId)
        {
            var order = await _context.Orders.Where(x=>x.PaymentIntentId == paymentIntentId)
                .FirstOrDefaultAsync();
            if (order is null)
                return null;
            order.OrderStatus = OrderStatus.PaymentFailed;
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateOrderPaymentSuccessStatus(string paymentIntentId)
        {
            var order = await _context.Orders.Where(x => x.PaymentIntentId == paymentIntentId)
                .FirstOrDefaultAsync();
            if (order is null)
                return null;
            order.OrderStatus = OrderStatus.PaymentReceived;
            await _context.SaveChangesAsync();
            return order;
        }
    }
}
