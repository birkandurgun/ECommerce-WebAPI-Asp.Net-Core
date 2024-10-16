using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repositories;
using Entities.Concrete;
using Entities.DTOs.AddressDTOs;
using Entities.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("GetUserAddresses")]
        public async Task<IActionResult> GetUserAddresses()
        {
            
            var userIdFromToken = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var (success, message, addresses) = await _addressService.GetAddressesByUserIdAsync(userIdFromToken);

            if (!success)
            {
                return NotFound(new { Success = success, Message = message });
            }

            return Ok(new { Success = success, Message = message, Addresses = addresses });
        }

        [HttpGet]
        [Route("GetAddressById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var userIdFromToken = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _addressService.GetAddressByIdAsync(userIdFromToken, id);

            if (!result.Success)
            {
                return NotFound(result.Message); 
            }

            return Ok(result.Address); 
        }

        [HttpPost("AddAddress")]
        public async Task<IActionResult> Create([FromBody] CreateAddressDto createAddressDto)
        {
            var userIdFromToken = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var (success, message, addressDetailDto) = await _addressService.CreateAddressAsync(userIdFromToken, createAddressDto);

            if (!success)
            {
                return BadRequest(message);
            }

            return CreatedAtAction(nameof(GetById), new { id = addressDetailDto?.Id }, addressDetailDto);
        }

        [HttpPut]
        [Route("UpdateAddress/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateAddressDto updateAddressDto)
        {
            var userIdFromToken = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _addressService.UpdateAddressAsync(userIdFromToken, id, updateAddressDto);

            if (!result.Success)
            {
                return BadRequest(result.Message); 
            }

            return Ok(result.Address);
        }

        [HttpDelete]
        [Route("DeleteAddress/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var userIdFromToken = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _addressService.DeleteAddressAsync(userIdFromToken, id);

            if (!result.Success)
            {
                return BadRequest(result.Message); 
            }

            return NoContent();
        }

    }
}
