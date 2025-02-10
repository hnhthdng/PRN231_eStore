namespace eStoreClient.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string Email { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public string Freight { get; set; }
    }
}
