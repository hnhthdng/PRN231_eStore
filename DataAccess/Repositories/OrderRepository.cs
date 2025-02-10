using BusinessObject.Models;
using DataAccess.DAO;
using DataAccess.DTO.Order;
using DataAccess.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrdersDAO _ordersDAO;

        public OrderRepository(OrdersDAO ordersDAO)
        {
            _ordersDAO = ordersDAO;
        }

        public Order AddOrder(Order order) => _ordersDAO.AddOrder(order);

        public Order GetOrderById(int id) => _ordersDAO.FindOrderById(id);

        public List<Order> GetOrders() => _ordersDAO.GetOrders();

        public List<Order> GetAllOrdersByMemberId(int customerId) => _ordersDAO.FindAllOrdersByMemberId(customerId);

        public void UpdateOrder(Order order) => _ordersDAO.UpdateOrder(order);

        public void DeleteOrder(Order order) => _ordersDAO.DeleteOrder(order);

        public List<SalesReport> GetSalesReport(DateTime startDate, DateTime endDate)
        {
            return _ordersDAO.GetSalesReport(startDate, endDate);   
        }
    }
}
