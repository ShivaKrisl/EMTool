using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EfCore
{
    public class EMDbContext : DbContext
    {
        /// <summary>
        /// Employees Table
        /// </summary>
        public DbSet<Users> users { get; set; }

        /// <summary>
        /// Roles Table that describe role of employee
        /// </summary>
        public DbSet<Roles> roles { get; set; }

        /// <summary>
        /// Teams Table that describe team of employees
        /// </summary>
        public DbSet<Teams> teams { get; set; }

        /// <summary>
        /// Team Members Table that describe mapping of employees and teams
        /// </summary>
        public DbSet<TeamMembers> teamMembers { get; set; }

        /// <summary>
        /// Tasks Table that describe tasks of employees
        /// </summary>
        public DbSet<Tasks> tasks { get; set; }

        /// <summary>
        /// Task Comments Table that describe comments of tasks
        /// </summary>
        public DbSet<TaskComments> tasksComments { get; set; }

        /// <summary>
        /// Task Attachments Table that describe attachments of tasks
        /// </summary>
        public DbSet<TaskAttachments> taskAttachments { get; set; }

        /// <summary>
        /// Pull Requests Table that describe pull requests of tasks
        /// </summary>
        public DbSet<PullRequests> pullRequests { get; set; }

        /// <summary>
        /// Reviews Table that describe reviews of Pull Requests
        /// </summary>
        public DbSet<Reviews> reviews { get; set; }

        /// <summary>
        /// Notifications Table that describe notifications of employees when task is added or changed
        /// </summary>
        public DbSet<Notifications> notifications { get; set; }

        /// <summary>
        /// Scrum Meetings Table that describe scrum meetings of teams
        /// </summary>
        public DbSet<ScrumMeetings> scrumMeetings { get; set; }

        /// <summary>
        /// Scrum Attendance Table that tracks attendance of employees in scrum meetings
        /// </summary>
        public DbSet<ScrumAttendance> scrumAttendances { get; set; }

        /// <summary>
        /// Reports Table that describe reports of employees to their managers
        /// </summary>
        public DbSet<Reports> reports { get; set; }

        /// <summary>
        /// Constructor of EMDbContext
        /// </summary>
        /// <param name="dbContextOptions"></param>
        public EMDbContext(DbContextOptions<EMDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        /// <summary>
        /// OnModelCreating method that is used to define relationships between tables
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define table names
            modelBuilder.Entity<Users>().ToTable("Users");
            modelBuilder.Entity<Roles>().ToTable("Roles");
            modelBuilder.Entity<Teams>().ToTable("Teams");
            modelBuilder.Entity<TeamMembers>().ToTable("TeamMembers");
            modelBuilder.Entity<Tasks>().ToTable("Tasks");
            modelBuilder.Entity<TaskComments>().ToTable("TaskComments");
            modelBuilder.Entity<TaskAttachments>().ToTable("TaskAttachments");
            modelBuilder.Entity<PullRequests>().ToTable("PullRequests");
            modelBuilder.Entity<Reviews>().ToTable("Reviews");
            modelBuilder.Entity<Notifications>().ToTable("Notifications");
            modelBuilder.Entity<ScrumMeetings>().ToTable("ScrumMeetings");
            modelBuilder.Entity<ScrumAttendance>().ToTable("ScrumAttendance");
            modelBuilder.Entity<Reports>().ToTable("Reports");

            // Users & Roles (One-to-Many)
            modelBuilder.Entity<Users>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Users & Teams (One-to-Many via TeamMembers)
            modelBuilder.Entity<TeamMembers>()
                .HasOne(tm => tm.users)
                .WithMany(u => u.TeamMembers)
                .HasForeignKey(tm => tm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TeamMembers>()
                .HasOne(tm => tm.teams)
                .WithMany(t => t.TeamMembers)
                .HasForeignKey(tm => tm.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            // Tasks & Users (One-to-Many: Assigned to a user)
            modelBuilder.Entity<Tasks>()
                .HasOne(t => t.Assignee)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.AssignedTo)
                .OnDelete(DeleteBehavior.Restrict);

            // Task Comments & Tasks (One-to-Many)
            modelBuilder.Entity<TaskComments>()
                .HasOne(tc => tc.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(tc => tc.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            // Task Attachments & Tasks (One-to-Many)
            modelBuilder.Entity<TaskAttachments>()
                .HasOne(ta => ta.tasks)
                .WithMany(t => t.Attachments)
                .HasForeignKey(ta => ta.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            // Pull Requests & Tasks (One-to-Many)
            modelBuilder.Entity<PullRequests>()
                .HasOne(pr => pr.task)
                .WithMany(t => t.PullRequests)
                .HasForeignKey(pr => pr.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            // Reviews & PRs (One-to-Many)
            modelBuilder.Entity<Reviews>()
                .HasOne(r => r.pullRequests)
                .WithMany(pr => pr.reviews)
                .HasForeignKey(r => r.PRId)
                .OnDelete(DeleteBehavior.Cascade);

            // Notifications & Users (One-to-Many)
            modelBuilder.Entity<Notifications>()
                .HasOne(n => n.user)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Scrum Meetings & Teams (One-to-Many)
            modelBuilder.Entity<ScrumMeetings>()
                .HasOne(sm => sm.team)
                .WithMany(t => t.ScrumMeetings)
                .HasForeignKey(sm => sm.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            // Scrum Attendance & Scrum Meetings (One-to-Many)
            modelBuilder.Entity<ScrumAttendance>()
                .HasOne(sa => sa.scrumMeeting)
                .WithMany(sm => sm.scrumAttendances)
                .HasForeignKey(sa => sa.MeetingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Scrum Attendance & Users (One-to-Many)
            modelBuilder.Entity<ScrumAttendance>()
                .HasOne(sa => sa.user)
                .WithMany(u => u.ScrumAttendances)
                .HasForeignKey(sa => sa.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Reports & Users (One-to-Many)
            modelBuilder.Entity<Reports>()
                .HasOne(r => r.Employee)
                .WithMany(u => u.SubmittedReports)
                .HasForeignKey(r => r.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reports>()
                .HasOne(r => r.Manager)
                .WithMany(u => u.ReceivedReports)
                .HasForeignKey(r => r.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        }



    }
}
