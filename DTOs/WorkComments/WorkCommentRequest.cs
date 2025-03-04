using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfCore;   

namespace DTOs.WorkComments
{
    public class WorkCommentRequest
    {
        /// <summary>
        /// Task Id
        /// </summary>
        [Required]
        public Guid TaskId { get; set; }

        /// <summary>
        /// User Id
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// Comment
        /// </summary>
        [Required]
        public string Comment { get; set; }

        /// <summary>
        /// Convert to WorkComment Entity
        /// </summary>
        /// <returns></returns>
        public WorkComment ToWorkComment()
        {
            return new WorkComment()
            {
                UserId = this.UserId,
                TaskId = this.TaskId,
                Comment = this.Comment,
            };
        }

    }
}
