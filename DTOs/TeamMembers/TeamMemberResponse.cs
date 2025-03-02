using EfCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.TeamMembers
{
    public class TeamMemberResponse
    {

        /// <summary>
        /// Team member Id
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Team Id
        /// </summary>
        [Required]
        public Guid TeamId { get; set; }

        /// <summary>
        /// User Id
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// User Name
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Team Name
        /// </summary>
        [Required]
        public string TeamName { get; set; }

    }

    public static class TeamMemberResponseExtensions
    {
        public static TeamMemberResponse ToTeamMemberResponse(this TeamMember teamMember)
        {
            return new TeamMemberResponse
            {
                Id = teamMember.Id,
                TeamId = teamMember.TeamId,
                UserId = teamMember.UserId,
                UserName = teamMember.User.Username,
                TeamName = teamMember.Team.TeamName
            };
        }

    }

}
