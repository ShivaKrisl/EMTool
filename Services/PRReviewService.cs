using DTOs.PRReviews;
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
using DTOs.Teams;
using DTOs.TeamMembers;
using DTOs.Roles;

namespace Services
{
    public class PRReviewService : IPRReviewService
    {
        private readonly List<Review> _reviews;
        private readonly IPullRequestService _pullRequestService;
        private readonly IUserService _userService;
        private readonly IWorkService _workService;
        private readonly ITeamService _teamService;
        private readonly ITeamMemberService _teamMemberService;
        private readonly IRoleService _roleService;

        public PRReviewService()
        {
            _reviews = new List<Review>();
            _pullRequestService = new PullRequestService();
            _userService = new UserService();
            _workService = new WorkService();
            _teamService = new TeamService();
            _teamMemberService = new TeamMemberService();
            _roleService = new RoleService();
        }

        /// <summary>
        /// Add a review to a pull request
        /// </summary>
        /// <param name="reviewRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PRReviewResponse> AddReview(PRReviewRequest reviewRequest)
        {
            if(reviewRequest == null)
            {
                throw new ArgumentNullException(nameof(reviewRequest));
            }

            bool isValidModel = ValidationHelper.IsStateValid(reviewRequest);

            if (!isValidModel)
            {
                throw new ArgumentException("Invalid Request!");
            }
            
            PREntityResponse? pREntityResponse = await _pullRequestService.GetPullRequestById(reviewRequest.PullRequestId);
            if (pREntityResponse == null)
            {
                throw new Exception("Pull Request not found");
            }

            WorkResponse? workResponse = await _workService.GetWorkById(pREntityResponse.TaskId);
            if(workResponse == null)
            {
                throw new ArgumentException("Task not found");
            }

            UserResponse userResponse = await _userService.GetEmployeeById(reviewRequest.ReviewerId);
            if(userResponse == null)
            {
                throw new Exception("User not found");
            }

            // check is reviewer is part of the same team where PR is raised

            TeamResponse? teamResponse = await _teamService.GetTeamById(workResponse.TeamId);
            if (teamResponse == null)
            {
                throw new Exception("Team not found");
            }

            TeamMemberResponse? teamMemberResponse = await _teamMemberService.GetTeamMemberByUserId(userResponse.Id);
            if (teamMemberResponse == null || teamMemberResponse.TeamId != teamResponse.Id)
            {
                throw new Exception("Reviewer is not part of the same team where PR is raised");
            }

            RoleResponse? roleResponse = await _roleService.GetRoleById(userResponse.RoleId);
            if (roleResponse == null)
            {
                throw new Exception("Role not found");
            }

            Review review = reviewRequest.ToReview();
            review.Id = Guid.NewGuid();
            review.ReviewDate = DateTime.UtcNow;
            review.PullRequest = pREntityResponse.ToPullRequest();
            review.Reviewer = userResponse.ToUser(roleResponse.ToRole());
            _reviews.Add(review);
            return review.ToPRReviewResponse();
        }

        /// <summary>
        /// Get all reviews of a pull request
        /// </summary>
        /// <param name="pullRequestId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<List<PRReviewResponse>?> GetReviewsOfPullRequest(Guid pullRequestId)
        {
            if(pullRequestId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(pullRequestId));
            }

            List<Review>? reviews = _reviews.Where(r => r.PullRequestId == pullRequestId).ToList();
            if (!reviews.Any())
            {
                return Task.FromResult<List<PRReviewResponse>?>(null);
            }

            List<PRReviewResponse>? pRReviewResponses = new List<PRReviewResponse>();
            foreach (Review review in reviews)
            {
                pRReviewResponses.Add(review.ToPRReviewResponse());
            }

            return Task.FromResult<List<PRReviewResponse>?>(pRReviewResponses);
        }

        /// <summary>
        /// Get all reviews given by user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<List<PRReviewResponse>?> GetReviewsOfUser(Guid userId)
        {
            if(userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            List<Review>? reviews = _reviews.Where(r => r.ReviewerId == userId).ToList();

            if (!reviews.Any())
            {
                return Task.FromResult<List<PRReviewResponse>?>(null);
            }

            List<PRReviewResponse>? pRReviewResponses = new List<PRReviewResponse>();
            foreach (Review review in reviews)
            {
                pRReviewResponses.Add(review.ToPRReviewResponse());
            }

            return Task.FromResult<List<PRReviewResponse>?>(pRReviewResponses);
        }

        /// <summary>
        /// Edit a review
        /// </summary>
        /// <param name="reviewerUserId"></param>
        /// <param name="reviewRequest"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task<PRReviewResponse> EditReview(Guid reviewerUserId, PRReviewRequest reviewRequest)
        {
            if(reviewerUserId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(reviewerUserId));
            }

            if(reviewRequest == null)
            {
                throw new ArgumentNullException(nameof(reviewRequest));
            }

            bool isValidModel = ValidationHelper.IsStateValid(reviewRequest);
            if (!isValidModel)
            {
                throw new ArgumentException("Invalid Request");
            }

            UserResponse userResponse = await _userService.GetEmployeeById(reviewerUserId);

            if (userResponse == null)
            {
                throw new ArgumentException("User not found");
            }

            PREntityResponse? pREntityResponse = await _pullRequestService.GetPullRequestById(reviewRequest.PullRequestId);

            if (pREntityResponse == null) {
                throw new ArgumentException("Pull Request not found");
            }

            WorkResponse? workResponse = await _workService.GetWorkById(pREntityResponse.TaskId);

            if (workResponse == null)
            {
                throw new ArgumentException("Task not found!");
            }

            TeamMemberResponse? teamMemberResponse = await _teamMemberService.GetTeamMemberByUserId(reviewerUserId);

            if (teamMemberResponse == null || teamMemberResponse.TeamId != workResponse.TeamId)
            {
                throw new ArgumentException("Reviewer is not part of same team");
            }

            // Find the specific review(not just the first review by this user)
            Review? review = _reviews.FirstOrDefault(r => r.ReviewerId == reviewerUserId && r.PullRequestId == reviewRequest.PullRequestId);

            if(review == null)
            {
                throw new ArgumentException("Review not found");
            }

            if (review.ReviewerId != reviewerUserId)
            {
                throw new UnauthorizedAccessException("You can only edit your own review");
            }

            review.ReviewStatus = reviewRequest.ReviewStatus;
            review.ReviewComments = reviewRequest.ReviewComments;
            review.ReviewDate = DateTime.UtcNow;

            return review.ToPRReviewResponse();

        }

        public Task<bool> DeleteReview(Guid reviewId)
        {
            if(reviewId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(reviewId));
            }

            Review? review = _reviews.FirstOrDefault(r => r.Id == reviewId);
            if(review == null)
            {
                return Task.FromResult(false);
            }

            _reviews.Remove(review);
            return Task.FromResult(true);
        }

        
    }
}
