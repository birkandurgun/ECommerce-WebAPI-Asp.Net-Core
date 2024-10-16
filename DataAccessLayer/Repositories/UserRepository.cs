using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using Entities.Concrete;
using Entities.DTOs.UserDTOs;
using Entities.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ECommerceWithWebAPIDbContext _context;
        public UserRepository(ECommerceWithWebAPIDbContext context) 
        {
            _context = context; 
        }
        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(i => i.ID==id);
        }

        public async Task<User> CreateAsync(User userModel)
        {

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.Users.AddAsync(userModel);
                    await _context.SaveChangesAsync(); 

                    var cart = new Cart
                    {
                        UserId = userModel.ID
                    };
                    await _context.Carts.AddAsync(cart);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return userModel;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Error!", ex);
                }
            }
        }

        public async Task<User?> DeleteAsync(int id)
        {
            var userModel = await _context.Users.FirstOrDefaultAsync(u => u.ID == id);
            if (userModel == null) {
                return null;
            }
            _context.Users.Remove(userModel);
            await _context.SaveChangesAsync();
            return userModel;
        }

        public async Task<User?> UpdateAsync(int id, UserUpdateDto userUpdateDto)
        {
            var userModel = await _context.Users.FirstOrDefaultAsync(u => u.ID == id);
            if (userModel == null)
            {
                return null;
            }
            userModel.Username = userUpdateDto.Username;
            userModel.FirstName = userUpdateDto.FirstName;
            userModel.LastName = userUpdateDto.LastName;
            userModel.Email = userUpdateDto.Email;
            userModel.PhoneNumber = userUpdateDto.PhoneNumber;

            await _context.SaveChangesAsync();

            return userModel;

        }

        public async Task<bool> Exist(int id)
        {
            return await _context.Users.AnyAsync(u => u.ID == id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

    }
}
