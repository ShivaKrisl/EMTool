using DTOs.PullRequests;
using DTOs.Users;
using EfCore;
using Service_Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.Tasks;
using DTOs.Roles;

namespace Services
{
    public class PullRequestService : IPullRequestService
    {
        private readonly List<PullRequest> _pullRequests;
        private readonly IUserService _userService;
        private readonly IWorkService _workService;
        private readonly IRoleService _roleService;

        public PullRequestService()
        {
            _pullRequests = new List<PullRequest>();
            _userService = new UserService();
            _workService = new WorkService();
            _roleService = new RoleService();
        }

        /// <summary>
        /// Create a Pull Request
        /// </summary>
        /// <param name="PR_EntityRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PREntityResponse> CreatePullRequest(PREntityRequest PR_EntityRequest)
        {
            if (PR_EntityRequest == null)
            {
                throw new ArgumentNullException(nameof(PR_EntityRequest));
            }

            bool isValidModel = ValidationHelper.IsStateValid(PR_EntityRequest);
            if (!isValidModel)
            {
                throw new ArgumentException("Invalid Request!");
            }

            bool existingPR = _pullRequests.Any(pr => pr.TaskId == PR_EntityRequest.TaskId && pr.PRStatus == PRStatus.Pending.ToString());
            if (existingPR)
            {
                throw new ArgumentException("A PR already exists for this task! Please wait for approval.");
            }

            if (!Uri.TryCreate(PR_EntityRequest.PRLink, UriKind.Absolute, out _))
            {
                throw new ArgumentException("Invalid PR link format!");
            }

            UserResponse userResponse =  await _userService.GetEmployeeById(PR_EntityRequest.CreatedById);
            // check user exists
            if(userResponse == null)
            {
                throw new ArgumentException("User not found!");
            }

            RoleResponse? roleResponse = await _roleService.GetRoleById(userResponse.RoleId);

            if(roleResponse == null)
            {
                throw new ArgumentException("Role not found!");
            }

            WorkResponse workResponse = await _workService.GetWorkById(PR_EntityRequest.TaskId);
            // check Work exists
            if (workResponse == null)
            {
                throw new ArgumentException("Task not found!");
            }
            // check user is part of the work
            if (workResponse.AssignedTo.Id != userResponse.Id)
            {
                throw new ArgumentException("User is not part of the task!");
            }
            PullRequest newPullRequest = PR_EntityRequest.ToPullRequest();
            newPullRequest.PRStatus = PRStatus.Pending.ToString();
            newPullRequest.CreatedAt = DateTime.UtcNow;
            newPullRequest.CreatedBy = userResponse.ToUser(roleResponse.ToRole());
            newPullRequest.Task = workResponse.ToWork(workResponse.AssignedBy, workResponse.AssignedTo, roleResponse);
            _pullRequests.Add(newPullRequest);
            return newPullRequest.ToPREntityResponse();
        }

        /// <summary>
        /// Get a pull request by its Id
        /// </summary>
        /// <param name="PR_Id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<PREntityResponse> GetPullRequestById(Guid PR_Id)
        {
            if(PR_Id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(PR_Id));
            }

            PullRequest? pullRequest = _pullRequests.FirstOrDefault(pr => pr.Id == PR_Id);
            if (pullRequest == null)
            {
                throw new ArgumentException("PR not found!");
            }
            return Task.FromResult(pullRequest.ToPREntityResponse());
        }

        /// <summary>
        /// Get all Pull Requests of a Task
        /// </summary>
        /// <param name="TaskId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<List<PREntityResponse>?> GetPullRequestsOfTask(Guid TaskId)
        {
            if (TaskId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(TaskId));
            }

