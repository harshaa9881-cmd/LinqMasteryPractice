using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<College> Colleges { get; set; }

        public DbSet<PlacementOfficer> PlacementOfficers { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<CompanyHR> CompanyHRs { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<JobOpening> JobOpenings { get; set; }

        public DbSet<JobTargetCollege> JobTargetColleges { get; set; }

        public DbSet<JobApplication> JobApplications { get; set; }

        public DbSet<RecruitmentDrive> RecruitmentDrives { get; set; }

        public DbSet<InterviewRound> InterviewRounds { get; set; }

        public DbSet<InterviewSchedule> InterviewSchedules { get; set; }

        public DbSet<ApplicationStatusHistory> ApplicationStatusHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // College Configuration

            modelBuilder.Entity<College>()
                .HasIndex(c => c.CollegeCode)
                .IsUnique();

            modelBuilder.Entity<College>()
                .Property(c => c.CollegeName)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<College>()
                .Property(c => c.CollegeCode)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<College>()
                .Property(c => c.OfficialEmail)
                .IsRequired()
                .HasMaxLength(150);

            // Placement Officer Configuration

            modelBuilder.Entity<PlacementOfficer>()
                .HasIndex(p => p.UserId)
                .IsUnique();

            modelBuilder.Entity<PlacementOfficer>()
                .HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<PlacementOfficer>(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PlacementOfficer>()
                .HasOne(p => p.ApprovedByUser)
                .WithMany()
                .HasForeignKey(p => p.ApprovedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PlacementOfficer>()
                .HasOne(p => p.College)
                .WithMany(c => c.PlacementOfficers)
                .HasForeignKey(p => p.CollegeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Company Configuration

            modelBuilder.Entity<Company>()
                .Property(c => c.CompanyName)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<Company>()
                .Property(c => c.CompanyEmail)
                .IsRequired()
                .HasMaxLength(150);

            // Company HR Configuration

            modelBuilder.Entity<CompanyHR>()
                .HasIndex(h => h.UserId)
                .IsUnique();

            modelBuilder.Entity<CompanyHR>()
                .HasOne(h => h.User)
                .WithOne()
                .HasForeignKey<CompanyHR>(h => h.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CompanyHR>()
                .HasOne(h => h.ApprovedByUser)
                .WithMany()
                .HasForeignKey(h => h.ApprovedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CompanyHR>()
                .HasOne(h => h.Company)
                .WithMany(c => c.CompanyHRs)
                .HasForeignKey(h => h.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Student Configuration

            modelBuilder.Entity<Student>()
                .HasIndex(s => new { s.CollegeId, s.RegisterNumber })
                .IsUnique();

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.UserId)
                .IsUnique();

            modelBuilder.Entity<Student>()
                .Property(s => s.CGPA)
                .HasPrecision(4, 2);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)
                .WithOne()
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.College)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.CollegeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.PlacementOfficer)
                .WithMany(p => p.Students)
                .HasForeignKey(s => s.PlacementOfficerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Job Opening Configuration

            modelBuilder.Entity<JobOpening>()
                .Property(j => j.SalaryPackage)
                .HasPrecision(12, 2);

            modelBuilder.Entity<JobOpening>()
                .Property(j => j.MinimumCGPA)
                .HasPrecision(4, 2);

            modelBuilder.Entity<JobOpening>()
                .HasOne(j => j.Company)
                .WithMany(c => c.JobOpenings)
                .HasForeignKey(j => j.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobOpening>()
                .HasOne(j => j.CreatedByHR)
                .WithMany(h => h.JobOpenings)
                .HasForeignKey(j => j.CreatedByHRId)
                .OnDelete(DeleteBehavior.Restrict);

            // Job Target College Configuration

            modelBuilder.Entity<JobTargetCollege>()
                .HasIndex(j => new { j.JobId, j.CollegeId })
                .IsUnique();

            modelBuilder.Entity<JobTargetCollege>()
                .HasOne(j => j.JobOpening)
                .WithMany(j => j.JobTargetColleges)
                .HasForeignKey(j => j.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobTargetCollege>()
                .HasOne(j => j.College)
                .WithMany(c => c.JobTargetColleges)
                .HasForeignKey(j => j.CollegeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Job Application Configuration

            modelBuilder.Entity<JobApplication>()
                .HasIndex(a => new { a.StudentId, a.JobId })
                .IsUnique();

            modelBuilder.Entity<JobApplication>()
                .HasOne(a => a.JobOpening)
                .WithMany(j => j.JobApplications)
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobApplication>()
                .HasOne(a => a.Student)
                .WithMany(s => s.JobApplications)
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Recruitment Drive Configuration

            modelBuilder.Entity<RecruitmentDrive>()
                .HasOne(d => d.JobOpening)
                .WithMany(j => j.RecruitmentDrives)
                .HasForeignKey(d => d.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RecruitmentDrive>()
                .HasOne(d => d.College)
                .WithMany(c => c.RecruitmentDrives)
                .HasForeignKey(d => d.CollegeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Interview Round Configuration

            modelBuilder.Entity<InterviewRound>()
                .HasIndex(r => new { r.JobId, r.RoundNumber })
                .IsUnique();

            modelBuilder.Entity<InterviewRound>()
                .HasOne(r => r.JobOpening)
                .WithMany(j => j.InterviewRounds)
                .HasForeignKey(r => r.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            // Interview Schedule Configuration

            modelBuilder.Entity<InterviewSchedule>()
                .HasOne(i => i.JobApplication)
                .WithMany(a => a.InterviewSchedules)
                .HasForeignKey(i => i.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InterviewSchedule>()
                .HasOne(i => i.InterviewRound)
                .WithMany(r => r.InterviewSchedules)
                .HasForeignKey(i => i.InterviewRoundId)
                .OnDelete(DeleteBehavior.Restrict);

            // Application Status History Configuration

            modelBuilder.Entity<ApplicationStatusHistory>()
                .HasOne(h => h.JobApplication)
                .WithMany(a => a.StatusHistory)
                .HasForeignKey(h => h.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationStatusHistory>()
                .HasOne(h => h.UpdatedByUser)
                .WithMany()
                .HasForeignKey(h => h.UpdatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}