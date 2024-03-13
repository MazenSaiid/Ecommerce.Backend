using AutoMapper;
using Ecom.Backend.Core.Dtos;
using Ecom.Backend.Core.Entities;
using Ecom.Backend.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> GetCategories()
        {
            var categories =await _unitOfWork.CategoryRepository.GetAllAsync();
            if(categories is not null)
            {
                //Manual Mapping

                //var result = categories.Select(x => new ListingCategoryDto
                //{
                //    Id = x.Id,
                //    Name = x.Name,
                //    Description = x.Description
                //}).ToList();
                var result = _mapper.Map<IReadOnlyList<Category>, IReadOnlyList<ListingCategoryDto>>((IReadOnlyList<Category>)categories);
                return Ok(result);
            }
                
            return BadRequest("No Categories Found");
        }
        [HttpGet]
        [Route("GetCategoryById/{id}")]
        public async Task<ActionResult> GetCategoryById(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category is not null)
            {
                //var newCategoryDto = new ListingCategoryDto
                //{
                //    Id = category.Id,
                //    Name= category.Name,
                //    Description= category.Description
                //};
                var newCategoryDto = _mapper.Map<Category, ListingCategoryDto>(category);
                return Ok(newCategoryDto);
            }
                
            return BadRequest($"No Categories Found with id {id}");
        }
        [HttpPost]
        [Route("AddCategory")]
        public async Task<ActionResult> AddCategory(CategoryDto categoryDto)
        {
            try
            {
                if(ModelState.IsValid) 
                {
                    //var category = new Category()
                    //{
                    //    Name = categoryDto.Name,
                    //    Description = categoryDto.Description,
                    //};
                    var category = _mapper.Map<Category>(categoryDto);
                    await _unitOfWork.CategoryRepository.AddAsync(category);
                    return Ok(category);
                }
                return BadRequest(categoryDto);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
           
        }
        [HttpPut]
        [Route("UpdateCategory")]
        public async Task<ActionResult> UpdateCategory(UpdateCategoryDto categoryDto)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var existingCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryDto.Id);
                    if (existingCategory is not null)
                    {
                        //existingCategory.Name = categoryDto.Name;
                        //existingCategory.Description = categoryDto.Description;
                        _mapper.Map(categoryDto, existingCategory);
                        await _unitOfWork.CategoryRepository.UpdateAsync(categoryDto.Id, existingCategory);
                        return Ok(existingCategory);
                    }
                    return BadRequest($"Category Not Found, Id {categoryDto.Id} is incorrect");
                }
                return BadRequest("Model is not Valid");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [HttpDelete]
        [Route("DeleteCategory/{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var categoryToDelete = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
                    if (categoryToDelete is not null) 
                    {
                        await _unitOfWork.CategoryRepository.DeleteAsync(id);
                        return Ok($"Category {categoryToDelete.Name} deleted Successfully!");
                    }
                    return BadRequest($"Category Not Found, Id {id} is incorrect");
                }
                return BadRequest("Model is not Valid");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        
    }
}
