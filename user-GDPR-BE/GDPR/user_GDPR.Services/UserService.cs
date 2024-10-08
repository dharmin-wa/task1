using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using user_GDPR.Models;
using user_GDPR.Repositories.Interface;
using user_GDPR.Services.Interface;
using static MongoDB.Driver.WriteConcern;

namespace user_GDPR.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncryptionRepository _encryptionHelper;

        public UserService(IUserRepository userRepository, IEncryptionRepository encryptionHelper)
        {
            _userRepository = userRepository;
            _encryptionHelper = encryptionHelper;
        }

        public async Task<Users> AuthenticateUser(string email, string password)
        {
            email = _encryptionHelper.DecryptString(email);
            password = _encryptionHelper.DecryptString(password);
            return await _userRepository.AuthenticateUser(email, password);
        }

        public async Task<Users> CreateUser(Users user)
        {
            user.Email = _encryptionHelper.DecryptString(user.Email);
            user.Password = _encryptionHelper.DecryptString(user.Password);
            user.MobileNo = _encryptionHelper.DecryptString(user.MobileNo);
            return await _userRepository.CreateUser(user);
        }

        public async Task<bool> DeleteUser(string userId)
        {
            return await _userRepository.DeleteUser(userId);
        }

        public Task<List<Users>> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public async Task<Users> GetUserDetails(string userId)
        {
            return await _userRepository.GetUserDetails(userId);
        }
    }
}
