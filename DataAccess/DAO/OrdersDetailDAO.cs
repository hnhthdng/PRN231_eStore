using BusinessObject.Models;
using DataAccess.Data;

namespace DataAccess.DAO
{
    public class OrdersDetailDAO
    {
        private readonly StoreDbContext _context;

        // Constructor nhận StoreDbContext từ DI
        public OrdersDetailDAO(StoreDbContext context)
        {
            _context = context;
        }

        public List<OrderDetail> GetOrderDetails()
        {
            var listOrderDetails = new List<OrderDetail>();
            try
            {
                listOrderDetails = _context.OrderDetails.ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listOrderDetails;
        }

        public List<OrderDetail> FindAllOrderDetailsByProductID(int productID)
        {
            var listOrderDetails = new List<OrderDetail>();
            try
            {
                listOrderDetails = _context.OrderDetails
                    .Where(o => o.ProductId == productID)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listOrderDetails;
        }

        public List<OrderDetail> FindAllOrderDetailsByOrderId(int orderId)
        {
            var listOrderDetails = new List<OrderDetail>();
            try
            {
                listOrderDetails = _context.OrderDetails
                    .Where(o => o.OrderId == orderId)
                    .ToList();
                listOrderDetails.ForEach(o =>
                    o.Product = _context.Products.SingleOrDefault(f => f.ProductId == o.ProductId)
                );
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listOrderDetails;
        }

        public OrderDetail FindOrderDetailByOrderIdAndProductID(int orderId, int ProductID)
        {
            OrderDetail orderDetail;
            try
            {
                orderDetail = _context.OrderDetails
                    .SingleOrDefault(o => o.OrderId == orderId && o.ProductId == ProductID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orderDetail;
        }

        public void AddOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                _context.OrderDetails.Add(orderDetail);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                var orderDetailToUpdate = _context.OrderDetails
                    .SingleOrDefault(o => o.OrderId == orderDetail.OrderId && o.ProductId == orderDetail.ProductId);
                if (orderDetailToUpdate != null)
                {
                    orderDetailToUpdate.Quantity = orderDetail.Quantity;
                    orderDetailToUpdate.UnitPrice = orderDetail.UnitPrice;
                    orderDetailToUpdate.Discount = orderDetail.Discount;
                    _context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                var orderDetailToDelete = _context.OrderDetails
                    .SingleOrDefault(o => o.OrderId == orderDetail.OrderId && o.ProductId == orderDetail.ProductId);
                if (orderDetailToDelete != null)
                {
                    _context.OrderDetails.Remove(orderDetailToDelete);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
