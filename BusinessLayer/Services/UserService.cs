using BusinessLayer.Abstract;
using BusinessLayer.Security;
using DataAccessLayer.Abstract;
using Entities.Concrete;
using Entities.DTOs.UserDTOs;
using Entities.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenHelper _jwtTokenHelper;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(IUserRepository userRepository, JwtTokenHelper jwtTokenHelper, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _jwtTokenHelper = jwtTokenHelper;
            _passwordHasher = passwordHasher;
        }

        public async Task<(bool Success, string Message, UserDetailDto? User)> RegisterUserAsync(UserRegisterDto userRegisterDto)
        {
            if (string.IsNullOrEmpty(userRegisterDto.Username) ||
                string.IsNullOrEmpty(userRegisterDto.FirstName) ||
                string.IsNullOrEmpty(userRegisterDto.LastName) ||
                string.IsNullOrEmpty(userRegisterDto.Email) ||
                string.IsNullOrEmpty(userRegisterDto.Password) ||
                string.IsNullOrEmpty(userRegisterDto.ConfirmPassword) ||
                string.IsNullOrEmpty(userRegisterDto.PhoneNumber))
            {
                return (false, "All fields are required.", null);
            }

            if (userRegisterDto.Password != userRegisterDto.ConfirmPassword)
            {
                return (false, "Password and Confirm Password must match.", null);
            }

            if (!IsValidEmail(userRegisterDto.Email))
            {
                return (false, "Invalid email format.", null);
            }

            var existingUserByUsername = await _userRepository.GetByUsernameAsync(userRegisterDto.Username);
            if (existingUserByUsername != null)
            {
                return (false, "A user with this username already exists.", null);
            }

            var existingUserByEmail = await _userRepository.GetByEmailAsync(userRegisterDto.Email);
            if (existingUserByEmail != null)
            {
                return (false, "A user with this email already exists.", null);
            }

            var user = userRegisterDto.ToUserFromUserRegisterDto();

            user.PasswordHash = _passwordHasher.HashPassword(user, userRegisterDto.Password);

            var createdUser = await _userRepository.CreateAsync(user);

            var userDetailDto = user.ToUserDetailDto();

            return (true, "User registered successfully.", userDetailDto);
        }

        public async Task<(bool Success, string Message, List<UserDetailDto>? Users)> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();

            if (users == null || !users.Any())
            {
                return (false, "No users found.", null);
            }

            var userDtos = users.Select(u => u.ToUserDetailDto()).ToList();
            return (true, $"{users.Count} users found.", userDtos);
        }

        public async Task<(bool Success, string Message, UserDetailDto? User)> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return (false, "User not found.", null);
            }

            var userDto = user.ToUserDetailDto();
            return (true, "User found.", userDto);
        }

        public async Task<(bool Success, string Message, UserDetailDto? User)> UpdateUserAsync(int id, UserUpdateDto userUpdateDto)
        {
            if (string.IsNullOrEmpty(userUpdateDto.Username) ||
                string.IsNullOrEmpty(userUpdateDto.FirstName) ||
                string.IsNullOrEmpty(userUpdateDto.LastName) ||
                string.IsNullOrEmpty(userUpdateDto.Email) ||
                string.IsNullOrEmpty(userUpdateDto.PhoneNumber))
            {
                return (false, "All fields are required.", null);
            }

            if (!IsValidEmail(userUpdateDto.Email))
            {
                return (false, "Invalid email format.", null);
            }

            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                return (false, "User not found.", null);
            }

            var existingUserByUsername = await _userRepository.GetByUsernameAsync(userUpdateDto.Username);
            if (existingUserByUsername != null && existingUserByUsername.ID != id)
            {
                return (false, "A user with this username already exists.", null);
            }

            var existingUserByEmail = await _userRepository.GetByEmailAsync(userUpdateDto.Email);
            if (existingUserByEmail != null && existingUserByEmail.ID != id)
            {
                return (false, "A user with this email already exists.", null);
            }

            var user = await _userRepository.UpdateAsync(id, userUpdateDto);

            if (user == null)
            {
                return (false, "User not found or update failed.", null);
            }

            var updatedUserDto = user.ToUserDetailDto();
            return (true, "User updated successfully.", updatedUserDto);
        }

        public async Task<(bool Success, string Message, UserDetailDto? User)> DeleteUserAsync(int id)
        {
            var user = await _userRepository.DeleteAsync(id);

            if (user == null)
            {
                return (false, "User not found or delete failed.", null);
            }

            var deletedUserDto = user.ToUserDetailDto();
            return (true, "User deleted successfully.", deletedUserDto);
        }

        public async Task<(bool Success, string Message, string? Token)> LoginAsync(UserLoginDto userLoginDto)
        {
            var user = await _userRepository.GetByEmailAsync(userLoginDto.Email);

            if (user == null)
            {
                return (false, "Invalid email or password.", null);
            }

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userLoginDto.Password);

            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                return (false, "Invalid email or password.", null);
            }

            var token = _jwtTokenHelper.CreateToken(user);

            return (true, "Login successful.", token);
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

    }
}
