using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using NZParadiseAPI.Library;
using NZParadiseAPI.Model.Entities;

namespace NZParadiseAPI.Model.Abstract
{
    public interface IUsersRepository
    {
        Task<Users> Save(Users user);
        Task<Users> GetUserByID(int userID);
        Task<Users?> GetUserByEmail(string email);
        Task<List<Users>> GetUserList();
        Task<Users> Update(Users user);
    }
}
