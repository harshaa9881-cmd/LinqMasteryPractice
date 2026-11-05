using Microsoft.AspNetCore.Identity;
using CampusPlacementManagement.DTOs.Auth;
using CampusPlacementManagement.Enums;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace CampusPlacementManagement.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        public UserService(
            IUserRepository userRepository,
            UserManager<ApplicationUser> userManager,
            IJwtService jwtService,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _jwtService = jwtService;
            _configuration = configuration;
        }

        public async Task<(bool Success, string Message, AuthResponseDto? Data)>
            LoginAsync(LoginDto dto)
        {
            var email = dto.Email.Trim().ToLowerInvariant();

            var user = await _userRepository.GetByEmailAsync(email);

            if (user is null)
            {
                return (false, "Invalid email or password.", null);
            }

            if (user.AccountStatus != AccountStatus.Approved)
            {
                return (false, "Your account is not approved or active.", null);
            }

            var passwordValid = await _userManager.CheckPasswordAsync(
                user,
                dto.Password
            );

            if (!passwordValid)
            {
                return (false, "Invalid email or password.", null);
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Count == 0)
            {
                return (false, "No role is assigned to this account.", null);
            }

            return await GenerateAuthResponseAsync(user, roles);
        }

        public async Task<(bool Success, string Message, AuthResponseDto? Data)>
            RefreshTokenAsync(string userId, RefreshTokenDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user is null ||
                user.AccountStatus != AccountStatus.Approved)
            {
                return (false, "Invalid account.", null);
            }

            if (string.IsNullOrWhiteSpace(user.RefreshTokenHash) ||
                user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return (false, "Refresh token has expired.", null);
            }

            var refreshTokenHash = HashRefreshToken(dto.RefreshToken);

            if (user.RefreshTokenHash != refreshTokenHash)
            {
                return (false, "Invalid refresh token.", null);
            }

            var roles = await _userManager.GetRolesAsync(user);

            return await GenerateAuthResponseAsync(user, roles);
        }

        public async Task<(bool Success, string Message)>
            ChangePasswordAsync(string userId, ChangePasswordDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user is null)
            {
                return (false, "User not found.");
            }

            if (user.AccountStatus != AccountStatus.Approved)
            {
                return (false, "Account is not active.");
            }

            var result = await _userManager.ChangePasswordAsync(
                user,
                dto.CurrentPassword,
                dto.NewPassword
            );

            if (!result.Succeeded)
            {
                return (
                    false,
                    string.Join(" ", result.Errors.Select(e => e.Description))
                );
            }

            user.UpdatedAt = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return (true, "Password changed successfully.");
        }

        public async Task<(bool Success, string Message, string? ResetToken)>
            ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var email = dto.Email.Trim().ToLowerInvariant();

            var user = await _userRepository.GetByEmailAsync(email);

            if (user is null ||
                user.AccountStatus != AccountStatus.Approved)
            {
                return (
                    true,
                    "If the account exists, password reset instructions will be provided.",
                    null
                );
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return (
                true,
                "If the account exists, password reset instructions will be provided.",
                token
            );
        }

        public async Task<(bool Success, string Message)>
            ResetPasswordAsync(ResetPasswordDto dto)
        {
            var email = dto.Email.Trim().ToLowerInvariant();

            var user = await _userRepository.GetByEmailAsync(email);

            if (user is null ||
                user.AccountStatus != AccountStatus.Approved)
            {
                return (false, "Invalid password reset request.");
            }

            var result = await _userManager.ResetPasswordAsync(
                user,
                dto.Token,
                dto.NewPassword
            );

            if (!result.Succeeded)
            {
                return (
                    false,
                    string.Join(" ", result.Errors.Select(e => e.Description))
                );
            }

            user.UpdatedAt = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return (true, "Password reset successfully.");
        }

        public async Task<CurrentUserDto?> GetCurrentUserAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user is null)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);

            return new CurrentUserDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                Roles = roles.ToList(),
                AccountStatus = user.AccountStatus
            };
        }

        private async Task<(
            bool Success,
            string Message,
            AuthResponseDto? Data
        )> GenerateAuthResponseAsync(
            ApplicationUser user,
            IList<string> roles)
        {
            var (accessToken, expiresAt) =
                _jwtService.GenerateToken(user, roles);

            var refreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshTokenHash = HashRefreshToken(refreshToken);

            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(
                _configuration.GetValue<int>("Jwt:RefreshTokenExpiryDays")
            );

            user.UpdatedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return (false, "Unable to update authentication details.", null);
            }

            var response = new AuthResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                Roles = roles.ToList(),
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = expiresAt
            };

            return (true, "Authentication successful.", response);
        }

        private static string HashRefreshToken(string token)
        {
            var bytes = SHA256.HashData(
                Encoding.UTF8.GetBytes(token)
            );

            return Convert.ToBase64String(bytes);
        }
    }
}