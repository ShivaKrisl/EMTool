using DTOs.ScrumMeetings;
using DTOs.Teams;
using DTOs.Users;
using EfCore;
using Service_Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.TeamMembers;
using DTOs.Roles;

namespace Services
{
    public class ScrumMeetingService : IScrumMeetingService
    {
        private readonly List<ScrumMeeting> _scrumMeetings;
        private readonly ITeamService _teamService;
        private readonly IUserService _userService;
        private readonly ITeamMemberService _teamMemberService;
        private readonly IRoleService _roleService;

        /// <summary>
        /// Constructor for ScrumMeetingService
        /// </summary>
        public ScrumMeetingService()
        {
            _scrumMeetings = new List<ScrumMeeting>();
            _teamService = new TeamService();
            _userService = new UserService();
            _teamMemberService = new TeamMemberService();
            _roleService = new RoleService();
        }

        /// <summary>
        /// Create a Scrum Meeting
        /// </summary>
        /// <param name="meetingRequest"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>

        public async Task<ScrumMeetingResponse> CreateScrumMeeting(ScrumMeetingRequest meetingRequest)
        {
            if (meetingRequest == null)
            {
                throw new ArgumentNullException(nameof(meetingRequest));
            }

            bool isValidModel = ValidationHelper.IsStateValid(meetingRequest);

            if (!isValidModel)
            {
                throw new ArgumentException("Invalid Scrum Meeting Request");
            }

            // check url
            if (!Uri.IsWellFormedUriString(meetingRequest.MeetingLink, UriKind.Absolute))
            {
                throw new ArgumentException("Invalid Meeting Link");
            }

            // need to check if team exists
            // need to check if users exist and is part of the team
            // need to check invited users are there and are part of the team

            TeamResponse? teamResponse = await _teamService.GetTeamById(meetingRequest.TeamId);
            if (teamResponse == null)
            {
                throw new ArgumentException("Team does not exist");
            }

            // Get manager of team

            UserResponse? managerResponse = await _userService.GetEmployeeById(teamResponse.ManagerId);

            if (managerResponse == null)
            {
                throw new ArgumentException("Manager does not exist");
            }

            RoleResponse managerRoleResponse = await _roleService.GetRoleById(managerResponse.RoleId);

            if (managerRoleResponse == null || managerResponse.userRole != UserRoles.Manager.ToString())
            {
                throw new ArgumentException("Role does not exist");
            }

            UserResponse? userResponse = await _userService.GetEmployeeById(meetingRequest.CreatedBy);

            if (userResponse == null)
            {
                throw new ArgumentException("User does not exist");
            }

            RoleResponse? roleResponse = await _roleService.GetRoleById(userResponse.RoleId);
            if (roleResponse == null)
            {
                throw new ArgumentException("Role does not exist");
            }

            if (meetingRequest.InvitedUserIds.Count <= 0)
            {
                throw new ArgumentException("No users invited to the meeting");
            }

            foreach (Guid userId in meetingRequest.InvitedUserIds)
            {
                UserResponse? invitedUserResponse = await _userService.GetEmployeeById(userId);
                if (invitedUserResponse == null)
                {
                    throw new ArgumentException("Invited User does not exist");
                }

                TeamMemberResponse? teamMemberResponse = await _teamMemberService.GetTeamMemberByUserId(userId);

                if (teamMemberResponse == null || teamMemberResponse.TeamId != meetingRequest.TeamId)
                {
                    throw new ArgumentException("Invited User is not part of the team");
                }
            }
            ScrumMeeting scrumMeeting = meetingRequest.ToScrumMeeting();
            scrumMeeting.ScheduledDate = DateTime.Now;
            scrumMeeting.CreatedByRole = userResponse.userRole;
            scrumMeeting.CreatedByUser = userResponse.ToUser(roleResponse.ToRole());
            scrumMeeting.Team = teamResponse.ToTeam(managerResponse.ToUser(roleResponse.ToRole()));
            _scrumMeetings.Add(scrumMeeting);
            return scrumMeeting.ToScrumMeetingResponse();
        }


        public Task<ScrumMeetingResponse> GetScrumMeetingById(Guid meetingId)
        {
            if (meetingId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(meetingId));
            }

            ScrumMeeting? scrumMeeting = _scrumMeetings.FirstOrDefault(meeting => meeting.Id == meetingId);
            if (scrumMeeting == null)
            {
                throw new ArgumentException("Scrum Meeting does not exist");
            }

            return Task.FromResult(scrumMeeting.ToScrumMeetingResponse());

        }

        /// <summary>
        /// Get Scrum Meetings of a Team
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<List<ScrumMeetingResponse>?> GetScrumMeetingsOfTeam(Guid teamId)
        {
            if (teamId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(teamId));
            }

            List<ScrumMeeting> scrumMeetings = _scrumMeetings.Where(meeting => meeting.TeamId == teamId).ToList();
            if (scrumMeetings.Count <= 0)
            {
                return Task.FromResult<List<ScrumMeetingResponse>?>(null);
            }

            List<ScrumMeetingResponse> scrumMeetingResponses = new List<ScrumMeetingResponse>();

            foreach (ScrumMeeting scrumMeeting in scrumMeetings)
            {
                scrumMeetingResponses.Add(scrumMeeting.ToScrumMeetingResponse());
            }

