using DataAccessLayer.Concrete;
using Entities.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Repositories;
using DataAccessLayer.Abstract;
using Entities.DTOs.UserDTOs;
using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(ECommerceWithWebAPIDbContext context, IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var (success, message, users) = await _userService.GetAllUsersAsync();

            if (!success)
            {
                return NotFound(new { Success = success, Message = message });
            }

            return Ok(new { Success = success, Message = message, Users = users });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var (success, message, user) = await _userService.GetUserByIdAsync(id);

            if (!success)
            {
                return NotFound(new { Success = success, Message = message });
            }

            return Ok(new { Success = success, Message = message, User = user });
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            var (success, message, userDetailDto) = await _userService.RegisterUserAsync(userRegisterDto);

            if (!success)
            {
                return BadRequest(message);
            }

            return CreatedAtAction(nameof(GetById), new { id = userDetailDto.ID }, userDetailDto);
        }

        [Authorize(Roles = "Customer")]
        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UserUpdateDto userUpdateDto)
        {
            var userIdFromToken = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            if (userIdFromToken != id)
            {
                return Forbid("You are not authorized to update this user.");
            }

            var (success, message, user) = await _userService.UpdateUserAsync(id, userUpdateDto);

            if (!success)
            {
                return NotFound(new { Success = success, Message = message });
            }

            return Ok(new { Success = success, Message = message, User = user });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var (success, message, user) = await _userService.DeleteUserAsync(id);

            if (!success)
            {
                return NotFound(new { Success = success, Message = message });
            }

            return Ok(new { Success = success, Message = message, User = user });
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var result = await _userService.LoginAsync(userLoginDto);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { token = result.Token });
        }
    }

}
