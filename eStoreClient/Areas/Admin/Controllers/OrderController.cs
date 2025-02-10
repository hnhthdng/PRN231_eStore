using eStoreClient.DTO.Order;
using eStoreClient.Models;
using eStoreClient.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;

namespace eStoreClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IMemberService _memberService;
        public OrderController(IOrderService orderService, IMemberService memberService)
        {
            _orderService = orderService;
            _memberService = memberService;
        }
        public IActionResult Index(DateTime? startDate, DateTime? endDate)
        {
            var orders = _orderService.GetOrdersAsync().Result;
            List<OrderViewModel> orderViewModels = new List<OrderViewModel>();

            foreach (var order in orders)
            {
                var member = _memberService.GetMemberByIdAsync(order.MemberId).Result;
                var orderViewModel = new OrderViewModel
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    Email = member.Email,
                    RequiredDate = order.RequiredDate,
                    ShippedDate = order.ShippedDate,
                    Freight = order.Freight
                };
                orderViewModels.Add(orderViewModel);
            }

            // Filter orders by date range
            if (startDate.HasValue)
            {
                orderViewModels = orderViewModels.Where(o => o.OrderDate >= startDate.Value).ToList();
            }

            if (endDate.HasValue)
            {
                orderViewModels = orderViewModels.Where(o => o.OrderDate <= endDate.Value).ToList();
            }

            // Sort the orders by OrderId ascending
            orderViewModels = orderViewModels.OrderBy(o => o.OrderId).ToList();

            return View(orderViewModels);
        }

        public IActionResult GenerateExcel()
        {
            // Set the LicenseContext to NonCommercial or Commercial depending on your use
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var orders = _orderService.GetOrdersAsync().Result;
            List<OrderViewModel> orderViewModels = new List<OrderViewModel>();

            foreach (var order in orders)
            {
                var member = _memberService.GetMemberByIdAsync(order.MemberId).Result;
                var orderViewModel = new OrderViewModel
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    Email = member.Email,
                    RequiredDate = order.RequiredDate,
                    ShippedDate = order.ShippedDate,
                    Freight = order.Freight
                };
                orderViewModels.Add(orderViewModel);
            }

            // Generate Excel using EPPlus
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Orders");

                // Add headers to the first row
                worksheet.Cells[1, 1].Value = "Order ID";
                worksheet.Cells[1, 2].Value = "Order Date";
                worksheet.Cells[1, 3].Value = "Email";
                worksheet.Cells[1, 4].Value = "Required Date";
                worksheet.Cells[1, 5].Value = "Shipped Date";
                worksheet.Cells[1, 6].Value = "Freight";

                // Add order data to the worksheet
                int row = 2;
                foreach (var order in orderViewModels)
                {
                    worksheet.Cells[row, 1].Value = order.OrderId;
                    worksheet.Cells[row, 2].Value = order.OrderDate.ToString("yyyy-MM-dd");
                    worksheet.Cells[row, 3].Value = order.Email;
                    worksheet.Cells[row, 4].Value = order.RequiredDate.ToString("yyyy-MM-dd");
                    worksheet.Cells[row, 5].Value = order.ShippedDate.ToString("yyyy-MM-dd");
                    worksheet.Cells[row, 6].Value = order.Freight;
                    row++;
                }

                // Adjust column widths to fit content
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Convert to byte array for download
                var fileStream = new MemoryStream(package.GetAsByteArray());

                // Return Excel file as response
                return File(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Orders.xlsx");
            }
        }


        //Create Order for member
        public async Task<IActionResult> CreateAsync()
        {
            var order = new OrderRequestDTO();
            var members = await _memberService.GetAllMembersAsync();
            ViewBag.Members = members.Select(m => new SelectListItem
            {
                Value = m.MemberId.ToString(),
                Text = m.Email
            }).ToList();
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(OrderRequestDTO orderRequest)
        {
            if (ModelState.IsValid)
            {
                await _orderService.AddOrderAsync(orderRequest);
                return RedirectToAction(nameof(Index));
            }
            return View(orderRequest);
        }

        //Delete
        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