            return Task.FromResult<List<ScrumMeetingResponse>?>(scrumMeetingResponses);

        }

        /// <summary>
        /// Get Scrum Meetings of a User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<List<ScrumMeetingResponse>?> GetScrumMeetingsOfUser(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            List<ScrumMeeting> scrumMeetings = _scrumMeetings.Where(meeting => meeting.CreatedBy == userId).ToList();

            if (scrumMeetings.Count <= 0)
            {
                return Task.FromResult<List<ScrumMeetingResponse>?>(null);
            }

            List<ScrumMeetingResponse> scrumMeetingResponses = new List<ScrumMeetingResponse>();

            foreach (ScrumMeeting scrumMeeting in scrumMeetings)
            {
                scrumMeetingResponses.Add(scrumMeeting.ToScrumMeetingResponse());
            }

            return Task.FromResult<List<ScrumMeetingResponse>?>(scrumMeetingResponses);

        }

        /// <summary>
        /// Update Scrum Meeting
        /// </summary>
        /// <param name="meetingId"></param>
        /// <param name="meetingRequest"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task<ScrumMeetingResponse> UpdateScrumMeeting(Guid meetingId, ScrumMeetingRequest meetingRequest)
        {
            if (meetingId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(meetingId));
            }

            if (meetingRequest == null)
            {
                throw new ArgumentNullException(nameof(meetingRequest));
            }

            bool isValidModel = ValidationHelper.IsStateValid(meetingRequest);
            if (!isValidModel) {
                throw new ArgumentException("Invalid Scrum Meeting Request");
            }

            // check url
            if (!Uri.IsWellFormedUriString(meetingRequest.MeetingLink, UriKind.Absolute))
            {
                throw new ArgumentException("Invalid Meeting Link");
            }

            // need to check if team exists

            TeamResponse? teamResponse = await _teamService.GetTeamById(meetingRequest.TeamId);
            if (teamResponse == null)
            {
                throw new ArgumentException("Team does not exist");
            }


            // Get manager of team

            UserResponse? managerResponse = await _userService.GetEmployeeById(teamResponse.ManagerId);

            if (managerResponse == null)
            {
                throw new ArgumentException("Manager does not exist");
            }

            RoleResponse managerRoleResponse = await _roleService.GetRoleById(managerResponse.RoleId);

            if (managerRoleResponse == null || managerResponse.userRole != UserRoles.Manager.ToString())
            {
                throw new ArgumentException("Role does not exist");
            }

            // need to check if users exist and is part of the team

            UserResponse? userResponse = await _userService.GetEmployeeById(meetingRequest.CreatedBy);
            if (userResponse == null) {
                throw new ArgumentException("User does not exist");
            }

            RoleResponse? roleResponse =  await _roleService.GetRoleById(userResponse.RoleId);

            if (roleResponse == null)
            {
                throw new ArgumentException("Role does not exist");
            }

            // need to check invited users are there and are part of the team

            if (meetingRequest.InvitedUserIds.Count <= 0)
            {
                throw new ArgumentException("No users invited to the meeting");
            }

            foreach (Guid userId in meetingRequest.InvitedUserIds)
            {
                UserResponse? invitedUserResponse = await _userService.GetEmployeeById(userId);
                if (invitedUserResponse == null)
                {
                    throw new ArgumentException("Invited User does not exist");
                }
                TeamMemberResponse? teamMemberResponse = await _teamMemberService.GetTeamMemberByUserId(userId);
                if (teamMemberResponse == null || teamMemberResponse.TeamId != meetingRequest.TeamId)
                {
                    throw new ArgumentException("Invited User is not part of the team");
                }

            }
            ScrumMeeting? scrumMeeting = _scrumMeetings.FirstOrDefault(meeting => meeting.Id == meetingId);
            if (scrumMeeting == null)
            {
                throw new ArgumentException("Scrum Meeting does not exist");
            }

            if (scrumMeeting.CreatedBy != meetingRequest.CreatedBy)
            {
                throw new ArgumentException("Only the creator of the meeting can update it.");
            }

            scrumMeeting.TeamId = meetingRequest.TeamId;
            scrumMeeting.ScheduledDate = meetingRequest.ScheduledDate;
            scrumMeeting.Agenda = meetingRequest.Agenda;
            scrumMeeting.MeetingLink = meetingRequest.MeetingLink;
            scrumMeeting.CreatedBy = meetingRequest.CreatedBy;
            scrumMeeting.CreatedByRole = userResponse.userRole;
            scrumMeeting.InvitedUsersIds = meetingRequest.InvitedUserIds;
            scrumMeeting.Team = teamResponse.ToTeam(managerResponse.ToUser(managerRoleResponse.ToRole()));
            scrumMeeting.CreatedByUser = userResponse.ToUser(roleResponse.ToRole());
            return scrumMeeting.ToScrumMeetingResponse();

        }


        /// <summary>
        /// Delete Scrum Meeting by Id
        /// </summary>
        /// <param name="meetingId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<bool> DeleteScrumMeeting(Guid meetingId)
        {
            if (meetingId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(meetingId));

            }

            ScrumMeeting? scrumMeeting = _scrumMeetings.FirstOrDefault(meeting => meeting.Id == meetingId);
            if (scrumMeeting == null)
            {
                return Task.FromResult(false);
            }
            _scrumMeetings.Remove(scrumMeeting);
            return Task.FromResult(true);

        }
    }
}
