using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfCore;
namespace DTOs.Teams
{
    public class TeamResponse
    {

        /// <summary>
        /// Team Id
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Team Name
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string TeamName { get; set; }

        /// <summary>
        /// Team Manager Id
        /// </summary>
        [Required]
        public Guid ManagerId { get; set; }

        [Required]
        public string ManagerName { get; set; }
    }

    public static class TeamResponseExtensions
    {
        public static TeamResponse ToTeamResponse(this Team team)
        {
            return new TeamResponse
            {
                Id = team.Id,
                TeamName = team.TeamName,
                ManagerId = team.ManagerId,
                ManagerName = team.Manager.Username,

            };
        }

        public static Team ToTeam(this TeamResponse teamResponse, User manager)
        {
            return new Team
            {
                Id = teamResponse.Id,
                TeamName = teamResponse.TeamName,
                ManagerId = teamResponse.ManagerId,
                Manager = manager
            };
        }
    }

}
