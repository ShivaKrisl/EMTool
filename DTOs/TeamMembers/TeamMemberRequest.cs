using EfCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.TeamMembers
{
    public class TeamMemberRequest
    {
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
        /// Convert TeamMemberRequest to TeamMember
        /// </summary>
        /// <returns></returns>
        public TeamMember ToTeamMember()
        {
            return new TeamMember()
            {
                TeamId = TeamId,
                UserId = UserId, 
            };
        }
    }
}
