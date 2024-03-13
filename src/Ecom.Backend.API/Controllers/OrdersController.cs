using AutoMapper;
using Ecom.Backend.API.Errors;
using Ecom.Backend.Core.Dtos;
using Ecom.Backend.Core.Entities.Orders;
using Ecom.Backend.Core.Interfaces;
using Ecom.Backend.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecom.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderServices _orderServices;
        private readonly IMapper _mapper;

        public OrdersController(IUnitOfWork unitOfWork,IOrderServices orderServices,IMapper mapper )
        {
            _unitOfWork = unitOfWork;
            _orderServices = orderServices;
            _mapper = mapper;
        }
        [HttpPost]
        [Route("CreateOrder")]
        public async Task<IActionResult> CreateOrder(OrderDto orderDto)
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x=>x.Type == ClaimTypes.Email)?.Value;
            var address = _mapper.Map<AddressDto, ShippingAddress>(orderDto.ShippingAddress);
            var order = await _orderServices.CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.BasketId,address);
            if (order is null)
                return BadRequest(new CommonResponseError(400,"Failed to create an order"));
            return Ok(order);
        }
        [HttpGet]
        [Route("GetOrderForUser")]
        public async Task<IActionResult> GetOrderForUser()
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x=>x.Type == ClaimTypes.Email)?.Value;

            var order = await _orderServices.GetOrdersForUserAsync(email);
            var result = _mapper.Map<IReadOnlyList<Order>,IReadOnlyList<OrderResultDto>>(order);
            if (order is null)
                return NotFound(new CommonResponseError(404));
            return Ok(result);

        }
        [HttpGet]
        [Route("GetOrderById/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            var order = await _orderServices.GetOrderByIdAsync(email,id);
            var result = _mapper.Map<Order,OrderResultDto>(order); ;
            if (order is null)
                return NotFound(new CommonResponseError(404));
            return Ok(result);

        }
        [HttpGet]
        [Route("GetDeliveryMethods")]
        public async Task<IActionResult> GetDeliveryMethods()
        {
            var deliveryMethod = await _orderServices.GetDeliveryMethodsAsync();
            if (deliveryMethod is null)
                return NotFound(new CommonResponseError(404));
            return Ok(deliveryMethod);

        }
    }
}
