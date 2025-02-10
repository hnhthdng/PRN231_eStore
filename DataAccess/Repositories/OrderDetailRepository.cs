using BusinessObject.Models;
using DataAccess.DAO;
using DataAccess.Repositories.IRepositories;

namespace DataAccess.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly OrdersDetailDAO _orderDetailDAO;

        public OrderDetailRepository(OrdersDetailDAO orderDetailDAO)
        {
            _orderDetailDAO = orderDetailDAO;
        }

        public void AddOrderDetail(OrderDetail orderDetail) => _orderDetailDAO.AddOrderDetail(orderDetail);

        public OrderDetail GetOrderDetailByOrderIdAndProductId(int orderId, int productId)
            => _orderDetailDAO.FindOrderDetailByOrderIdAndProductID(orderId, productId);

        public List<OrderDetail> GetOrderDetails() => _orderDetailDAO.GetOrderDetails();

        public List<OrderDetail> GetOrderDetailsByOrderId(int orderId) => _orderDetailDAO.FindAllOrderDetailsByOrderId(orderId);

        public void UpdateOrderDetail(OrderDetail orderDetail) => _orderDetailDAO.UpdateOrderDetail(orderDetail);

        public void DeleteOrderDetail(OrderDetail orderDetail) => _orderDetailDAO.DeleteOrderDetail(orderDetail);
    }
}
