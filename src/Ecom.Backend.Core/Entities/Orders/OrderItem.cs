namespace Ecom.Backend.Core.Entities.Orders
{
    public class OrderItem :BaseEntity<int>
    {
        public OrderItem()
        {

        }
        public OrderItem(ProductItemsOrdered productItemsOrdered, decimal price, int quantity)
        {
            ProductItemsOrdered = productItemsOrdered;
            Price = price;
            Quantity = quantity;
        }

        public ProductItemsOrdered ProductItemsOrdered { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}