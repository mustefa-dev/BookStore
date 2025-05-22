using Microsoft.AspNetCore.Mvc;
using BookStore.DATA.DTOs.Category;
using BookStore.Entities;
using BookStore.Services;
using Microsoft.AspNetCore.Authorization;

namespace BookStore.Controllers
{
    [Authorize(Roles = "Admin")]

    public class CategoriesController : BaseController
    {
        private readonly ICategoryServices _categoryServices;

        public CategoriesController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }

        [AllowAnonymous]

        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> GetAll([FromQuery] CategoryFilter filter) => Ok(await _categoryServices.GetAll(filter) , filter.PageNumber , filter.PageSize);

        [HttpPost]
        public async Task<ActionResult<Category>> Create([FromBody] CategoryForm categoryForm) => Ok(await _categoryServices.Create(categoryForm));
        
        [AllowAnonymous]

        [HttpGet("{id}")]
        
        public async Task<ActionResult<Category>> GetById(Guid id) => Ok(await _categoryServices.GetById(id));
       

        [HttpPut("{id}")]
        public async Task<ActionResult<Category>> Update([FromBody] CategoryUpdate categoryUpdate, Guid id) => Ok(await _categoryServices.Update(id , categoryUpdate));


        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> Delete(Guid id) =>  Ok( await _categoryServices.Delete(id));
        
    }
}
