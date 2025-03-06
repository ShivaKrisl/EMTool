using DTOs.ScrumMeetings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Contracts
{
    public interface IScrumMeetingService
    {
        /// <summary>
        /// Create a Scrum Meeting
        /// </summary>
        /// <param name="meetingRequest"></param>
        /// <returns></returns>
        public Task<ScrumMeetingResponse> CreateScrumMeeting(ScrumMeetingRequest meetingRequest);

        /// <summary>
        /// Get Scrum Meeting by Id
        /// </summary>
        /// <param name="meetingId"></param>
        /// <returns></returns>
        public Task<ScrumMeetingResponse> GetScrumMeetingById(Guid meetingId);

        /// <summary>
        /// Get Scrum Meetings of a Team
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public Task<List<ScrumMeetingResponse>?> GetScrumMeetingsOfTeam(Guid teamId);

        /// <summary>
        /// Get Scrum Meetings of a User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<List<ScrumMeetingResponse>?> GetScrumMeetingsOfUser(Guid userId);

        /// <summary>
        /// Update Scrum Meeting
        /// </summary>
        /// <param name="meetingId"></param>
        /// <param name="meetingRequest"></param>
        /// <returns></returns>
        public Task<ScrumMeetingResponse> UpdateScrumMeeting(Guid meetingId, ScrumMeetingRequest meetingRequest);

        /// <summary>
        /// Delete Scrum Meeting
        /// </summary>
        /// <param name="meetingId"></param>
        /// <returns></returns>
        public Task<bool> DeleteScrumMeeting(Guid meetingId);
    }
}
