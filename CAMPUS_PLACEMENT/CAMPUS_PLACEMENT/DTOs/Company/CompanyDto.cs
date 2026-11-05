using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Company
{
    public class CompanyDto
    {
        [Required]
        [MaxLength(200)]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string CompanyEmail { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Website { get; set; }

        [Required]
        [MaxLength(100)]
        public string Industry { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Address { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}