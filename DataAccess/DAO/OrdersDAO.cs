using BusinessObject.Models;
using DataAccess.Data;
using DataAccess.DTO.Order;

namespace DataAccess.DAO
{
    public class OrdersDAO
    {
        private readonly StoreDbContext _context;

        // Constructor nhận StoreDbContext từ DI
        public OrdersDAO(StoreDbContext context)
        {
            _context = context;
        }

        public List<Order> GetOrders()
        {
            var listOrders = new List<Order>();
            try
            {
                listOrders = _context.Orders.ToList();
                listOrders.ForEach(o => o.Member = _context.Members.Find(o.MemberId));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listOrders;
        }

        public List<Order> FindAllOrdersByMemberId(int memberId)
        {
            var listOrders = new List<Order>();
            try
            {
                listOrders = _context.Orders.Where(o => o.MemberId == memberId).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listOrders;
        }

        public Order FindOrderById(int orderId)
        {
            Order order;
            try
            {
                order = _context.Orders.SingleOrDefault(o => o.OrderId == orderId);
                if (order != null)
                    order.Member = _context.Members.Find(order.MemberId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return order;
        }

        public Order AddOrder(Order order)
        {
            try
            {
                _context.Orders.Add(order);
                _context.SaveChanges();
                return order;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateOrder(Order order)
        {
            try
            {
                var orderToUpdate = _context.Orders.SingleOrDefault(o => o.OrderId == order.OrderId);
                if (orderToUpdate != null)
                {
                    orderToUpdate.MemberId = order.MemberId;
                    orderToUpdate.OrderDate = order.OrderDate;
                    orderToUpdate.RequiredDate = order.RequiredDate;
                    orderToUpdate.ShippedDate = order.ShippedDate;
                    orderToUpdate.Freight = order.Freight;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteOrder(Order order)
        {
            try
            {
                var orderToDelete = _context.Orders.SingleOrDefault(o => o.OrderId == order.OrderId);
                if (orderToDelete != null)
                {
                    _context.Orders.Remove(orderToDelete);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<SalesReport> GetSalesReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                // Query orders and related details within the specified period
                var salesReport = _context.Orders
                    .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                    .SelectMany(o => o.OrderDetails, (order, orderDetail) => new
                    {
                        Order = order,
                        OrderDetail = orderDetail
                    })
                    .GroupBy(
                        o => new { o.OrderDetail.ProductId, o.OrderDetail.Product.ProductName },
                        (key, group) => new SalesReport
                        {
                            ProductId = key.ProductId,
                            ProductName = key.ProductName,
                            TotalSales = group.Sum(g => g.OrderDetail.Quantity * g.OrderDetail.UnitPrice),
                            TotalQuantity = group.Sum(g => g.OrderDetail.Quantity)
                        })
                    .OrderByDescending(s => s.TotalSales)
                    .ToList();

                return salesReport;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
