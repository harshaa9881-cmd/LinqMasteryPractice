using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CampusPlacementManagement.DTOs.Auth;
using CampusPlacementManagement.Services.Interfaces;
using System.Security.Claims;

namespace CampusPlacementManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _environment;

        public AuthController(
            IUserService userService,
            IWebHostEnvironment environment)
        {
            _userService = userService;
            _environment = environment;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _userService.LoginAsync(dto);

            if (!result.Success)
            {
                return Unauthorized(new
                {
                    message = result.Message
                });
            }

            return Ok(new
            {
                message = result.Message,
                data = result.Data
            });
        }

        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var result = await _userService.RefreshTokenAsync(
                userId,
                dto
            );

            if (!result.Success)
            {
                return Unauthorized(new
                {
                    message = result.Message
                });
            }

            return Ok(new
            {
                message = result.Message,
                data = result.Data
            });
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(
            ChangePasswordDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var result = await _userService.ChangePasswordAsync(
                userId,
                dto
            );

            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message
                });
            }

            return Ok(new
            {
                message = result.Message
            });
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(
            ForgotPasswordDto dto)
        {
            var result = await _userService.ForgotPasswordAsync(dto);

            return Ok(new
            {
                message = result.Message,
                resetToken = _environment.IsDevelopment()
                    ? result.ResetToken
                    : null
            });
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(
            ResetPasswordDto dto)
        {
            var result = await _userService.ResetPasswordAsync(dto);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message
                });
            }

            return Ok(new
            {
                message = result.Message
            });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var user = await _userService.GetCurrentUserAsync(userId);

            if (user is null)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }

            return Ok(user);
        }
    }
}