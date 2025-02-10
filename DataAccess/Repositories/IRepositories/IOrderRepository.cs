using BusinessObject.Models;
using DataAccess.DTO.Order;

namespace DataAccess.Repositories.IRepositories
{
    public interface IOrderRepository
    {
        Order AddOrder(Order order);
        Order GetOrderById(int id);
        List<Order> GetOrders();
        List<Order> GetAllOrdersByMemberId(int customerId);
        void UpdateOrder(Order order);
        void DeleteOrder(Order order);
        List<SalesReport> GetSalesReport(DateTime startDate, DateTime endDate);
    }
}
