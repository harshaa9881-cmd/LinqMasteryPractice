using CampusPlacementManagement.DTOs.Auth;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface IUserService
    {
        Task<(bool Success, string Message, AuthResponseDto? Data)>
            LoginAsync(LoginDto dto);

        Task<(bool Success, string Message, AuthResponseDto? Data)>
            RefreshTokenAsync(string userId, RefreshTokenDto dto);

        Task<(bool Success, string Message)>
            ChangePasswordAsync(string userId, ChangePasswordDto dto);

        Task<(bool Success, string Message, string? ResetToken)>
            ForgotPasswordAsync(ForgotPasswordDto dto);

        Task<(bool Success, string Message)>
            ResetPasswordAsync(ResetPasswordDto dto);

        Task<CurrentUserDto?> GetCurrentUserAsync(string userId);
    }
}