using DTOs.TeamMembers;
using DTOs.Teams;
using DTOs.Users;
using EfCore;
using Service_Contracts;
using DTOs.Roles;


namespace Services
{
    public class TeamMemberService : ITeamMemberService
    {

        private readonly List<TeamMember> _teamMembers;
        private readonly IUserService _userService;
        private readonly ITeamService _teamService;
        private readonly IRoleService _roleService;

        public TeamMemberService(IUserService userService, ITeamService teamService, IRoleService roleService)
        {
            _teamMembers = new List<TeamMember>();
            _userService = userService;
            _teamService = teamService;
            _roleService = roleService;
        }

        /// <summary>
        /// Add a team member to Team  -- Only Manager can add a team member
        /// </summary>
        /// <param name="teamMemberRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<TeamMemberResponse> AddTeamMember(TeamMemberRequest teamMemberRequest)
        {
            if(teamMemberRequest == null)
            {
                throw new ArgumentNullException();
            }

            bool isModelValid = ValidationHelper.IsStateValid(teamMemberRequest);
            if (!isModelValid)
            {
                throw new ArgumentException("Invalid Team Member Request", nameof(teamMemberRequest));
            }

            // Check team exists or not
            TeamResponse teamResponse = await _teamService.GetTeamById(teamMemberRequest.TeamId);
            if (teamResponse == null)
            {
                throw new ArgumentException("Team does not exist", nameof(teamResponse));
            }

            // Check manager exists and part of the same team
            UserResponse? managerResponse = await _userService.GetEmployeeById(teamMemberRequest.AddedByUserId);

            if (managerResponse == null)
            {
                throw new ArgumentException("Manager Not Found", nameof(managerResponse));
            }

            RoleResponse? managerRoleResponse = await _roleService.GetRoleById(managerResponse.RoleId);

            if (managerRoleResponse ==null || managerRoleResponse.Name != UserRoles.Manager.ToString())
            {
                throw new ArgumentException("Only Manager can Add a member to Team", nameof(managerResponse));
            }

            if(managerResponse.Id != teamResponse.ManagerId)
            {
                throw new ArgumentException("Only Same Manager of Team can Add a member to Team", nameof(managerResponse));
            }

            // Check if Employee exists

            UserResponse? response = await _userService.GetEmployeeById(teamMemberRequest.UserId);
            if (response == null)
            {
                throw new ArgumentException("User does not exist", nameof(response));
            }

            RoleResponse? roleResponse = await _roleService.GetRoleById(response.RoleId);

            if (roleResponse == null || roleResponse.Name != UserRoles.Employee.ToString())
            {
                throw new ArgumentException("Only Employee can be added to Team", nameof(roleResponse));
            }

            User user = response.ToUser(roleResponse.ToRole());

            // Check if he is already added to the team
            bool memberExist = _teamMembers.Any(m => m.UserId == teamMemberRequest.UserId && m.TeamId == teamMemberRequest.TeamId);
            if (memberExist)
            {
                throw new ArgumentException("User is already a member of the team");
            }

            TeamMember teamMember = teamMemberRequest.ToTeamMember();
            teamMember.Id = Guid.NewGuid();
            teamMember.User = user;
            teamMember.AddedById = managerResponse.Id;
            teamMember.AddedBy = managerResponse.ToUser(managerRoleResponse.ToRole());
            teamMember.AddedOn = DateTime.UtcNow;

            Team team = teamResponse.ToTeam(user);
            teamMember.Team = team;

            _teamMembers.Add(teamMember);

            return teamMember.ToTeamMemberResponse();

        }

        /// <summary>
        /// Get all team members of a team
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<TeamMemberResponse>> GetAllTeamMembers(Guid teamId)
        {
            if(teamId == Guid.Empty)
            {
                throw new ArgumentException("Invalid Team Id", nameof(teamId));
            }

            List<TeamMember> teamMembers = _teamMembers.Where(t => t.TeamId == teamId).ToList();
            List<TeamMemberResponse> teamMemberResponses = teamMembers.Select(t => t.ToTeamMemberResponse()).ToList();
            if(teamMemberResponses.Count == 0)
            {
                return new List<TeamMemberResponse>();
            }
            return teamMemberResponses;
        }

        /// <summary>
        /// Get a Team member by Id
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public async Task<TeamMemberResponse?> GetTeamMemberByUserId(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("Invalid Employee Id",nameof(userId));
            }

            TeamMember? teamMember = _teamMembers.FirstOrDefault(t => t.UserId == userId);

            if (teamMember == null)
            {
                return null;
            }

            return teamMember.ToTeamMemberResponse();

        }


        /// <summary>
        /// Delete a team member
        /// </summary>
        /// <param name="teamMemberId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteTeamMember(Guid teamMemberId)
        {
            if(teamMemberId == Guid.Empty)
            {
                throw new ArgumentException("Invalid Team member Id", nameof(teamMemberId));
            }

            TeamMember? teamMember = _teamMembers.FirstOrDefault(t => t.Id == teamMemberId);
            if (teamMember == null)
            {
                return false;
            }
            _teamMembers.Remove(teamMember);
            return true;
        }
    }
}
