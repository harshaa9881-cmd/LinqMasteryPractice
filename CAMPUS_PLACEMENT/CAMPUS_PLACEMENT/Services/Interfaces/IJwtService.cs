using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface IJwtService
    {
        (string Token, DateTime ExpiresAt) GenerateToken(
            ApplicationUser user,
            IList<string> roles
        );

        string GenerateRefreshToken();
    }
}