using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using user_GDPR.Models;
using user_GDPR.Repositories;
using user_GDPR.Services;
using user_GDPR.Services.Interface;

namespace user_GDPR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly TokenRepository _tokenRepository;
        private readonly ILog _logger;

        public UserController(IUserService userService, TokenRepository tokenRepository)
        {
            _userService = userService;
            _tokenRepository = tokenRepository;
            _logger = LogManager.GetLogger(typeof(UserRepository));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] Users user)
        {
            try
            {
                var userData = await _userService.CreateUser(user);
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "User created successfully.",
                    User = userData
                });
            }
            catch (Exception ex)
            {
                _logger.Error("create: " + ex.Message);
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An error occurred while creating the user.",
                    Details = ex.Message
                });
            }
        }

        [Authorize]
        [HttpGet("get-detail")]
        public async Task<IActionResult> GetUserDetails(string userId)
        {
            try
            {
                var user = await _userService.GetUserDetails(userId);
                if (user == null)
                {
                    _logger.Error("get-detail: User not found.");
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "User not found."
                    });
                }

                return Ok(new
                {
                    StatusCode = 200,
                    User = user
                });
            }
            catch (Exception ex)
            {
                _logger.Error("get-detail: " + ex.Message);
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An error occurred while retrieving user details.",
                    Details = ex.Message
                });
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                _logger.Error("Login: Email and password are required.");
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Email and password are required."
                });
            }

            try
            {
                var user = await _userService.AuthenticateUser(loginRequest.Email, loginRequest.Password);

                if (user == null)
                {
                    _logger.Error("Login: Invalid email or password.");
                    return Unauthorized(new
                    {
                        StatusCode = 401,
                        Message = "Invalid email or password."
                    });
                }

                var token = _tokenRepository.GenerateToken(user);

                return Ok(new
                {
                    StatusCode = 200,
                    Token = token,
                    User = new
                    {
                        user.Id,
                        user.FirstName,
                        user.lastName,
                        user.Email,
                        user.IsAdmin
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.Error("Login: " + ex.Message);
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An error occurred during login.",
                    Details = ex.Message
                });
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return Ok(new
                {
                    StatusCode = 200,
                    Users = users
                });
            }
            catch (Exception ex)
            {
                _logger.Error("get-all: " + ex.Message);
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An error occurred while retrieving users.",
                    Details = ex.Message
                });
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("delete-user")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                var isDeleted = await _userService.DeleteUser(userId);
                if (!isDeleted)
                {
                    _logger.Error("delete-user: User not found or already deleted.");
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "User not found or already deleted."
                    });
                }

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "User successfully deleted."
                });
            }
            catch (Exception ex)
            {
                _logger.Error("delete-user: " + ex.Message);
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An error occurred while deleting the user.",
                    Details = ex.Message
                });
            }
        }


    }
}