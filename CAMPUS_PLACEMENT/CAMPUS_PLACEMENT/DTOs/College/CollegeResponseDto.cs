using CampusPlacementManagement.Enums;

namespace CampusPlacementManagement.DTOs.College
{
    public class CollegeResponseDto
    {
        public int CollegeId { get; set; }

        public string CollegeName { get; set; } = string.Empty;

        public string CollegeCode { get; set; } = string.Empty;

        public string? UniversityName { get; set; }

        public string OfficialEmail { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string? Website { get; set; }

        public RecordStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}