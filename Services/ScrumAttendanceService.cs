using DTOs.ScrumAttendances;
using EfCore;
using Service_Contracts;
using DTOs.ScrumMeetings;
using DTOs.Users;
using DTOs.Roles;
using DTOs.TeamMembers;
using DTOs.Teams;

namespace Services
{
    public class ScrumAttendanceService : IScrumAttendanceService
    {

        private readonly List<ScrumAttendance> _scrumAttendances;
        private readonly IScrumMeetingService _scrumMeetingService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly ITeamMemberService _teamMemberService;
        private readonly ITeamService _teamService;

        public ScrumAttendanceService(IScrumMeetingService scrumMeetingService, IUserService userService, IRoleService roleService, ITeamMemberService teamMemberService, ITeamService teamService)
        {
            _scrumAttendances = new List<ScrumAttendance>();
            _scrumMeetingService = scrumMeetingService;
            _userService = userService;
            _roleService = roleService;
            _teamMemberService = teamMemberService;
            _teamService = teamService;
        }

        /// <summary>
        /// Create a new Scrum Attendance
        /// </summary>
        /// <param name="scrumAttendanceRequest"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task<ScrumAttendanceResponse> MarkAttendance(ScrumAttendanceRequest scrumAttendanceRequest)
        {
            if(scrumAttendanceRequest == null)
            {
                throw new ArgumentNullException(nameof(scrumAttendanceRequest));
            }

            bool isValidModel = ValidationHelper.IsStateValid(scrumAttendanceRequest);
            if (!isValidModel)
            {
                throw new ArgumentException("Invalid Model State");
            }

            // Check meeting exists
            ScrumMeetingResponse? scrumMeeting = await _scrumMeetingService.GetScrumMeetingById(scrumAttendanceRequest.MeetingId);

            if (scrumMeeting == null)
            {
                throw new ArgumentException("Meeting not found");
            }

            // Check meeting is in past
            if (scrumMeeting.ScheduledDate > DateTime.UtcNow)
            {
                throw new ArgumentException("You cannot mark attendance for a future meeting.");
            }

            // Check user exists
            UserResponse? userResponse = await _userService.GetEmployeeById(scrumAttendanceRequest.UserId);

            if (userResponse == null)
            {
                throw new ArgumentException("User not found");
            }

            RoleResponse? roleResponse = await _roleService.GetRoleById(userResponse.RoleId);
            if(roleResponse == null)
            {
                throw new ArgumentException("Role not found");
            }

            // check user is a part of same team

            TeamResponse teamResponse = await _teamService.GetTeamById(scrumMeeting.TeamId);
            if (teamResponse == null)
            {
                throw new ArgumentException("User is not part of Team!");
            }

            UserResponse? managerUserResponse = await _userService.GetEmployeeById(teamResponse.ManagerId);

            if (managerUserResponse == null) {
                throw new ArgumentException("Manager not found");
            }

            RoleResponse? managerRole = await _roleService.GetRoleById(managerUserResponse.RoleId);
            if (managerRole == null) {
                throw new ArgumentException("Manager Role not found");
            }

            TeamMemberResponse? teamMemberResponse = await _teamMemberService.GetTeamMemberByUserId(scrumAttendanceRequest.UserId);

            if(teamMemberResponse == null || teamMemberResponse.TeamId != teamResponse.Id)
            {
                throw new ArgumentException("User is not part of Team!");
            }

            // Check if user has already marked attendance
            bool alreadyMarked = _scrumAttendances.Any(sa => sa.MeetingId == scrumAttendanceRequest.MeetingId && sa.UserId == scrumAttendanceRequest.UserId);
            if (alreadyMarked)
            {
                throw new ArgumentException("User has already marked attendance for this meeting.");
            }

            ScrumAttendance scrumAttendance = scrumAttendanceRequest.ToScrumAttendance();
            scrumAttendance.Id = Guid.NewGuid();

            scrumAttendance.User = userResponse.ToUser(roleResponse.ToRole());
            scrumAttendance.ScrumMeeting = scrumMeeting.ToScrumMeeting(managerUserResponse, roleResponse, managerRole);
            _scrumAttendances.Add(scrumAttendance);

            return scrumAttendance.ToScrumAttendanceResponse();
        }

