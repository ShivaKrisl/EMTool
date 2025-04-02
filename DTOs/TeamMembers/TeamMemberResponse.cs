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
        /// Manager Id
        /// </summary>
        [Required]
        public Guid AddedByUserId { get; set; }

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

        /// <summary>
        /// Team 
        /// </summary>
        [Required]
        public Team Team { get; set; }


        /// <summary>
        /// User 
        /// </summary>
        [Required]
        public User User { get; set; }

        /// <summary>
        /// Manager
        /// </summary>
        [Required]
        public User AddedBy { get; set; }


        /// <summary>
        /// Added Date
        /// </summary>
        [Required]
        public DateTime AddedOn { get; set; }

        public override bool Equals(object? obj)
        {
            if(obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            TeamMemberResponse teamMemberResponse = (TeamMemberResponse)obj;

            return Id == teamMemberResponse.Id && TeamId == teamMemberResponse.TeamId && UserId == teamMemberResponse.UserId && UserName == teamMemberResponse.UserName && TeamName == teamMemberResponse.TeamName && Team == teamMemberResponse.Team && User == teamMemberResponse.User && teamMemberResponse.AddedByUserId == AddedByUserId && teamMemberResponse.AddedBy == AddedBy;

        }

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
                TeamName = teamMember.Team.TeamName,
                Team = teamMember.Team,
                User = teamMember.User,
                AddedByUserId = teamMember.AddedById,
                AddedBy = teamMember.AddedBy,
                AddedOn = teamMember.AddedOn,
            };
        }

    }

}
