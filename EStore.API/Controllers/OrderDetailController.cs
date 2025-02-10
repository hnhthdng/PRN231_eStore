using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO.OrderDetail;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IMapper _mapper;
        public OrderDetailController(IOrderDetailRepository orderDetailRepository, IMapper mapper)
        {
            _orderDetailRepository = orderDetailRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var orderDetails = _orderDetailRepository.GetOrderDetails();
            var orderDetailsResponse = _mapper.Map<List<OrderDetailResponseDTO>>(orderDetails);
            return Ok(orderDetailsResponse);
        }

        [HttpGet("{orderId}")]
        public IActionResult Get(int orderId)
        {
            var orderDetails = _orderDetailRepository.GetOrderDetailsByOrderId(orderId);
            var orderDetailsResponse = _mapper.Map<List<OrderDetailResponseDTO>>(orderDetails);
            return Ok(orderDetailsResponse);
        }

        [HttpGet("{orderId}/{productId}")]
        public IActionResult Get(int orderId, int productId)
        {
            var orderDetail = _orderDetailRepository.GetOrderDetailByOrderIdAndProductId(orderId, productId);
            var orderDetailResponse = _mapper.Map<OrderDetailResponseDTO>(orderDetail);
            return Ok(orderDetailResponse);
        }

        [HttpPost]
        public IActionResult Post([FromBody] OrderDetailRequestDTO orderDetailRequest)
        {
            var orderDetail = _mapper.Map<OrderDetail>(orderDetailRequest);
            _orderDetailRepository.AddOrderDetail(orderDetail);
            return Ok();
        }
        [HttpPut]
        public IActionResult Put([FromBody] OrderDetailRequestDTO orderDetailRequest)
        {
            var isOrderDetailExist = _orderDetailRepository.GetOrderDetailByOrderIdAndProductId(orderDetailRequest.OrderId, orderDetailRequest.ProductId);
            if (isOrderDetailExist == null)
            {
                return NotFound();
            }
            var orderDetail = _mapper.Map<OrderDetail>(orderDetailRequest);
            _orderDetailRepository.UpdateOrderDetail(orderDetail);
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] OrderDetailRequestDTO orderDetailRequest)
        {
            var isOrderDetailExist = _orderDetailRepository.GetOrderDetailByOrderIdAndProductId(orderDetailRequest.OrderId, orderDetailRequest.ProductId);
            if (isOrderDetailExist == null)
            {
                return NotFound();
            }
            _orderDetailRepository.DeleteOrderDetail(isOrderDetailExist);
            return Ok();
        }


    }
}