            List<PullRequest>? pullRequests = _pullRequests.Where(pr => pr.TaskId == TaskId).ToList();
            if (!pullRequests.Any())
            {
                return Task.FromResult<List<PREntityResponse>?>(null);
            }
            List<PREntityResponse>? pREntityResponses = new List<PREntityResponse>();
            foreach (PullRequest pullRequest in pullRequests)
            {
                pREntityResponses.Add(pullRequest.ToPREntityResponse());
            }
            return Task.FromResult<List<PREntityResponse>?>(pREntityResponses);
        }

        /// <summary>
        /// Get all PR's of a user
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<List<PREntityResponse>?> GetPullRequestsOfUser(Guid UserId)
        {
            if(UserId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(UserId));
            }

            List<PullRequest>? pullRequests = _pullRequests.Where(pr => pr.CreatedById == UserId).ToList();
            if (!pullRequests.Any())
            {
                return Task.FromResult<List<PREntityResponse>?>(null);
            }

            List<PREntityResponse>? pREntityResponses = new List<PREntityResponse>();
            foreach (PullRequest pullRequest in pullRequests)
            {
                pREntityResponses.Add(pullRequest.ToPREntityResponse());
            }
            return Task.FromResult<List<PREntityResponse>?>(pREntityResponses);
        }


        public bool GetPRApprovalStatus(Guid PR_Id)
        {
            if (PR_Id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(PR_Id));
            }

            PullRequest? pullRequest = _pullRequests.FirstOrDefault(pr => pr.Id == PR_Id);
            if (pullRequest == null)
            {
                throw new ArgumentException("PR not found!");
            }

            int totalReviews = pullRequest.Reviews.Count;
            int approvedReviews = pullRequest.Reviews.Count(r => r.ReviewStatus == PRStatus.Approved.ToString());

            // if all reviews are approved
            if (totalReviews > 0 && approvedReviews == totalReviews)
            {
                pullRequest.IsReadyForApproval = true;
                return true;
            }

            // check for >=70%
            if (totalReviews > 0 && (approvedReviews / (double)totalReviews) >= 0.7)
            {
                pullRequest.IsReadyForApproval = true;
                return true;
            }
            return false;

        }

        /// <summary>
        /// Update the status of a pull request by its Id only manager can do this
        /// </summary>
        /// <param name="ManagerId"></param>
        /// <param name="PR_Id"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PREntityResponse> UpdateStatusOfPullRequest(Guid ManagerId, Guid PR_Id, string newStatus)
        {
            if(ManagerId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(ManagerId));
            }
            if(PR_Id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(PR_Id));
            }
            if (string.IsNullOrEmpty(newStatus))
            {
                throw new ArgumentNullException(nameof(newStatus));
            }

            try
            {
                PRStatus pRStatus = (PRStatus)Enum.Parse(typeof(PRStatus), newStatus, true);
            }
            catch
            {
                throw new ArgumentException("Invalid status!");
            }

            // check if manager exists
            UserResponse? userResponse =  await _userService.GetEmployeeById(ManagerId);
            if(userResponse == null)
            {
                throw new ArgumentException("Manager not found!!");
            }
            // check if pr exists
            PullRequest? pullRequest = _pullRequests.FirstOrDefault(pr => pr.Id == PR_Id);
            if (pullRequest == null)
            {
                throw new ArgumentException("PR not found!");
            }

            WorkResponse? workResponse =  await _workService.GetWorkById(pullRequest.TaskId);
            if (workResponse == null)
            {
                throw new ArgumentException("Task not Found!!");
            }

            if (workResponse.AssignedBy.Id != ManagerId)
            {
                throw new ArgumentException("Manager is not assigned to this task!");
            }

            if (pullRequest.PRStatus == PRStatus.Approved.ToString() || pullRequest.PRStatus == PRStatus.Rejected.ToString())
            {
                throw new ArgumentException("PR is already approved or rejected!");
            }

            bool isReady = GetPRApprovalStatus(PR_Id);
            if (!isReady && newStatus == PRStatus.Approved.ToString())
            {
                throw new ArgumentException("PR cannot be approved until it has enough reviews!");
            }

            pullRequest.PRStatus = newStatus;
            return pullRequest.ToPREntityResponse();
        }


        /// <summary>
        /// Delete a pull request by its Id
        /// </summary>
        /// <param name="PR_Id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<bool> DeletePullRequest(Guid PR_Id)
        {
            if(PR_Id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(PR_Id));
            }

            PullRequest? pullRequest = _pullRequests.FirstOrDefault(pr => pr.Id == PR_Id);
            if (pullRequest == null)
            {
                return Task.FromResult(false);
            }
            _pullRequests.Remove(pullRequest);
            return Task.FromResult(true);
        }

        /// <summary>
        /// Get all Pull Requests of a Team
        /// </summary>
        /// <param name="TeamId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<List<PREntityResponse>?> GetAllPullRequestsOfTeam(Guid TeamId)
        {
            if(TeamId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(TeamId));
            }

            List<PullRequest>? pullRequests = _pullRequests.Where(pr => pr.Task.TeamId == TeamId).ToList();

            if (!pullRequests.Any())
            {
                return Task.FromResult<List<PREntityResponse>?>(null);
            }

            List<PREntityResponse>? pREntityResponses = new List<PREntityResponse>();

            foreach (PullRequest pullRequest in pullRequests)
            {
                pREntityResponses.Add(pullRequest.ToPREntityResponse());
            }

            return Task.FromResult<List<PREntityResponse>?>(pREntityResponses);

        }
    }
}
