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
        private readonly ILogger<UserRepository> _logger;
        private readonly IEncryptionRepository _encryptionHelper;
        private readonly TokenRepository _tokenRepository;

        public UserRepository(MongoDbContext context, ILogger<UserRepository> logger, IEncryptionRepository encryptionHelper, TokenRepository tokenRepository)
        {
            _context = context;
            _logger = logger;
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
                _logger.LogWarning("Email or password is null or empty.");
                throw new ArgumentException("Email and password are required.");
            }

            if (user.IsUserConsent != true)
            {
                _logger.LogWarning("User consent is not given.");
                throw new ArgumentException("User consent is required to create an account.");
            }

            try
            {
                user.Email = _encryptionHelper.EncryptString(user.Email);
                user.Password = _encryptionHelper.EncryptString(user.Password);
                user.MobileNo = _encryptionHelper.EncryptString(user.MobileNo);
                user.CreatedAt = DateTime.UtcNow;

                await _context.Users.InsertOneAsync(user).ConfigureAwait(false);
                _logger.LogInformation($"User created successfully: {user.Email}");
                return user;
            }
            catch (MongoException mongoEx)
            {
                _logger.LogError(mongoEx, "MongoDB error occurred while creating user.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating user.");
                throw;
            }
        }

        public async Task<Users> GetUserDetails(string userId)
        {
            try
            {
                if (!ObjectId.TryParse(userId, out ObjectId objectId))
                {
                    _logger.LogWarning($"Invalid userId format: {userId}");
                    return null;
                }

                var filter = Builders<Users>.Filter.And(
                    Builders<Users>.Filter.Eq(u => u.IsDeleted, false),
                    Builders<Users>.Filter.Eq(u => u.Id, objectId)
                );

                var user = await _context.Users.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);

                if (user != null)
                {
                    user.Email = _encryptionHelper.DecryptString(user.Email);
                    user.Password = _encryptionHelper.DecryptString(user.Password);
                    user.MobileNo = _encryptionHelper.DecryptString(user.MobileNo);

                    _logger.LogInformation($"User retrieved successfully: {user.Email}");
                    return user;
                }

                _logger.LogWarning($"User not found: {userId}");
                return null;
            }
            catch (MongoException mongoEx)
            {
                _logger.LogError(mongoEx, "MongoDB error occurred while retrieving user details.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving user details.");
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
                    user.Password = _encryptionHelper.DecryptString(user.Password);
                    user.MobileNo = _encryptionHelper.DecryptString(user.MobileNo);
                }

                _logger.LogInformation($"Retrieved {users.Count} users successfully.");
                return users;
            }
            catch (MongoException mongoEx)
            {
                _logger.LogError(mongoEx, "MongoDB error occurred while retrieving all users.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all users.");
                throw;
            }
        }

        public async Task<Users> AuthenticateUser(string email, string password)
        {
            try
            {
                var user = await _context.Users.Find(u => u.Email == _encryptionHelper.EncryptString(email)
                            && u.Password == _encryptionHelper.EncryptString(password)).FirstOrDefaultAsync();

                _logger.LogInformation($"User Details: {user}");

                if (user != null && password != null)
                {
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all users.");
                throw;
            }
        }

        public async Task<bool> DeleteUser(string userId)
        {
            try
            {
                if (!ObjectId.TryParse(userId, out ObjectId objectId))
                {
                    _logger.LogWarning($"Invalid userId format: {userId}");
                    return false;
                }

                var filter = Builders<Users>.Filter.Eq(u => u.Id, objectId);

                var update = Builders<Users>.Update.Set(u => u.IsDeleted, true);

                var result = await _context.Users.UpdateOneAsync(filter, update).ConfigureAwait(false);

                if (result.ModifiedCount > 0)
                {
                    _logger.LogInformation($"User marked as deleted: {userId}");
                    return true;
                }

                _logger.LogWarning($"User not found or already deleted: {userId}");
                return false;
            }
            catch (MongoException mongoEx)
            {
                _logger.LogError(mongoEx, "MongoDB error occurred while deleting user.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user.");
                throw;
            }
        }
    }

}
