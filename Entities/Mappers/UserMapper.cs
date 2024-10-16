using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Entities.DTOs.UserDTOs;

namespace Entities.Mappers
{
    public static class UserMapper
    {
        public static UserDetailDto ToUserDetailDto(this User userModel)
        {
            return new UserDetailDto
            {
                ID = userModel.ID,
                Username = userModel.Username,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email,
                CreatedAt = userModel.CreatedAt,
                PhoneNumber = userModel.PhoneNumber,
                UserRole = userModel.UserRole.ToString(),
            };
        }

        public static User ToUserFromUserRegisterDto(this UserRegisterDto userRegisterDto) {
            return new User
            {
                Username = userRegisterDto.Username,
                FirstName = userRegisterDto.FirstName,
                LastName = userRegisterDto.LastName,
                Email = userRegisterDto.Email,
                CreatedAt = userRegisterDto.CreatedAt,
                PhoneNumber = userRegisterDto.PhoneNumber,
                UserRole = userRegisterDto.UserRole
            };
        }

    }
}
