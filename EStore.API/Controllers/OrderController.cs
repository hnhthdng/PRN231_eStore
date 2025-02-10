using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO.Order;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderController(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var orders = _orderRepository.GetOrders();
            var ordersResponse = _mapper.Map<List<OrderResponseDTO>>(orders);
            return Ok(ordersResponse);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var order = _orderRepository.GetOrderById(id);
            var orderResponse = _mapper.Map<OrderResponseDTO>(order);
            return Ok(orderResponse);
        }

        [HttpGet("member/{memberId}")]
        public IActionResult GetOrdersByMemberId(int memberId)
        {
            var orders = _orderRepository.GetAllOrdersByMemberId(memberId);
            var ordersResponse = _mapper.Map<List<OrderResponseDTO>>(orders);
            return Ok(ordersResponse);
        }

        [HttpPost]
        public IActionResult Post([FromBody] OrderRequestDTO orderRequest)
        {
            var order = _mapper.Map<Order>(orderRequest);
            var addedOrder = _orderRepository.AddOrder(order);
            var orderResponse = _mapper.Map<OrderResponseDTO>(addedOrder);
            return Ok(orderResponse);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] OrderRequestDTO orderRequest)
        {
            var order = _mapper.Map<Order>(orderRequest);
            order.OrderId = id;
            _orderRepository.UpdateOrder(order);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var order = _orderRepository.GetOrderById(id);
            _orderRepository.DeleteOrder(order);
            return Ok();
        }

        [HttpGet("salesReport")]
        public IActionResult GetSalesReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var salesReport = _orderRepository.GetSalesReport(startDate, endDate);
            return Ok(salesReport);
        }
    }
}
