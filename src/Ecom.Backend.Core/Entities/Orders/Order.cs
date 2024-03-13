using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Backend.Core.Entities.Orders
{
    public class Order:BaseEntity<int>
    {
        public Order()
        {
            
        }
        public Order(string buyerEmail, ShippingAddress shippingAddress, DeliveryMethod deliveryMethod,
            IReadOnlyList<OrderItem> orderItems, decimal subTotal, string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        public string BuyerEmail { get; set; }
        public DateTime OrderDate { get; set; } =  DateTime.Now;
        public ShippingAddress ShippingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public decimal SubTotal { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending; //enum

        public string PaymentIntentId { get; set; }

        public decimal GetTotal()
        {
            return SubTotal + DeliveryMethod.Price;
        }
    }
}
