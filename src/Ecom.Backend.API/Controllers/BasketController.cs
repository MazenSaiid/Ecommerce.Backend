using AutoMapper;
using Ecom.Backend.Core.Dtos;
using Ecom.Backend.Core.Entities;
using Ecom.Backend.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BasketController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("GetBasketById/{id}")]
        public async Task<IActionResult> GetBasketById(string id) 
        {
            var basket = await _unitOfWork.BasketRepository.GetCustomerBasket(id);
                return Ok(basket ?? new CustomerBasket(id));
        }
        [HttpPost]
        [Route("UpdateBasket")]
        public async Task<IActionResult> UpdateBasket(CustomerBasketDto customerBasketDto)
        {
            var result = _mapper.Map<CustomerBasket>(customerBasketDto);
            var basket = await _unitOfWork.BasketRepository.UpdateCustomerBasketAsync(result);
            return Ok(basket);
        }
        [HttpDelete]
        [Route("DeleteBasket/{id}")]
        public async Task<IActionResult> DeleteBasket(string id)
        {
            var basket = await _unitOfWork.BasketRepository.DeleteCustomerBasketAsync(id);
            return Ok(basket);
        }

    }
}
