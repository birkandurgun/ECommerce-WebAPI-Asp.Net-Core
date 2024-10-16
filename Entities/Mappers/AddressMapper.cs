using Entities.Concrete;
using Entities.DTOs.AddressDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Mappers
{
    public static class AddressMapper
    {
        public static AddressDetailDto ToAddressDetailDto(this Address addressModel)
        {
            return new AddressDetailDto
            {
                Id = addressModel.Id,
                AddressLine = addressModel.AddressLine,
                City = addressModel.City,
                Country = addressModel.Country,
                State = addressModel.State
            };
        }

        public static Address ToAddressFromCreateAddressDto(this CreateAddressDto createAddressDto, int userId)
        {
            return new Address
            {
                AddressLine = createAddressDto.AddressLine,
                City = createAddressDto.City,
                Country = createAddressDto.Country,
                State = createAddressDto.State,
                UserId = userId
            };
        }

        public static Address ToAddressFromUpdateAddressDto(this  UpdateAddressDto updateAddressDto)
        {
            return new Address
            {
                AddressLine = updateAddressDto.AddressLine,
                City = updateAddressDto.City,
                Country = updateAddressDto.Country,
                State = updateAddressDto.State
            };
        }
    }
}
