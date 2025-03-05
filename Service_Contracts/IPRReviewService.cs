using DTOs.PRReviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Contracts
{
    public interface IPRReviewService
    {
        /// <summary>
        /// Add a review to a pull request
        /// </summary>
        /// <param name="reviewRequest"></param>
        /// <returns></returns>
        public Task<PRReviewResponse> AddReview(PRReviewRequest reviewRequest);

        /// <summary>
        /// Get all reviews of a pull request
        /// </summary>
        /// <param name="pullRequestId"></param>
        /// <returns></returns>
        public Task<List<PRReviewResponse>?> GetReviewsOfPullRequest(Guid pullRequestId);

        /// <summary>
        /// Get all reviews given by user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<List<PRReviewResponse>?> GetReviewsOfUser(Guid userId);

        /// <summary>
        /// Edit a review
        /// </summary>
        /// <param name="reviewerUserId"></param>
        /// <param name="reviewRequest"></param>
        /// <returns></returns>
        public Task<PRReviewResponse> EditReview(Guid reviewerUserId,PRReviewRequest reviewRequest);

        /// <summary>
        /// Delete a review
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        public Task<bool> DeleteReview(Guid reviewId);
    }
}
