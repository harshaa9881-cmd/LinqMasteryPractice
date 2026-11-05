using CampusPlacementManagement.Enums;

namespace CampusPlacementManagement.Models
{
    public class PlacementOfficer
    {
        public int PlacementOfficerId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public int CollegeId { get; set; }

        public string Designation { get; set; } = string.Empty;

        public RecordStatus Status { get; set; } = RecordStatus.Pending;

        public string? ApprovedBy { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public ApplicationUser User { get; set; } = null!;

        public ApplicationUser? ApprovedByUser { get; set; }

        public College College { get; set; } = null!;

        public ICollection<Student> Students { get; set; }
            = new List<Student>();
    }
}