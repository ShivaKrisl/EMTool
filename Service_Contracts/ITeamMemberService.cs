using DTOs.TeamMembers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Contracts
{
    public interface ITeamMemberService
    {
        /// <summary>
        /// Add a team member to Team
        /// </summary>
        /// <param name="teamMemberRequest"></param>
        /// <returns></returns>
        public Task<TeamMemberResponse> AddTeamMember(TeamMemberRequest teamMemberRequest);

        /// <summary>
        /// Get all team members of a team
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public Task<List<TeamMemberResponse>> GetAllTeamMembers(Guid teamId);

        /// <summary>
        /// Delete a team member
        /// </summary>
        /// <param name="teamMemberId"></param>
        /// <returns></returns>
        public Task<bool> DeleteTeamMember(Guid teamMemberId);
    }
}
