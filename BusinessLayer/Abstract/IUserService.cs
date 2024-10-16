using Entities.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IUserService
    {
        Task<(bool Success, string Message, UserDetailDto? User)> RegisterUserAsync(UserRegisterDto userRegisterDto);
        Task<(bool Success, string Message, List<UserDetailDto>? Users)> GetAllUsersAsync();
        Task<(bool Success, string Message, UserDetailDto? User)> GetUserByIdAsync(int id);
        Task<(bool Success, string Message, UserDetailDto? User)> UpdateUserAsync(int id, UserUpdateDto userUpdateDto);
        Task<(bool Success, string Message, UserDetailDto? User)> DeleteUserAsync(int id);
        Task<(bool Success, string Message, string? Token)> LoginAsync(UserLoginDto userLoginDto);
        bool IsValidEmail(string email);
    }
}
