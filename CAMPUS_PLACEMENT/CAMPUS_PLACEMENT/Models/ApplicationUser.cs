using Microsoft.AspNetCore.Identity;
using CampusPlacementManagement.Enums;

namespace CampusPlacementManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;

        public AccountStatus AccountStatus { get; set; }
            = AccountStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public string? RefreshTokenHash { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}