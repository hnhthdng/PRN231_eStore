namespace eStoreClient.DTO.Order
{
    public class SalesReport
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalQuantity { get; set; }
    }
}
