using CampusPlacementManagement.Enums;
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.Models
{
    public class JobOpening
    {
        [Key]
        public int JobId { get; set; }

        public int CompanyId { get; set; }

        public int CreatedByHRId { get; set; }

        public string JobTitle { get; set; } = string.Empty;

        public string JobDescription { get; set; } = string.Empty;

        public JobType JobType { get; set; }

        public string Location { get; set; } = string.Empty;

        public decimal? SalaryPackage { get; set; }

        public decimal? MinimumCGPA { get; set; }

        public int? MaximumBacklogs { get; set; }

        public DateTime ApplicationDeadline { get; set; }

        public bool IsOpenToAllColleges { get; set; }

        public JobStatus Status { get; set; } = JobStatus.Draft;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Company Company { get; set; } = null!;

        public CompanyHR CreatedByHR { get; set; } = null!;

        public ICollection<JobTargetCollege> JobTargetColleges { get; set; }
            = new List<JobTargetCollege>();

        public ICollection<JobApplication> JobApplications { get; set; }
            = new List<JobApplication>();

        public ICollection<RecruitmentDrive> RecruitmentDrives { get; set; }
            = new List<RecruitmentDrive>();

        public ICollection<InterviewRound> InterviewRounds { get; set; }
            = new List<InterviewRound>();
    }
}