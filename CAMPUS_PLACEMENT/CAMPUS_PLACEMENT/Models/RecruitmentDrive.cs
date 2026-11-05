using CampusPlacementManagement.Enums;
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.Models
{
    public class RecruitmentDrive
    {
        [Key]
        public int DriveId { get; set; }

        public int JobId { get; set; }

        public int CollegeId { get; set; }

        public DateTime DriveDate { get; set; }

        public string? Venue { get; set; }

        public string? Description { get; set; }

        public RecruitmentDriveStatus Status { get; set; }
            = RecruitmentDriveStatus.Scheduled;

        public JobOpening JobOpening { get; set; } = null!;

        public College College { get; set; } = null!;
    }
}