using CampusPlacementManagement.Enums;
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.Models
{
    public class JobApplication
    {
        [Key]
        public int ApplicationId { get; set; }

        public int JobId { get; set; }

        public int StudentId { get; set; }

        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

        public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;

        public string? ResumeUrl { get; set; }

        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

        public JobOpening JobOpening { get; set; } = null!;

        public Student Student { get; set; } = null!;

        public ICollection<InterviewSchedule> InterviewSchedules { get; set; }
            = new List<InterviewSchedule>();

        public ICollection<ApplicationStatusHistory> StatusHistory { get; set; }
            = new List<ApplicationStatusHistory>();
    }
}