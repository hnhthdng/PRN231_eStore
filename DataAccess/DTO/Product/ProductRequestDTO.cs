namespace DataAccess.DTO.Product
{
    public class ProductRequestDTO
    {
        public string ProductName { get; set; }
        public int UnitPrice { get; set; }
        public int Weight { get; set; }
        public int UnitsInStock { get; set; }
        public int CategoryId { get; set; }
    }
}
