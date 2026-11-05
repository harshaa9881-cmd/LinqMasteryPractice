using CampusPlacementManagement.Enums;

namespace CampusPlacementManagement.DTOs.Company
{
    public class CompanyResponseDto
    {
        public int CompanyId { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public string CompanyEmail { get; set; } = string.Empty;

        public string? Website { get; set; }

        public string Industry { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string? Description { get; set; }

        public RecordStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}