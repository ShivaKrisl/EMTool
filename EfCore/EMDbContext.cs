using System;
using Microsoft.EntityFrameworkCore;

namespace EfCore
{
    public class EMDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Work> Tasks { get; set; }
        public DbSet<WorkComment> TaskComments { get; set; }
        public DbSet<WorkAttachment> TaskAttachments { get; set; }
        public DbSet<PullRequest> PullRequests { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ScrumMeeting> ScrumMeetings { get; set; }
        public DbSet<ScrumAttendance> ScrumAttendances { get; set; }
        public DbSet<Report> Reports { get; set; }

        public EMDbContext(DbContextOptions<EMDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Explicitly setting table names
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<Team>().ToTable("Teams");
            modelBuilder.Entity<TeamMember>().ToTable("TeamMembers");
            modelBuilder.Entity<Work>().ToTable("Tasks");
            modelBuilder.Entity<WorkComment>().ToTable("TaskComments");
            modelBuilder.Entity<WorkAttachment>().ToTable("TaskAttachments");
            modelBuilder.Entity<PullRequest>().ToTable("PullRequests");
            modelBuilder.Entity<Review>().ToTable("Reviews");
            modelBuilder.Entity<Notification>().ToTable("Notifications");
            modelBuilder.Entity<ScrumMeeting>().ToTable("ScrumMeetings");
            modelBuilder.Entity<ScrumAttendance>().ToTable("ScrumAttendance");
            modelBuilder.Entity<Report>().ToTable("Reports");

            // Relationships
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeamMember>()
                .HasOne(tm => tm.User)
                .WithMany(u => u.TeamMembers)
                .HasForeignKey(tm => tm.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeamMember>()
                .HasOne(tm => tm.Team)
                .WithMany(t => t.TeamMembers)
                .HasForeignKey(tm => tm.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Work>()
                .HasOne(t => t.Assignee)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.AssignedTo)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WorkComment>()
                .HasOne(tc => tc.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(tc => tc.TaskId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WorkAttachment>()
                .HasOne(ta => ta.Task)
                .WithMany(t => t.Attachments) // Ensure Work has a collection: public ICollection<WorkAttachment> Attachments { get; set; }
                .HasForeignKey(ta => ta.TaskId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WorkAttachment>()
                .HasOne(ta => ta.User)
                .WithMany(u => u.workAttachments) // Ensure User has a collection: public ICollection<WorkAttachment> Attachments { get; set; }
                .HasForeignKey(ta => ta.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Fix the typo

            modelBuilder.Entity<PullRequest>()
                .HasOne(pr => pr.Task)
                .WithMany(t => t.PullRequests)
                .HasForeignKey(pr => pr.TaskId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<PullRequest>()
                .HasMany(pr => pr.Reviews)
                .WithOne(r => r.PullRequest)
                .HasForeignKey(r => r.PullRequestId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Reviewer)
                .WithMany(u => u.ReviewsGiven)
                .HasForeignKey(r => r.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ScrumMeeting>()
                .HasOne(sm => sm.Team)
                .WithMany(t => t.ScrumMeetings)
                .HasForeignKey(sm => sm.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ScrumAttendance>()
                .HasOne(sa => sa.ScrumMeeting)
                .WithMany(sm => sm.ScrumAttendances)
                .HasForeignKey(sa => sa.MeetingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ScrumAttendance>()
                .HasOne(sa => sa.User)
                .WithMany(u => u.ScrumAttendances)
                .HasForeignKey(sa => sa.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevents deleting Users with Attendance records

            modelBuilder.Entity<Report>()
                .HasOne(r => r.SubmittedBy)
                .WithMany(u => u.SubmittedReports)
                .HasForeignKey(r => r.SubmittedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.ReviewedBy)
                .WithMany(u => u.ReceivedReports)
                .HasForeignKey(r => r.ReviewedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
