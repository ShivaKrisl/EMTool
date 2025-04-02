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

            // 🔹 User - Role (Restrict Role Deletion)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 User - TeamMembers (Cascade Delete)
            modelBuilder.Entity<TeamMember>()
                .HasOne(tm => tm.User)
                .WithMany(u => u.TeamMembers)
                .HasForeignKey(tm => tm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TeamMember>()
            .HasOne(tm => tm.AddedBy)
            .WithMany(u => u.AddedTeamMembers)  // Use the new collection in User
            .HasForeignKey(tm => tm.AddedById)
            .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Team - TeamMembers (Restrict Team Deletion)
            modelBuilder.Entity<TeamMember>()
                .HasOne(tm => tm.Team)
                .WithMany(t => t.TeamMembers)
                .HasForeignKey(tm => tm.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 User - Notifications (Cascade Delete)
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Task - AssignedTo User (Restrict Delete to prevent data loss)
            modelBuilder.Entity<Work>()
                .HasOne(t => t.Assignee)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.AssignedTo)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WorkComment>()
     .HasOne(tc => tc.Task)
     .WithMany(t => t.Comments)
     .HasForeignKey(tc => tc.TaskId)
     .OnDelete(DeleteBehavior.Cascade); // ✅ Task deletion will delete comments

            modelBuilder.Entity<WorkComment>()
                .HasOne(tc => tc.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(tc => tc.UserId)
                .OnDelete(DeleteBehavior.Restrict); // ✅ Prevents multiple cascade paths


            // 🔹 Task - Work Attachments (Cascade Delete)
            modelBuilder.Entity<WorkAttachment>()
                .HasOne(ta => ta.Task)
                .WithMany(t => t.Attachments)
                .HasForeignKey(ta => ta.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 WorkAttachment - User (Restrict Delete to prevent orphaned attachments)
            modelBuilder.Entity<WorkAttachment>()
                .HasOne(ta => ta.User)
                .WithMany(u => u.workAttachments)
                .HasForeignKey(ta => ta.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Task - Pull Requests (Cascade Delete)
            modelBuilder.Entity<PullRequest>()
                .HasOne(pr => pr.Task)
                .WithMany(t => t.PullRequests)
                .HasForeignKey(pr => pr.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            // ❌ Fix: PullRequest - CreatedBy User (Restrict to avoid multiple cascade paths)
            modelBuilder.Entity<PullRequest>()
                .HasOne(pr => pr.CreatedBy)
                .WithMany(u => u.PullRequests)
                .HasForeignKey(pr => pr.CreatedById)
                .OnDelete(DeleteBehavior.Restrict); // ✅ Prevents multiple cascade paths error

            // 🔹 Pull Request - Reviews (Cascade Delete)
            modelBuilder.Entity<PullRequest>()
                .HasMany(pr => pr.Reviews)
                .WithOne(r => r.PullRequest)
                .HasForeignKey(r => r.PullRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Review - User (Restrict Delete to keep review history)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Reviewer)
                .WithMany(u => u.ReviewsGiven)
                .HasForeignKey(r => r.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 ScrumMeeting - Team (Restrict Delete to prevent orphaned records)
            modelBuilder.Entity<ScrumMeeting>()
                .HasOne(sm => sm.Team)
                .WithMany(t => t.ScrumMeetings)
                .HasForeignKey(sm => sm.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 ScrumMeeting - ScrumAttendance (Cascade Delete)
            modelBuilder.Entity<ScrumAttendance>()
                .HasOne(sa => sa.ScrumMeeting)
                .WithMany(sm => sm.ScrumAttendances)
                .HasForeignKey(sa => sa.MeetingId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 ScrumAttendance - User (Restrict Delete to avoid orphaned attendance records)
            modelBuilder.Entity<ScrumAttendance>()
                .HasOne(sa => sa.User)
                .WithMany(u => u.ScrumAttendances)
                .HasForeignKey(sa => sa.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
