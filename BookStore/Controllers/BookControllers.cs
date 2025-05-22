using BookStore.Helpers;
using BookStore.Properties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BookStore.DATA.DTOs;
using BookStore.DATA.DTOs.Book;
using BookStore.Entities;
using BookStore.Services;

namespace BookStore.Controllers
{
    
    [Authorize(Roles = "Admin")]
    public class BooksController : BaseController
    {
        private readonly IBookServices _bookServices;

        public BooksController(IBookServices bookServices)
        {
            _bookServices = bookServices;
        }
        [AllowAnonymous]

        [HttpGet]
        public async Task<ActionResult<List<BookDto>>> GetAll([FromQuery] BookFilter filter) => Ok(await _bookServices.GetAll(filter) , filter.PageNumber , filter.PageSize);


        [HttpPost]
        public async Task<ActionResult<Book>> Create([FromBody] BookForm bookForm) => Ok(await _bookServices.Create(bookForm,Id));
        [AllowAnonymous]

        [HttpGet("{id}") ]
        public async Task<ActionResult<Book>> GetById(Guid id) => Ok(await _bookServices.GetById(id));


        [HttpPut("{id}")]
        public async Task<ActionResult<Book>> Update([FromBody] BookUpdate bookUpdate, Guid id) => Ok(await _bookServices.Update(id , bookUpdate,Id));


        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> Delete(Guid id) =>  Ok( await _bookServices.Delete(id,Id));
        
        [HttpGet("book-statistics")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetStatistics() => Ok(await _bookServices.GetBookStatistics());
    }
}
