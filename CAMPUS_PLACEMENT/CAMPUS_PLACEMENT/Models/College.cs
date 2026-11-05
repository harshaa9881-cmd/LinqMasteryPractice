using CampusPlacementManagement.Enums;

namespace CampusPlacementManagement.Models
{
    public class College
    {
        public int CollegeId { get; set; }

        public string CollegeName { get; set; } = string.Empty;

        public string CollegeCode { get; set; } = string.Empty;

        public string? UniversityName { get; set; }

        public string OfficialEmail { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string? Website { get; set; }

        public RecordStatus Status { get; set; } = RecordStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<PlacementOfficer> PlacementOfficers { get; set; }
            = new List<PlacementOfficer>();

        public ICollection<Student> Students { get; set; }
            = new List<Student>();

        public ICollection<JobTargetCollege> JobTargetColleges { get; set; }
            = new List<JobTargetCollege>();

        public ICollection<RecruitmentDrive> RecruitmentDrives { get; set; }
            = new List<RecruitmentDrive>();
    }
}