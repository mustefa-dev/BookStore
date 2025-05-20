using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BookStore.DATA.DTOs;
using BookStore.Entities;
using BookStore.Properties;
using BookStore.Services;

namespace BookStore.Controllers
{
    public class CitysController : BaseController
    {
        private readonly ICityServices _cityServices;

        public CitysController(ICityServices cityServices)
        {
            _cityServices = cityServices;
        }

        
        [HttpGet]
        public async Task<ActionResult<List<CityDto>>> GetAll([FromQuery] CityFilter filter) => Ok(await _cityServices.GetAll(filter) , filter.PageNumber , filter.PageSize);

        
        [HttpPost]
        public async Task<ActionResult<City>> Create([FromBody] CityForm cityForm) => Ok(await _cityServices.Create(cityForm));

        
        [HttpPut("{id}")]
        public async Task<ActionResult<City>> Update([FromBody] CityUpdate cityUpdate, Guid id) => Ok(await _cityServices.Update(id , cityUpdate));

        
        [HttpDelete("{id}")]
        public async Task<ActionResult<City>> Delete(Guid id) =>  Ok( await _cityServices.Delete(id));
        
    }
}
