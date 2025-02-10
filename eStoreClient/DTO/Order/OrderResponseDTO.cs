namespace eStoreClient.DTO.Order
{
    public class OrderResponseDTO
    {
        public int OrderId { get; set; }
        public int MemberId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public string Freight { get; set; }
    }

}
