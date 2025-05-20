using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BookStore.DATA.DTOs;
using BookStore.Entities;
using BookStore.Properties;
using BookStore.Services;

namespace BookStore.Controllers
{
    public class AddressController : BaseController
    {
        private readonly IAddressServices _addressServices;

        public AddressController(IAddressServices addressServices)
        {
            _addressServices = addressServices;
        }

        [HttpGet]
        public async Task<ActionResult<List<AddressDto>>> GetAll([FromQuery] AddressFilter filter) => Ok(await _addressServices.GetAll(filter,Id) , filter.PageNumber , filter.PageSize);

        [HttpPost]
        public async Task<ActionResult<Address>> Create([FromBody] AddressForm addressForm) => Ok(await _addressServices.Create(addressForm, Id));

        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetById(Guid id) => Ok(await _addressServices.GetById(id));
        [HttpPut("{id}")]
        public async Task<ActionResult<Address>> Update([FromBody] AddressUpdate addressUpdate, Guid id) => Ok(await _addressServices.Update(id, addressUpdate, Id));

        [HttpDelete("{id}")]
        public async Task<ActionResult<Address>> Delete(Guid id) => Ok(await _addressServices.Delete(id, Id));
    }
}