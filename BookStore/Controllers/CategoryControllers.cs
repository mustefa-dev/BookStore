using BookStore.Helpers;
using BookStore.Properties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BookStore.DATA.DTOs;
using BookStore.DATA.DTOs.Category;
using BookStore.Entities;
using BookStore.Services;

namespace BookStore.Controllers
{
    public class CategorysController : BaseController
    {
        private readonly ICategoryServices _categoryServices;

        public CategorysController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }

        
        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> GetAll([FromQuery] CategoryFilter filter) => Ok(await _categoryServices.GetAll(filter) , filter.PageNumber , filter.PageSize);

        
        [HttpPost]
        public async Task<ActionResult<Category>> Create([FromBody] CategoryForm categoryForm) => Ok(await _categoryServices.Create(categoryForm));

        
        [HttpPut("{id}")]
        public async Task<ActionResult<Category>> Update([FromBody] CategoryUpdate categoryUpdate, Guid id) => Ok(await _categoryServices.Update(id , categoryUpdate));

        
        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> Delete(Guid id) =>  Ok( await _categoryServices.Delete(id));
        
    }
}
