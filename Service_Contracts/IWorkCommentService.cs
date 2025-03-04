using DTOs.WorkComments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Contracts
{
    public interface IWorkCommentService
    {

        /// <summary>
        /// Add Comment on a Work
        /// </summary>
        /// <param name="workCommentRequest"></param>
        /// <returns></returns>
        public Task<WorkCommentResponse> AddComment(WorkCommentRequest workCommentRequest);

        /// <summary>
        /// Get all Comments given on a Work
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public Task<List<WorkCommentResponse>?> GetCommentsOnWork(Guid taskId);

        /// <summary>
        /// Edit Comment
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="workCommentRequest"></param>
        /// <returns></returns>
        public Task<WorkCommentResponse> EditComment(Guid commentId, WorkCommentRequest workCommentRequest);

        /// <summary>
        /// Get Comment by Id
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public Task<bool> DeleteComment(Guid commentId);



    }
}
