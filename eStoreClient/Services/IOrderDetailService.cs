using eStoreClient.DTO.OrderDetail;
using Refit;

namespace eStoreClient.Services
{
    public interface IOrderDetailService
    {
        // Get all order details
        [Get("/api/orderdetail")]
        Task<List<OrderDetailResponseDTO>> GetAllOrderDetailsAsync();

        // Get order details by order ID
        [Get("/api/orderdetail/{orderId}")]
        Task<List<OrderDetailResponseDTO>> GetOrderDetailsByOrderIdAsync(int orderId);

        // Get order detail by order ID and product ID
        [Get("/api/orderdetail/{orderId}/{productId}")]
        Task<OrderDetailResponseDTO> GetOrderDetailByOrderIdAndProductIdAsync(int orderId, int productId);

        // Create a new order detail
        [Post("/api/orderdetail")]
        Task CreateOrderDetailAsync([Body] OrderDetailRequestDTO orderDetailRequest);

        // Update an existing order detail
        [Put("/api/orderdetail")]
        Task UpdateOrderDetailAsync([Body] OrderDetailRequestDTO orderDetailRequest);

        // Delete an order detail
        [Delete("/api/orderdetail")]
        Task DeleteOrderDetailAsync([Body] OrderDetailRequestDTO orderDetailRequest);
    }
}
