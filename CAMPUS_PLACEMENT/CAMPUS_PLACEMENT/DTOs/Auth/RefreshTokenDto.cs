using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Auth
{
    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}