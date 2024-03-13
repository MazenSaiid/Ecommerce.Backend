using AutoMapper;
using Ecom.Backend.API.Helper;
using Ecom.Backend.Core.Dtos;
using Ecom.Backend.Core.Entities;
using Ecom.Backend.Core.Interfaces;
using Ecom.Backend.Core.Sharing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("")]
        public async Task<ActionResult> GetAllProducts()
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync(x => x.Category);
            //var products = await _unitOfWork.ProductRepository.GetAllAsync();
            if (products is not null)
            {
                var result = _mapper.Map<List<ProductDto>>(products);
                return Ok(result);
            }
            return NotFound("No Products Found");
        }
        [HttpGet]
        [Route("GetAllProductsSorted")]
        public async Task<ActionResult> GetAllProductsSorted([FromQuery] ProductParams productParams)
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync(productParams);
            //var products = await _unitOfWork.ProductRepository.GetAllAsync();
            if (products is not null)
            {
                var result = _mapper.Map<List<ProductDto>>(products.ProductDtos);
                return Ok(new Pagination<ProductDto>(productParams.PageNumber,productParams.PageSize,products.TotalItems,result));
            }
            return NotFound("No Products Found");
        }
        [HttpGet]
        [Route("GetProductById/{id}")]
        public async Task<ActionResult> GetProductById(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id, x => x.Category);
            if (product is not null)
            {
                var result = _mapper.Map<ProductDto>(product);
                return Ok(result);
            }
            return NotFound($"Product not Found with id {id}");
        }
        [HttpPost]
        [Route("CreateProduct")]
        public async Task<ActionResult> CreateProduct([FromForm] CreateProductDto productDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var product = await _unitOfWork.ProductRepository.AddAsync(productDto);
                    return product ? Ok(product) : BadRequest($"Failed to add a new product{product}");
                }
                return BadRequest("Model State is not Valid");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("UpdateProduct/{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromForm] UpdateProductDto updateProductDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var product = await _unitOfWork.ProductRepository.UpdateAsync(id,updateProductDto);
                    return product ? Ok(product) : BadRequest($"Failed to update product{product}");
                }
                return BadRequest("Model State is not Valid");

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("DeleteProduct/{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var product = await _unitOfWork.ProductRepository.DeleteAsyncWithPicture(id);
                    return product ? Ok(product) : BadRequest($"Failed to delete product and repsonse is {product}");
                }
                return NotFound($"Product not Found with id {id}");

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

    }
}
