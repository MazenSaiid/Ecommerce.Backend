using Ecom.Backend.Core.Entities.Orders;

namespace Ecom.Backend.Core.Dtos
{
    public class OrderItemDto
    {
        public int ProductItemId { get; set; }
        public string ProductItemName { get; set; }
        public string ProductItemPicture { get; set;}
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}