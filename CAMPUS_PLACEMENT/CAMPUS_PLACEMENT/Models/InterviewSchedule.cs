using CampusPlacementManagement.Enums;

namespace CampusPlacementManagement.Models
{
    public class InterviewSchedule
    {
        public int InterviewScheduleId { get; set; }

        public int ApplicationId { get; set; }

        public int InterviewRoundId { get; set; }

        public DateTime ScheduledAt { get; set; }

        public string? MeetingLink { get; set; }

        public string? Venue { get; set; }

        public InterviewScheduleStatus Status { get; set; }
            = InterviewScheduleStatus.Scheduled;

        public string? Feedback { get; set; }

        public JobApplication JobApplication { get; set; } = null!;

        public InterviewRound InterviewRound { get; set; } = null!;
    }
}