using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace eStoreClient.Models
{
    public class OrderDetailViewModel
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        [ValidateNever]
        public string ProductName { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
    }
}
