using eStoreClient.DTO.Order;
using eStoreClient.DTO.OrderDetail;
using eStoreClient.DTO.Product;
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
        private readonly IOrderDetailService _orderDetailService;
        private readonly IProductService _productService;
        public OrderController(IOrderService orderService, IMemberService memberService, IOrderDetailService orderDetailService, IProductService productService)
        {
            _orderService = orderService;
            _memberService = memberService;
            _orderDetailService = orderDetailService;
            _productService = productService;
        }
        public IActionResult Index(DateTime? startDate, DateTime? endDate)
        {
            var orders = _orderService.GetOrdersAsync().Result;
            List<OrderViewModel> orderViewModels = new List<OrderViewModel>();

            foreach (var order in orders)
            {
                var member = _memberService.GetMemberByIdAsync(order.MemberId).Result;
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
                var orderViewModel = new OrderViewModel
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    Email = member.Email,
                    RequiredDate = order.RequiredDate,
                    ShippedDate = order.ShippedDate,
                    Freight = order.Freight,
                    Total = totalAfterDiscount,
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
            orderViewModels = orderViewModels.OrderBy(o => o.Total).ToList();

            return View(orderViewModels);
        }

        //AddProduct
        public async Task<IActionResult> AddProductAsync(int id)
        {
            var products = await _productService.GetAllProductsAsync();
            ViewBag.Products = products.Select(c => new SelectListItem
            {
                Value = c.ProductId.ToString(),
                Text = c.ProductName
            }).ToList();

            var model = new OrderDetailViewModel
            {
                OrderId = id // Gán OrderId từ tham số id
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(OrderDetailViewModel model)
        {
            var products = await _productService.GetAllProductsAsync();
            ViewBag.Products = products.Select(c => new SelectListItem
            {
                Value = c.ProductId.ToString(),
                Text = c.ProductName
            }).ToList();

            var product = await _productService.GetProductByIdAsync(model.ProductId);

            if (product == null)
            {
                ModelState.AddModelError("ProductId", "Product not found");
            }
            else
            {
                if (model.Quantity <= 0)
                {
                    ModelState.AddModelError("Quantity", "Quantity must be greater than 0");
                }
                else if (product.UnitsInStock < model.Quantity)
                {
                    ModelState.AddModelError("Quantity", "Not enough stock");
                }

                if (model.Discount < 0 || model.Discount > 100)
                {
                    ModelState.AddModelError("Discount", "Discount must be between 0 and 100");
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
                return View(model);
            }

            // Nếu ModelState hợp lệ, tạo order detail và chuyển hướng
            var orderDetailRequest = new OrderDetailRequestDTO
            {
                OrderId = model.OrderId,
                ProductId = model.ProductId,
                UnitPrice = product.UnitPrice,
                Quantity = model.Quantity,
                Discount = model.Discount
            };

            await _orderDetailService.CreateOrderDetailAsync(orderDetailRequest);
            
            product.UnitsInStock -= model.Quantity;
            var productDTO = new ProductRequestDTO
            {
                ProductName = product.ProductName,
                UnitsInStock = product.UnitsInStock,
                CategoryId = product.CategoryId,
                UnitPrice = product.UnitPrice,
                Weight = product.Weight
            };
            _productService.UpdateProductAsync(product.ProductId, productDTO);

            return RedirectToAction("Index");
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
                var orderViewModel = new OrderViewModel
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    Email = member.Email,
                    RequiredDate = order.RequiredDate,
                    ShippedDate = order.ShippedDate,
                    Freight = order.Freight,
                    Total = totalAfterDiscount
                };
                orderViewModels.Add(orderViewModel);
            }

            // Sort orderViewModels by Total in ascending order
            orderViewModels = orderViewModels.OrderBy(o => o.Total).ToList();

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
                worksheet.Cells[1, 7].Value = "Total";

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
                    worksheet.Cells[row, 7].Value = order.Total;
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
