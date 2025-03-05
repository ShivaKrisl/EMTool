using DTOs.PullRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Contracts
{
    public interface IPullRequestService
    {
        /// <summary>
        /// Create a new Pull Request
        /// </summary>
        /// <param name="PR_EntityRequest"></param>
        /// <returns></returns>
        public Task<PREntityResponse> CreatePullRequest(PREntityRequest PR_EntityRequest);

        /// <summary>
        /// Get a pull request by its Id
        /// </summary>
        /// <param name="PR_Id"></param>
        /// <returns></returns>
        public Task<PREntityResponse> GetPullRequestBy(Guid PR_Id);

        /// <summary>
        /// Get all Pull Requests of a Task
        /// </summary>
        /// <param name="TaskId"></param>
        /// <returns></returns>
        public Task<List<PREntityResponse>?> GetPullRequestsOfTask(Guid TaskId);

        /// <summary>
        /// Get all pull requests of a user
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public Task<List<PREntityResponse>?> GetPullRequestsOfUser(Guid UserId);

        /// <summary>
        /// Update the status of a pull request by its Id only manager can do this
        /// </summary>
        /// <param name="PR_Id"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        public Task<PREntityResponse> UpdateStatusOfPullRequest(Guid ManagerId, Guid PR_Id, string newStatus);

        /// <summary>
        /// Delete a pull request by its Id
        /// </summary>
        /// <param name="PR_Id"></param>
        /// <returns></returns>
        public Task<bool> DeletePullRequest(Guid PR_Id);

    }
}
