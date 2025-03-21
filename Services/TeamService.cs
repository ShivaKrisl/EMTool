using DTOs.Teams;
using Service_Contracts;
using EfCore;
using DTOs.Users;
using DTOs.Roles;

namespace Services
{
    public class TeamService : ITeamService
    {

        private readonly List<Team> _teams;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public TeamService(IUserService userService, IRoleService roleService)
        {
            _teams = new List<Team>();
            _userService = userService;
            _roleService = roleService;
        }

        /// <summary>
        /// Create a new team
        /// </summary>
        /// <param name="teamRequest"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task<TeamResponse> CreateTeam(TeamRequest teamRequest)
        {
           if(teamRequest == null)
           {
                throw new ArgumentNullException();
           }

            bool isModelValid = ValidationHelper.IsStateValid(teamRequest);

            if (!isModelValid)
            {
                throw new ArgumentException("Invalid Team Request", nameof(teamRequest));    
            }

            UserResponse managerResponse = await _userService.GetEmployeeById(teamRequest.ManagerId);
            if (managerResponse == null)
            {
                throw new ArgumentException("Manager not found");
            }

            if (managerResponse.userRole != UserRoles.Manager.ToString())
            {
                throw new ArgumentException("Only Manager can create a Team");
            }

            RoleResponse? managerRole = await _roleService.GetRoleById(managerResponse.RoleId);
            if (managerRole == null)
            {
                throw new ArgumentException("Manager not found");
            }
            User manager = managerResponse.ToUser(managerRole.ToRole());

            List<Team> teamsUnderManager = _teams.Where(t => t.ManagerId == teamRequest.ManagerId).ToList();

            bool teamExists = teamsUnderManager.Any(t => t.TeamName == teamRequest.TeamName);
            if (teamExists)
            {
                throw new ArgumentException("A team with this name already exists.", nameof(teamRequest.TeamName));
            }

            Team team = teamRequest.ToTeam();
            team.Id = Guid.NewGuid();
            team.Manager = manager;

            // Save the team to the database
            _teams.Add(team);

            return team.ToTeamResponse();

        }

        /// <summary>
        /// Get a team by team name
        /// </summary>
        /// <param name="teamName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task<TeamResponse?> GetTeamByTeamName(string? teamName)
        {
            if (string.IsNullOrEmpty(teamName))
            {
                throw new ArgumentException("Invalid Team Name",nameof(teamName));
            }

            Team? team = _teams.FirstOrDefault(t => t.TeamName == teamName);
            if (team == null)
            {
                return null;
            }

            TeamResponse teamResponse = team.ToTeamResponse();
            return teamResponse;
        }

        /// <summary>
        /// Get Team by team id
        /// </summary>
        /// <param name="TeamId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<TeamResponse?> GetTeamById(Guid TeamId)
        {
            if(TeamId  == Guid.Empty)
            {
                throw new ArgumentException("Invalid Team Id",nameof(TeamId));
            }

            Team? team = _teams.FirstOrDefault(t => t.Id == TeamId);
            if (team == null)
            {
                return null;
            }

            return team.ToTeamResponse();
        }

        /// <summary>
        /// Update a team by team id
        /// </summary>
        /// <param name="TeamId"></param>
        /// <param name="teamRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<TeamResponse> UpdateTeamDetails(Guid TeamId, TeamRequest teamRequest)
        {
            if (TeamId == Guid.Empty)
            {
                throw new ArgumentException("Invalid Team Id",nameof(TeamId));
            }

            if (teamRequest == null)
            {
                throw new ArgumentNullException();
            }

            bool isModelValid = ValidationHelper.IsStateValid(teamRequest);
            if (!isModelValid)
            {
                throw new ArgumentException("Invalid Team Request", nameof(teamRequest));

            }

            Team? team = _teams.FirstOrDefault(t => t.Id == TeamId);
            if (team == null)
            {
                throw new ArgumentException("Team not found", nameof(team));
            }

            if(team.ManagerId != teamRequest.ManagerId || team.Manager.Role.Name != UserRoles.Manager.ToString())
            {
                throw new ArgumentException("Only Manager can edit Team details", nameof(team.Manager.Role));
            }

            List<Team> teamsUnderManagerWithSameName = _teams.Where(t => t.ManagerId == teamRequest.ManagerId && t.Id != TeamId && t.TeamName == teamRequest.TeamName).ToList();

            if (teamsUnderManagerWithSameName.Any())
            {
                throw new ArgumentException("A team with this name already exists.", nameof(teamsUnderManagerWithSameName));
            }

            team.TeamName = teamRequest.TeamName;
            return Task.FromResult(team.ToTeamResponse());
        }

        /// <summary>
        /// Delete a team by team id
        /// </summary>
        /// <param name="TeamId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<bool> DeleteTeam(Guid TeamId)
        {
            if(TeamId == Guid.Empty)
            {
                throw new ArgumentException("Invalid Team Id",nameof(TeamId));
            }

            Team? team = _teams.FirstOrDefault(t => t.Id == TeamId);
            if (team == null)
            {
               return Task.FromResult(false);
            }
            _teams.Remove(team);
            return Task.FromResult(true);
        }

        /// <summary>
        /// Get all teams Under a Manager
        /// </summary>
        /// <param name="managerId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<TeamResponse>> GetAllTeamsOfManager(Guid managerId)
        {
            if(managerId == Guid.Empty)
            {
                throw new ArgumentException("Invalid Manager Id", nameof(managerId));
            }

            List<TeamResponse>? teamsUnderManager = _teams.Where(t => t.ManagerId == managerId).Select(t => t.ToTeamResponse()).ToList();

            if (teamsUnderManager == null)
            {
                return new List<TeamResponse>();
            }
            return teamsUnderManager;
        }
       
    }
}
