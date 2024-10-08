using log4net;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using user_GDPR.Models;
using user_GDPR.Models.Context;
using user_GDPR.Repositories.Interface;
using user_GDPR.Services.Interface;

namespace user_GDPR.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MongoDbContext _context;
        private readonly ILog _logger;
        private readonly IEncryptionRepository _encryptionHelper;
        private readonly TokenRepository _tokenRepository;

        public UserRepository(MongoDbContext context, IEncryptionRepository encryptionHelper, TokenRepository tokenRepository)
        {
            _context = context;
            _logger = LogManager.GetLogger(typeof(UserRepository));
            _encryptionHelper = encryptionHelper;
            _tokenRepository = tokenRepository;

            var indexKeys = Builders<Users>.IndexKeys
                .Ascending(u => u.IsDeleted)
                .Ascending(u => u.Id);

            var indexModel = new CreateIndexModel<Users>(indexKeys);
            _context.Users.Indexes.CreateOne(indexModel);
        }

        public async Task<Users> CreateUser(Users user)
        {
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                _logger.Info("Email or password is null or empty.");
                throw new ArgumentException("Email and password are required.");
            }

            if (user.IsUserConsent != true)
            {
                _logger.Info("User consent is not given.");
                throw new ArgumentException("User consent is required to create an account.");
            }

            try
            {
                user.Email = _encryptionHelper.EncryptString(user.Email);
                user.Password = _encryptionHelper.EncryptString(user.Password);
                user.MobileNo = _encryptionHelper.EncryptString(user.MobileNo);
                user.CreatedAt = DateTime.UtcNow;

                await _context.Users.InsertOneAsync(user).ConfigureAwait(false);
                _logger.Info($"User created successfully: {user.Email}");
                return user;
            }
            catch (MongoException mongoEx)
            {
                _logger.Error("MongoDB error occurred while creating user.", mongoEx);
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error("Error occurred while creating user.", ex);
                throw;
            }
        }

        public async Task<Users> GetUserDetails(string userId)
        {
            try
            {
                var filter = Builders<Users>.Filter.And(
                    Builders<Users>.Filter.Eq(u => u.IsDeleted, false),
                    Builders<Users>.Filter.Eq(u => u.Id, userId)
                );

                var user = await _context.Users.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);

                if (user != null)
                {
                    user.Email = _encryptionHelper.DecryptString(user.Email);
                    user.MobileNo = _encryptionHelper.DecryptString(user.MobileNo);

                    _logger.Info($"User retrieved successfully: {user.Email}");
                    return user;
                }

                _logger.Info($"User not found: {userId}");
                return null;
            }
            catch (MongoException mongoEx)
            {
                _logger.Error("MongoDB error occurred while retrieving user details.", mongoEx);
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error("Error occurred while retrieving user details.", ex);
                throw;
            }
        }

        public async Task<List<Users>> GetAllUsers()
        {
            try
            {
                var filter = Builders<Users>.Filter.Eq(u => u.IsDeleted, false);
                var users = await _context.Users.Find(filter).ToListAsync().ConfigureAwait(false);

                foreach (var user in users) {
                    user.Email = _encryptionHelper.DecryptString(user.Email);
                    user.MobileNo = _encryptionHelper.DecryptString(user.MobileNo);
                }

                _logger.Info($"Retrieved {users.Count} users successfully.");
                return users;
            }
            catch (MongoException mongoEx)
            {
                _logger.Error("MongoDB error occurred while retrieving all users.", mongoEx);
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error("Error occurred while retrieving all users.", ex);
                throw;
            }
        }

        public async Task<Users> AuthenticateUser(string email, string password)
        {
            try
            {
                _logger.Info($"Authenticating user with email: {email}");

                var user = await _context.Users.Find(u => u.Email == _encryptionHelper.EncryptString(email)
                            && u.Password == _encryptionHelper.EncryptString(password)).FirstOrDefaultAsync();

                if (user != null && password != null)
                {
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error("Error occurred while retrieving all users.", ex);
                throw;
            }
        }

        public async Task<bool> DeleteUser(string userId)
        {
            try
            {
                var filter = Builders<Users>.Filter.Eq(u => u.Id, userId);

                var update = Builders<Users>.Update.Set(u => u.IsDeleted, true);

                var result = await _context.Users.UpdateOneAsync(filter, update).ConfigureAwait(false);

                if (result.ModifiedCount > 0)
                {
                    _logger.Info($"User marked as deleted: {userId}");
                    return true;
                }

                _logger.Info($"User not found or already deleted: {userId}");
                return false;
            }
            catch (MongoException mongoEx)
            {
                _logger.Error("MongoDB error occurred while deleting user.", mongoEx);
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error("Error occurred while deleting user.", ex);
                throw;
            }
        }
    }

}
