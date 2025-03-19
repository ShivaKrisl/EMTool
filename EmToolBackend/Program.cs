using EfCore;
using Microsoft.EntityFrameworkCore;
using Service_Contracts;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EMDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Register Services
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ITeamMemberService, TeamMemberService>();
builder.Services.AddScoped<IWorkService, WorkService>();
builder.Services.AddScoped<IWorkCommentService, WorkCommentService>();
builder.Services.AddScoped<IWorkAttachmentService, WorkAttachmentService>();
builder.Services.AddScoped<IPullRequestService, PullRequestService>();
builder.Services.AddScoped<IPRReviewService, PRReviewService>();
builder.Services.AddScoped<IScrumMeetingService, ScrumMeetingService>();
builder.Services.AddScoped<IScrumAttendanceService, ScrumAttendanceService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.Run();
