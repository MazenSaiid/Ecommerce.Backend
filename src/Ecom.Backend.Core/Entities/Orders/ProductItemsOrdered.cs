namespace Ecom.Backend.Core.Entities.Orders
{
    public class ProductItemsOrdered
    {
        public ProductItemsOrdered()
        {

        }
        public ProductItemsOrdered(int productItemId, string productItemName, string productItemPicture)
        {
            ProductItemId = productItemId;
            ProductItemName = productItemName;
            ProductItemPicture = productItemPicture;
        }

        public int ProductItemId { get; set; }
        public string ProductItemName { get; set; }
        public string ProductItemPicture { get; set;}
    }
}