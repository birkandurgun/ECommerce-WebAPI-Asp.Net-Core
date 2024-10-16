using Entities.Concrete;
using Entities.DTOs.AddressDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IAddressRepository
    {
        Task<List<Address>> GetByUserIdAsync(int userId);
        Task<Address?> GetByIdAsync(int id);
        Task<Address> CreateAsync(int userId, Address addressModel);
        Task<Address?> UpdateAsync(int id, Address addressModel);
        Task<Address?> DeleteAsync(int id);
    }
}
