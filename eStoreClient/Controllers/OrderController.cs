using eStoreClient.Models;
using eStoreClient.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace eStoreClient.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IMemberService _memberService;
        private readonly IProductService _productService;


        public OrderController(IOrderService orderService, IMemberService memberService, IOrderDetailService orderDetailService, IProductService productService)
        {
            _orderService = orderService;
            _memberService = memberService;
            _orderDetailService = orderDetailService;
            _productService = productService;
        }
        public IActionResult Index()
        {
            string memberEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(memberEmail))
            {
                return RedirectToAction("Login", "Account");
            }
            var member = _memberService.GetMemberByEmailAsync(memberEmail).Result;
            var orders = _orderService.GetOrdersByMemberIdAsync(member.MemberId).Result;
            var orderViewModel = new List<OrderViewModel>();
            foreach (var order in orders)
            {
                var orderDetails = _orderDetailService.GetOrderDetailsByOrderIdAsync(order.OrderId).Result;

                decimal totalAfterDiscount = 0;

                if (orderDetails != null && orderDetails.Any())
                {
                  
                    foreach (var orderDetail in orderDetails)
                    {
                        decimal totalBeforeDiscount = orderDetail.Quantity * orderDetail.UnitPrice;
                        decimal discount = orderDetail.Discount / 100m;
                        totalAfterDiscount += totalBeforeDiscount * (1 - discount);
                    }
                }

                var orderItem = new OrderViewModel
                {
                    OrderId = order.OrderId,
                    Email = memberEmail,
                    OrderDate = order.OrderDate,
                    RequiredDate = order.RequiredDate,
                    ShippedDate = order.ShippedDate,
                    Freight = order.Freight,
                    Total = totalAfterDiscount
                };
                orderViewModel.Add(orderItem);
            }
            return View(orderViewModel);
        }

        public IActionResult OrderDetail(int orderId)
        {
            var orderDetail = _orderDetailService.GetOrderDetailsByOrderIdAsync(orderId).Result;
            var orderDetailViewModel = new List<OrderDetailViewModel>();
            foreach (var item in orderDetail)
            {
                var product = _productService.GetProductByIdAsync(item.ProductId).Result;
                var orderDetailItem = new OrderDetailViewModel
                {
                    OrderId = item.OrderId,
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Discount = item.Discount
                };
                orderDetailViewModel.Add(orderDetailItem);
            }

            return View(orderDetailViewModel);
        }
    }
}
