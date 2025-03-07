using DTOs.PullRequests;
using DTOs.Reports;
using DTOs.Tasks;
using DTOs.TeamMembers;
using DTOs.ScrumMeetings;
using Service_Contracts;
using EfCore;
using System.Threading.Tasks;
using DTOs.Users;

namespace Services
{
    public class ReportService : IReportService
    {

        private readonly ITeamService _teamService;
        private readonly IWorkService _workService;
        private readonly IPullRequestService _prService;
        private readonly IUserService _userService;
        private readonly IScrumMeetingService _scrumMeetingService;
        private readonly ITeamMemberService _teamMemberService;

        public ReportService(ITeamService teamService, IWorkService workService, IPullRequestService prService, IUserService userService, IScrumMeetingService scrumMeetingService, ITeamMemberService teamMemberService)
        {
            _teamService = teamService;
            _workService = workService;
            _prService = prService;
            _userService = userService;
            _scrumMeetingService = scrumMeetingService;
            _teamMemberService = teamMemberService;
        }

        /// <summary>
        /// Get overall team performance report within a specific time range
        /// </summary>
        /// <param name="reportRequest"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task<ReportResponse> GetTeamReport(ReportRequest reportRequest)
        {
            if(reportRequest == null)
            {
                throw new ArgumentNullException(nameof(reportRequest));
            }

            bool isValidModel = ValidationHelper.IsStateValid(reportRequest);

            if (!isValidModel)
            {
                throw new ArgumentException("Invalid model state");
            }

            Guid teamId = reportRequest.TeamId;
            DateTime startDate = reportRequest.StartDate;
            DateTime endDate = reportRequest.EndDate;

            if(endDate < startDate)
            {
                throw new ArgumentException("End date cannot be earlier than start date");
            }

            List<TeamMemberResponse>? teamMembers = await _teamMemberService.GetAllTeamMembers(teamId);

            if (teamMembers == null || teamMembers.Count == 0)
            {
                throw new ArgumentException("No team members found");
            }
            // Get all Tasks of team
           List<WorkResponse>? workResponses =  await _workService.GetAllWorksOfTeam(teamId);


            // Get all PRs of team
            List<PREntityResponse>? pREntityResponses =  await _prService.GetAllPullRequestsOfTeam(teamId);

           
            // Get all scrum meetings of Team
            List<ScrumMeetingResponse>? scrumMeetingResponses = await _scrumMeetingService.GetScrumMeetingsOfTeam(teamId);

            int completedTasks = workResponses.Count(w => w.Status == WorkStatus.Completed.ToString() && w.CreatedAt >= startDate && w.CreatedAt <= endDate);

            int pendingTasks = workResponses.Count(w => w.Status != WorkStatus.Completed.ToString() && w.CreatedAt >= startDate && w.CreatedAt <= endDate);

            int totalPRs = 0;
            int mergedPRs = 0;

            if (pREntityResponses == null)
            {
                totalPRs = 0;
                mergedPRs = 0;
            }

            else
            {
                totalPRs = pREntityResponses.Count;
                mergedPRs = pREntityResponses.Count(p => p.PRStatus == PRStatus.Approved.ToString());
            }

            return new ReportResponse
            {
                
                TeamId = teamId,
                TotalTasksAssigned = workResponses.Count,
                CompletedTasks = completedTasks,
                PendingTasks = pendingTasks,
                TotalPRsRaised = totalPRs,
                MergedPRs = mergedPRs,
                MeetingsHeld = scrumMeetingResponses!=null ? scrumMeetingResponses.Count : 0,
                TopPerformers = await GetTopPerformers(teamId, startDate, endDate)
            };

        }

        /// <summary>
        /// Get an individual employee's performance report
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task<EmployeePerformance> GetEmployeeReport(Guid employeeId, DateTime startDate, DateTime endDate)
        {
            if(employeeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(employeeId));
            }

            if (startDate <= endDate)
            {
                throw new ArgumentException("Invalid Range");
            }

            UserResponse userResponse = await _userService.GetEmployeeById(employeeId);

            if (userResponse == null) {
                throw new ArgumentException("Employee not found");
            }

           List<WorkResponse>? workResponses = await _workService.GetEmployeeWorks(employeeId);

            List<PREntityResponse>? pREntityResponses = await _prService.GetPullRequestsOfUser(employeeId);

            List<ScrumMeetingResponse>? scrumMeetingResponses = await _scrumMeetingService.GetScrumMeetingsOfUser(employeeId);

            int assignedTasks = 0;
            int completedTasks = 0;
            int PRsRaised = 0;
            int PRsMerged = 0;
            int meetingsAttended = 0;

            if (workResponses == null)
            {
                assignedTasks = 0;
                completedTasks = 0;
            }

            else {
                assignedTasks = workResponses.Count;
                completedTasks = workResponses.Count(w => w.Status == WorkStatus.Completed.ToString() && w.CreatedAt >= startDate && w.CreatedAt <= endDate);
            }

            if (pREntityResponses == null)
            {
                PRsRaised = 0;
                PRsMerged = 0;
            }
            else
            {
                PRsRaised = pREntityResponses.Count;
                PRsMerged = pREntityResponses.Count(p => p.PRStatus == PRStatus.Approved.ToString());
            }

            if (scrumMeetingResponses == null)
            {
                meetingsAttended = 0;
            }
            else
            {
                meetingsAttended = scrumMeetingResponses.Count;
            }

            return new EmployeePerformance
            {
                EmployeeId = employeeId,
                EmployeeName = userResponse.Username,
                TasksCompleted = completedTasks,
                PRsMerged = PRsMerged,
                MeetingsAttended = meetingsAttended
            };
        }

        /// <summary>
        /// Get top-performing employees based on completed tasks and merged PRs
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task<List<EmployeePerformance>> GetTopPerformers(Guid teamId, DateTime startDate, DateTime endDate)
        {
            if(teamId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(teamId));
            }

            if (startDate <= endDate)
            {
                throw new ArgumentException("Invalid Range");
            }
            List<UserResponse>? userResponses =   await _userService.GetAllEmployees();
            if (userResponses == null)
            {
                throw new ArgumentException("No employees found");
            }

            List<EmployeePerformance> employeePerformances = new List<EmployeePerformance>();

            foreach (UserResponse userResponse in userResponses)
            {
                EmployeePerformance employeePerformance = await GetEmployeeReport(userResponse.Id, startDate, endDate);
                employeePerformances.Add(employeePerformance);
            }

            return employeePerformances
                .OrderByDescending(emp => emp.TasksCompleted + emp.PRsMerged)
                .ToList();
        }
    }
}
