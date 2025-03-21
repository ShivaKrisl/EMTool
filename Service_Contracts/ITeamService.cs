using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.Teams;

namespace Service_Contracts
{
    public interface ITeamService
    {
        /// <summary>
        /// Create a new team
        /// </summary>
        /// <param name="teamRequest"></param>
        /// <returns></returns>
        public Task<TeamResponse> CreateTeam(TeamRequest teamRequest);

        /// <summary>
        /// Get a team by team name
        /// </summary>
        /// <param name="teamName"></param>
        /// <returns></returns>
        public Task<TeamResponse?> GetTeamByTeamName(string? teamName);

        /// <summary>
        /// Get a team by team id
        /// </summary>
        /// <param name="TeamId"></param>
        /// <returns></returns>
        public Task<TeamResponse?> GetTeamById(Guid TeamId);

        /// <summary>
        /// Update a team by team id
        /// </summary>
        /// <param name="teamRequest"></param>
        /// <returns></returns>
        public Task<TeamResponse> UpdateTeamDetails(Guid TeamId, TeamRequest teamRequest);

        /// <summary>
        /// Delete a team by team id
        /// </summary>
        /// <param name="teamRequest"></param>
        /// <returns></returns>
        public Task<bool> DeleteTeam(Guid TeamId);

        /// <summary>
        /// Get all teams Under a Manager
        /// </summary>
        /// <param name="ManagerId"></param>
        /// <returns></returns>
        public Task<List<TeamResponse>> GetAllTeamsOfManager(Guid ManagerId);
    }
}