        /// <summary>
        /// Get Scrum Attendance by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Task<ScrumAttendanceResponse>? GetScrumAttendanceById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid Id");
            }

            ScrumAttendance? scrumAttendance = _scrumAttendances.FirstOrDefault(x => x.Id == id);

            if (scrumAttendance == null)
            {
                return null;
            }

            return Task.FromResult(scrumAttendance.ToScrumAttendanceResponse());

        }

        /// <summary>
        /// Get Scrum Attendances by Meeting Id
        /// </summary>
        /// <param name="meetingId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<List<ScrumAttendanceResponse>?> GetAttendancesByMeeting(Guid meetingId)
        {
            if (meetingId == Guid.Empty)
            {
                throw new ArgumentException("Invalid Meeting Id");
            }

            List<ScrumAttendance> scrumAttendances = _scrumAttendances.Where(x => x.MeetingId == meetingId).ToList();

            if (scrumAttendances.Count == 0)
            {
                return null;
            }

            List<ScrumAttendanceResponse> scrumAttendanceResponses = new List<ScrumAttendanceResponse>();

            foreach (ScrumAttendance scrumAttendance in scrumAttendances)
            {
                scrumAttendanceResponses.Add(scrumAttendance.ToScrumAttendanceResponse());
            }
            return scrumAttendanceResponses;

        }

        /// <summary>
        /// Get Scrum Attendances of User by User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<List<ScrumAttendanceResponse>?> GetScrumAttendancesOfUserByUserId(Guid userId)
        {
            if(userId == Guid.Empty)
            {
                throw new ArgumentException("Invalid User Id");
            }

            List<ScrumAttendance> scrumAttendances = _scrumAttendances.Where(x => x.UserId == userId).ToList();

            if (scrumAttendances.Count == 0)
            {
                return null;
            }

            List<ScrumAttendanceResponse> scrumAttendanceResponses = new List<ScrumAttendanceResponse>();

            foreach (ScrumAttendance scrumAttendance in scrumAttendances)
            {
                scrumAttendanceResponses.Add(scrumAttendance.ToScrumAttendanceResponse());
            }

            return scrumAttendanceResponses;

        }

        /// <summary>
        /// Update Scrum Attendance
        /// </summary>
        /// <param name="attendanceId"></param>
        /// <param name="scrumAttendanceRequest"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task<ScrumAttendanceResponse> UpdateAttendance(Guid attendanceId, ScrumAttendanceRequest scrumAttendanceRequest)
        {
            if (attendanceId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(attendanceId), "Invalid Attendance ID");
            }

            if (scrumAttendanceRequest == null)
            {
                throw new ArgumentNullException(nameof(scrumAttendanceRequest));
            }

            bool isValidModel = ValidationHelper.IsStateValid(scrumAttendanceRequest);
            if (!isValidModel)
            {
                throw new ArgumentException("Invalid Scrum Attendance Request");
            }

            // Step 2: Find Attendance Record
            ScrumAttendance? scrumAttendance = _scrumAttendances.FirstOrDefault(x => x.Id == attendanceId);
            if (scrumAttendance == null)
            {
                throw new ArgumentException("Attendance record not found");
            }

            // Step 3: Validate Meeting & User
            ScrumMeetingResponse? scrumMeeting = await _scrumMeetingService.GetScrumMeetingById(scrumAttendanceRequest.MeetingId);
            if (scrumMeeting == null)
            {
                throw new ArgumentException("Meeting not found");
            }

            UserResponse? userResponse = await _userService.GetEmployeeById(scrumAttendanceRequest.UserId);
            if (userResponse == null)
            {
                throw new ArgumentException("User not found");
            }

            TeamMemberResponse? teamMemberResponse = await _teamMemberService.GetTeamMemberByUserId(scrumAttendanceRequest.UserId);
            if (teamMemberResponse == null || teamMemberResponse.TeamId != scrumMeeting.TeamId)
            {
                throw new ArgumentException("User is not part of the team");
            }

            // Step 4: Update Attendance Record
            scrumAttendance.IsPresent = scrumAttendanceRequest.IsPresent;
            scrumAttendance.Notes = scrumAttendanceRequest.Notes;

            return scrumAttendance.ToScrumAttendanceResponse();
        }

    }
}
