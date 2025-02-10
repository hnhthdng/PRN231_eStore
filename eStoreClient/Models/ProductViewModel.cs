namespace eStoreClient.Models
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int UnitPrice { get; set; }
        public int Weight { get; set; }
        public int UnitsInStock { get; set; }
        public string CategoryName { get; set; }
    }
}
