using CampusPlacementManagement.Enums;

namespace CampusPlacementManagement.DTOs.Auth
{
    public class CurrentUserDto
    {
        public string UserId { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public List<string> Roles { get; set; } = new();

        public AccountStatus AccountStatus { get; set; }
    }
}