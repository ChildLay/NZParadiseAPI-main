using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NZParadiseAPI.Library;
using NZParadiseAPI.Model.Abstract;
using NZParadiseAPI.Model.Entities;
using BCrypt.Net;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.EntityFrameworkCore;

namespace NZParadiseAPI.Model.Concrete
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ApplicationDbContext _context;
        public UsersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Users?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<List<Users>> GetUserList()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Users?> GetUserByID(int userID)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserID == userID);
        }

        public async Task<Users> Save(Users user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Users> Update(Users user)
        {
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
