namespace Ecom.Backend.Core.Entities.Orders
{
    public class DeliveryMethod : BaseEntity<int>
    {
        public DeliveryMethod()
        {
            
        }

        public DeliveryMethod(string name, string deliveryTime, string description, decimal price)
        {
            Name = name;
            DeliveryTime = deliveryTime;
            Description = description;
            Price = price;
        }

        public string Name { get; set; }
        public string DeliveryTime { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

    }
}