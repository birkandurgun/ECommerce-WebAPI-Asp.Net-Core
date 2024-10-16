using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using Entities.DTOs.AddressDTOs;
using Entities.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class AddressService:IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IUserRepository _userRepository;

        public AddressService(IAddressRepository addressRepository, IUserRepository userRepository)
        {
            _addressRepository = addressRepository;
            _userRepository = userRepository;
        }

        public async Task<(bool Success, string Message, AddressDetailDto? Address)> CreateAddressAsync(int userId, CreateAddressDto createAddressDto)
        {
            if (string.IsNullOrEmpty(createAddressDto.AddressLine) ||
                string.IsNullOrEmpty(createAddressDto.City) ||
                string.IsNullOrEmpty(createAddressDto.State) ||
                string.IsNullOrEmpty(createAddressDto.Country))
            {
                return (false, "All fields must be filled.", null);
            }

            if (!await _userRepository.Exist(userId))
            {
                return (false, "User does not exist.", null);
            }

            var addressModel = createAddressDto.ToAddressFromCreateAddressDto(userId);
            var createdAddress = await _addressRepository.CreateAsync(userId, addressModel);

            var addressDetailDto = createdAddress.ToAddressDetailDto();

            return (true, "Address created successfully.", addressDetailDto);
        }

        public async Task<(bool Success, string Message, List<AddressDetailDto> Addresses)> GetAddressesByUserIdAsync(int userId)
        {
            var addresses = await _addressRepository.GetByUserIdAsync(userId);

            if (addresses == null || !addresses.Any())
            {
                return (false, "No addresses found for this user.", new List<AddressDetailDto>());
            }

            var addressDetailDtos = addresses.Select(address => address.ToAddressDetailDto()).ToList();

            return (true, "Addresses retrieved successfully.", addressDetailDtos);
        }

        public async Task<(bool Success, string Message, AddressDetailDto Address)> GetAddressByIdAsync(int userIdFromToken, int id)
        {
            var address = await _addressRepository.GetByIdAsync(id);

            if (address == null || address.UserId != userIdFromToken)
            {
                return (false, "Address not found or you do not have permission to access this address.", null);
            }

            var addressDetailDto = address.ToAddressDetailDto();

            return (true, "Address retrieved successfully.", addressDetailDto);
        }

        public async Task<(bool Success, string Message, AddressDetailDto? Address)> UpdateAddressAsync(int userIdFromToken, int id, UpdateAddressDto updateAddressDto)
        {
            if (string.IsNullOrEmpty(updateAddressDto.AddressLine) ||
                string.IsNullOrEmpty(updateAddressDto.City) ||
                string.IsNullOrEmpty(updateAddressDto.State) ||
                string.IsNullOrEmpty(updateAddressDto.Country))
            {
                return (false, "All fields must be filled.", null);
            }

            var address = await _addressRepository.GetByIdAsync(id);

            if (address == null || address.UserId != userIdFromToken)
            {
                return (false, "Address not found or you do not have permission to update this address.", null);
            }

            var updatedAddress = updateAddressDto.ToAddressFromUpdateAddressDto();

            var result = await _addressRepository.UpdateAsync(id, updatedAddress);

            if (result == null)
            {
                return (false, "Address update failed.", null);
            }

            var updatedAddressDto = result.ToAddressDetailDto();

            return (true, "Address updated successfully.", updatedAddressDto);
        }

        public async Task<(bool Success, string Message)> DeleteAddressAsync(int userIdFromToken, int id)
        {
            var address = await _addressRepository.GetByIdAsync(id);

            if (address == null || address.UserId != userIdFromToken)
            {
                return (false, "Address not found or you do not have permission to delete this address.");
            }

            var deletedAddress = await _addressRepository.DeleteAsync(id);

            if (deletedAddress == null)
            {
                return (false, "Address deletion failed."); 
            }

            return (true, "Address deleted successfully."); 
        }

    }
}

