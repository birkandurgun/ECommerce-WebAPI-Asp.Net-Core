using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        public readonly ECommerceWithWebAPIDbContext _context;
        public AddressRepository(ECommerceWithWebAPIDbContext context) {
            _context = context;
        }

        public async Task<List<Address>> GetByUserIdAsync(int userId)
        {
            return await _context.Addresses
                                 .Where(a => a.UserId == userId)
                                 .ToListAsync();
        }
        public async Task<Address?> GetByIdAsync(int id)
        {
            return await _context.Addresses.FindAsync(id);
        }
        public async Task<Address> CreateAsync(int userId, Address addressModel)
        {
            await _context.Addresses.AddAsync(addressModel);
            await _context.SaveChangesAsync();
            return addressModel;
        }

        public async Task<Address?> UpdateAsync(int id, Address addressModel)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id);

            if(address == null)
            {
                return null;
            }

            address.State = addressModel.State;
            address.City = addressModel.City;
            address.AddressLine = addressModel.AddressLine;
            address.Country = addressModel.Country;

            await _context.SaveChangesAsync();

            return address;
        }

        public async Task<Address?> DeleteAsync(int id)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id);
            if(address == null)
            {
                return null;
            }
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            return address;
        }
    }
}
