using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BookStore.DATA.DTOs;
using BookStore.Entities;
using BookStore.Properties;
using BookStore.Services;

namespace BookStore.Controllers
{
    public class DistrictsController : BaseController
    {
        private readonly IDistrictServices _districtServices;

        public DistrictsController(IDistrictServices districtServices)
        {
            _districtServices = districtServices;
        }

        
        [HttpGet]
        public async Task<ActionResult<List<DistrictDto>>> GetAll([FromQuery] DistrictFilter filter) => Ok(await _districtServices.GetAll(filter) , filter.PageNumber , filter.PageSize);

        
        [HttpPost]
        public async Task<ActionResult<District>> Create([FromBody] DistrictForm districtForm) => Ok(await _districtServices.Create(districtForm));

        
        [HttpPut("{id}")]
        public async Task<ActionResult<District>> Update([FromBody] DistrictUpdate districtUpdate, Guid id) => Ok(await _districtServices.Update(id , districtUpdate));

        
        [HttpDelete("{id}")]
        public async Task<ActionResult<District>> Delete(Guid id) =>  Ok( await _districtServices.Delete(id));
        
    }
}
