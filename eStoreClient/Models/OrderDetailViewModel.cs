namespace eStoreClient.Models
{
    public class OrderDetailViewModel
    {
        public int OrderId { get; set; }
        public string ProductName { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
    }
}
