using DTOs.TeamMembers;
using DTOs.Teams;
using DTOs.Users;
using EfCore;
using Service_Contracts;


namespace Services
{
    public class TeamMemberService : ITeamMemberService
    {

        private readonly List<TeamMember> _teamMembers;
        private readonly IUserService _userService;
        private readonly ITeamService _teamService;

        public TeamMemberService()
        {
            _teamMembers = new List<TeamMember>();
            _userService = new UserService();
            _teamService = new TeamService();
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
                throw new ArgumentNullException(nameof(teamMemberRequest));
            }

            bool isModelValid = ValidationHelper.IsStateValid(teamMemberRequest);
            if (!isModelValid)
            {
                throw new ArgumentException("Invalid Team Member Request");
            }

            // Check if he is already added to the team
            bool memberExist = _teamMembers.Any(m => m.UserId == teamMemberRequest.UserId && m.TeamId == teamMemberRequest.TeamId);
            if (memberExist)
            {
                throw new ArgumentException("User is already a member of the team");
            }

            TeamMember teamMember = teamMemberRequest.ToTeamMember();
            teamMember.Id = Guid.NewGuid();

            UserResponse response = await _userService.GetEmployeeById(teamMember.UserId);
            if (response == null)
            {
                throw new ArgumentException("User does not exist");
            }
            User user = response.ToUser();
            teamMember.User = user;

            TeamResponse teamResponse = await _teamService.GetTeamById(teamMember.TeamId);
            if (teamResponse == null)
            {
                throw new ArgumentException("Team does not exist");
            }

            if(teamResponse.ManagerId != response.Id)
            {
                throw new ArgumentException("Only Manager can add a team member");
            }

            Team team = teamResponse.ToTeam();
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
        public Task<List<TeamMemberResponse>> GetAllTeamMembers(Guid teamId)
        {
            if(teamId == Guid.Empty)
            {
                return Task.FromResult(new List<TeamMemberResponse>());
            }

            List<TeamMember> teamMembers = _teamMembers.Where(t => t.TeamId == teamId).ToList();
            List<TeamMemberResponse> teamMemberResponses = teamMembers.Select(t => t.ToTeamMemberResponse()).ToList();
            return Task.FromResult(teamMemberResponses);
        }

        /// <summary>
        /// Delete a team member
        /// </summary>
        /// <param name="teamMemberId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<bool> DeleteTeamMember(Guid teamMemberId)
        {
            if(teamMemberId == Guid.Empty)
            {
               return Task.FromResult(false);
            }

            TeamMember? teamMember = _teamMembers.FirstOrDefault(t => t.Id == teamMemberId);
            if (teamMember == null)
            {
                return Task.FromResult(false);
            }
            _teamMembers.Remove(teamMember);
            return Task.FromResult(true);
        }
    }
}
