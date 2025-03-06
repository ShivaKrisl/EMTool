using DTOs.ScrumAttendances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Contracts
{
    public interface IScrumAttendanceService
    {
        /// <summary>
        /// Create a new Scrum Attendance
        /// </summary>
        /// <param name="scrumAttendanceRequest"></param>
        /// <returns></returns>
        public Task<ScrumAttendanceResponse> MarkAttendance(ScrumAttendanceRequest scrumAttendanceRequest);

        /// <summary>
        /// Update Scrum Attendance
        /// </summary>
        /// <param name="id"></param>
        /// <param name="scrumAttendanceRequest"></param>
        /// <returns></returns>
        public Task<ScrumAttendanceResponse> UpdateAttendance(Guid id, ScrumAttendanceRequest scrumAttendanceRequest);

        /// <summary>
        /// Get Scrum Attendance by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ScrumAttendanceResponse>? GetScrumAttendanceById(Guid id);

        /// <summary>
        /// Get Scrum Attendances by Meeting Id
        /// </summary>
        /// <param name="meetingId"></param>
        /// <returns></returns>
        public Task<List<ScrumAttendanceResponse>?> GetAttendancesByMeeting(Guid meetingId);

        /// <summary>
        /// Get Scrum Attendance of User by User Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<List<ScrumAttendanceResponse>?> GetScrumAttendancesOfUserByUserId(Guid userId);

    }
}
