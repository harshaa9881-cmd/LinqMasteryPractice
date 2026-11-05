using CampusPlacementManagement.Enums;

namespace CampusPlacementManagement.DTOs.CompanyHR
{
    public class CompanyHRResponseDto
    {
        public int CompanyHRId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public int CompanyId { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public string? Designation { get; set; }

        public string? ApprovedBy { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public AccountStatus Status { get; set; }
    }
}