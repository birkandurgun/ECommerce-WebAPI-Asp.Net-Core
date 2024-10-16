using Entities.DTOs.AddressDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IAddressService
    {
        public Task<(bool Success, string Message, AddressDetailDto? Address)> CreateAddressAsync(int userId, CreateAddressDto createAddressDto);
        Task<(bool Success, string Message, List<AddressDetailDto> Addresses)> GetAddressesByUserIdAsync(int userId);
        public Task<(bool Success, string Message, AddressDetailDto Address)> GetAddressByIdAsync(int userIdFromToken, int id);
        public Task<(bool Success, string Message, AddressDetailDto? Address)> UpdateAddressAsync(int userIdFromToken, int id, UpdateAddressDto updateAddressDto);
        public Task<(bool Success, string Message)> DeleteAddressAsync(int userIdFromToken, int id);
    }
}
