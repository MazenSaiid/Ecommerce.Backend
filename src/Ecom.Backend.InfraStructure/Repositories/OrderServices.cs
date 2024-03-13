using Ecom.Backend.Core.Entities.Orders;
using Ecom.Backend.Core.Interfaces;
using Ecom.Backend.Core.Services;
using Ecom.Backend.InfraStructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Backend.InfraStructure.Repositories
{
    public class OrderServices : IOrderServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
        private readonly IPaymentServices _paymentServices;

        public OrderServices(IUnitOfWork unitOfWork,ApplicationDbContext context,IPaymentServices paymentServices)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _paymentServices = paymentServices;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, ShippingAddress shippingAddress)
        {
            var basket = await _unitOfWork.BasketRepository.GetCustomerBasket(basketId);
            var orderItems = new List<OrderItem>();
            foreach (var item in basket.BasketItems)
            {
                var productItem = await _unitOfWork.ProductRepository.GetByIdAsync(item.Id);
                var productItemOrdered = new ProductItemsOrdered(productItem.Id,productItem.Name,productItem.ProductPicture);
                var orderItem = new OrderItem(productItemOrdered, item.Price, item.Quantity);
           
                orderItems.Add(orderItem);
                
            }
            await _context.OrderItems.AddRangeAsync(orderItems);
            await _context.SaveChangesAsync();

            var deliveryMethod =await _context.DeliveryMethods.Where(x => x.Id == deliveryMethodId)
                .FirstOrDefaultAsync();

            var subTotal = orderItems.Sum(x => x.Price * x.Quantity);

            var checkOrderExists = await _context.Orders.Where(x => x.PaymentIntentId == basket.PaymentIntentId)
                .FirstOrDefaultAsync();

            if(checkOrderExists is not null)
            {
                _context.Orders.Remove(checkOrderExists);
               await _paymentServices.CreateOrUpdatePayment(basket.PaymentIntentId);
            }

            var order = new Order(buyerEmail,shippingAddress,deliveryMethod,orderItems,subTotal,basket.PaymentIntentId);

            if (order is null)
                return null;

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            //await _unitOfWork.BasketRepository.DeleteCustomerBasketAsync(basketId);

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _context.DeliveryMethods.ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(string buyerEmail, int id)
        {
            return await _context.Orders.Where(x => x.Id == id && x.BuyerEmail == buyerEmail).
                Include(o=>o.OrderItems).Include(d=>d.DeliveryMethod).
                OrderByDescending(x=>x.OrderDate).
                FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            return await _context.Orders.Where(x => x.BuyerEmail == buyerEmail).
                Include(o => o.OrderItems).Include(d => d.DeliveryMethod).
                OrderByDescending(x => x.OrderDate).
                ToListAsync();
        }
    }
}
