using CampusPlacementManagement.Enums;
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.Models
{
    public class ApplicationStatusHistory
    {
        [Key]
        public int HistoryId { get; set; }

        public int ApplicationId { get; set; }

        public ApplicationStatus? PreviousStatus { get; set; }

        public ApplicationStatus NewStatus { get; set; }

        public string UpdatedByUserId { get; set; } = string.Empty;

        public string? Remarks { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public JobApplication JobApplication { get; set; } = null!;

        public ApplicationUser UpdatedByUser { get; set; } = null!;
    }
}