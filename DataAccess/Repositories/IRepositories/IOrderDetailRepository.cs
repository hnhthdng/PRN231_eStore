using BusinessObject.Models;

namespace DataAccess.Repositories.IRepositories
{
    public interface IOrderDetailRepository
    {
        void AddOrderDetail(OrderDetail orderDetail);
        OrderDetail GetOrderDetailByOrderIdAndProductId(int orderId, int productId);
        List<OrderDetail> GetOrderDetails();
        List<OrderDetail> GetOrderDetailsByOrderId(int orderId);
        void UpdateOrderDetail(OrderDetail orderDetail);
        void DeleteOrderDetail(OrderDetail orderDetail);
    }
}
