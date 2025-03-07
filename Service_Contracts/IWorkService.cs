using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTOs.Tasks;

namespace Service_Contracts
{
    public interface IWorkService
    {
        /// <summary>
        /// Create a new work (Task assigned by a Manager)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<WorkResponse> CreateWork(WorkRequest workRequest);

        /// <summary>
        /// Update an existing work (Only assigned tasks can be updated)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="workId"></param>
        /// <returns></returns>
        public Task<WorkResponse> UpdateWork(WorkRequest workRequest, Guid workId, Guid userId);

        /// <summary>
        /// Get work by Id (Retrieve a specific task)
        /// </summary>
        /// <param name="workId"></param>
        /// <returns></returns>
        public Task<WorkResponse> GetWorkById(Guid workId);

        /// <summary>
        /// Get all works assigned to a specific employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public Task<List<WorkResponse>> GetEmployeeWorks(Guid employeeId);

        /// <summary>
        /// Get all works assigned to a specific team
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public Task<List<WorkResponse?>> GetAllWorksOfTeam(Guid teamId);

        /// <summary>
        /// Delete a work (Only managers can delete work)
        /// </summary>
        /// <param name="workId"></param>
        /// <returns></returns>
        public Task<bool> DeleteWork(Guid workId);
    }
}
