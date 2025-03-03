using DTOs.Teams;
using Service_Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfCore;
using DTOs.Users;
using DTOs.Roles;

namespace Services
{
    public class TeamService : ITeamService
    {

        private readonly List<Team> _teams;
        private readonly IUserService _userService;
        private readonly RoleService _roleService;

        public TeamService()
        {
            _teams = new List<Team>();
            _userService = new UserService();
            _roleService = new RoleService();
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
                throw new ArgumentNullException(nameof(teamRequest));
           }

            bool isModelValid = ValidationHelper.IsStateValid(teamRequest);

            if (!isModelValid)
            {
                throw new ArgumentException("Invalid Team Request");    
            }

            bool teamExists = _teams.Any(t => t.TeamName == teamRequest.TeamName);
            if (teamExists)
            {
                throw new ArgumentException("A team with this name already exists.");
            }

            Team team = teamRequest.ToTeam();
            team.Id = Guid.NewGuid();

            UserResponse managerResponse = await _userService.GetEmployeeById(teamRequest.ManagerId);
            if (managerResponse == null)
            {
                throw new ArgumentException("Manager not found");
            }

            if(managerResponse.userRole != UserRoles.Manager.ToString())
            {
                throw new ArgumentException("Only Manager can create a Team");
            }

            RoleResponse? managerRole = await _roleService.GetRoleById(managerResponse.RoleId);
            if (managerRole == null)
            {
                throw new ArgumentException("Manager not found");
            }
            User manager = managerResponse.ToUser(managerRole.ToRole());
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
        public Task<TeamResponse> GetTeamByTeamName(string? teamName)
        {
            if (string.IsNullOrEmpty(teamName))
            {
                throw new ArgumentNullException(nameof(teamName));
            }

            Team? team = _teams.FirstOrDefault(t => t.TeamName == teamName);
            if (team == null)
            {
                throw new ArgumentException("Team not found");
            }

            TeamResponse teamResponse = team.ToTeamResponse();
            return Task.FromResult(teamResponse);
        }

        /// <summary>
        /// Get Team by team id
        /// </summary>
        /// <param name="TeamId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<TeamResponse> GetTeamById(Guid TeamId)
        {
            if(TeamId  == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(TeamId));
            }

            Team? team = _teams.FirstOrDefault(t => t.Id == TeamId);
            if (team == null)
            {
                throw new ArgumentException("Team not found");
            }

           return Task.FromResult(team.ToTeamResponse());
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
                throw new ArgumentNullException(nameof(TeamId));
            }

            if (teamRequest == null)
            {
                throw new ArgumentNullException(nameof(teamRequest));
            }

            bool isModelValid = ValidationHelper.IsStateValid(teamRequest);
            if (!isModelValid)
            {
                throw new ArgumentException("Invalid Team Request");

            }

            Team? teamExist = _teams.FirstOrDefault(t => t.TeamName == teamRequest.TeamName);
            if (teamExist != null)
            {
                throw new ArgumentException("A team with this name already exists.");
            }

            Team? team = _teams.FirstOrDefault(t => t.Id == TeamId);
            if (team == null)
            {
                throw new ArgumentException("Team not found");
            }

            if(team.Manager.Role.Name != UserRoles.Manager.ToString())
            {
                throw new ArgumentException("Only Manager can edit Team details");
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
                throw new ArgumentNullException(nameof(TeamId));
            }

            Team? team = _teams.FirstOrDefault(t => t.Id == TeamId);
            if (team == null)
            {
               return Task.FromResult(false);
            }
            _teams.Remove(team);
            return Task.FromResult(true);

        }
       
    }
}
