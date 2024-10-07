using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using user_GDPR.Models;

namespace user_GDPR.Services.Interface
{
	public interface IUserService
	{
        Task<Users> CreateUser(Users user);
        Task<Users> GetUserDetails(string userId);
        Task<bool> DeleteUser(string userId);
        Task<List<Users>> GetAllUsers();
        Task<Users> AuthenticateUser(string email, string password);
    }
}
