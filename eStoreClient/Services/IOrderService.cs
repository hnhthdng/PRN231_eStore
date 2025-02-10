using eStoreClient.DTO.Order;
using Refit;

namespace eStoreClient.Services
{
    public interface IOrderService
    {
        [Get("/api/order")]
        Task<List<OrderResponseDTO>> GetOrdersAsync();

        [Get("/api/order/{id}")]
        Task<OrderResponseDTO> GetOrderByIdAsync(int id);

        [Get("/api/order/member/{memberId}")]
        Task<List<OrderResponseDTO>> GetOrdersByMemberIdAsync(int memberId);

        [Post("/api/order")]
        Task<OrderResponseDTO> AddOrderAsync([Body] OrderRequestDTO orderRequest);

        [Put("/api/order/{id}")]
        Task UpdateOrderAsync(int id, [Body] OrderRequestDTO orderRequest);

        [Delete("/api/order/{id}")]
        Task DeleteOrderAsync(int id);

        [Get("/api/order/salesReport")]
        Task<object> GetSalesReportAsync([Query] DateTime startDate, [Query] DateTime endDate);
    }
}
