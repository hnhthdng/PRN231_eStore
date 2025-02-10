using eStoreClient.Models;
using eStoreClient.Services;
using Microsoft.AspNetCore.Mvc;

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
            return View(orders);
        }

        public IActionResult OrderDetail(int id)
        {
            var orderDetail = _orderDetailService.GetOrderDetailsByOrderIdAsync(id).Result;
            var orderDetailViewModel = new List<OrderDetailViewModel>();
            foreach (var item in orderDetail)
            {
                var product = _productService.GetProductByIdAsync(item.ProductId).Result;
                var orderDetailItem = new OrderDetailViewModel
                {
                    OrderId = item.OrderId,
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
