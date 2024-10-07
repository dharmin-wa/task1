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
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly TokenRepository _tokenRepository;

        public UserController(ILogger<UserController> logger, IUserService userService, TokenRepository tokenRepository)
        {
            _logger = logger;
            _userService = userService;
            _tokenRepository = tokenRepository;
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
        public async Task<IActionResult> GetAllUsers(bool isAdmin)
        {
            try
            {
                if (!isAdmin)
                {
                    return new ObjectResult(new
                    {
                        StatusCode = 403,
                        Message = "Access denied. Only admins can retrieve all users."
                    })
                    {
                        StatusCode = StatusCodes.Status403Forbidden
                    };
                }

                var users = await _userService.GetAllUsers();
                return Ok(new
                {
                    StatusCode = 200,
                    Users = users
                });
            }
            catch (Exception ex)
            {
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