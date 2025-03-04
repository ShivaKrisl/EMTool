using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.Roles;
using DTOs.Users;
using EfCore;

namespace DTOs.Tasks
{
    public class WorkResponse
    {
        /// <summary>
        /// Task Id
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Task Title
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        /// <summary>
        /// Task Description
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Task assigned by Id (Manager Id)
        /// </summary>
        [Required]
        public UserResponse AssignedBy { get; set; }

        /// <summary>
        /// Task assigned to Id (Employee Id)
        /// </summary>
        [Required]
        public UserResponse AssignedTo { get; set; }

        /// <summary>
        /// Team Id
        /// </summary>
        [Required]
        public Guid TeamId { get; set; }

        /// <summary>
        /// Task Status
        /// </summary>
        [Required]
        public string Status { get; set; }

        /// <summary>
        /// Task deadline
        /// </summary>
        [Required]
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Task created at
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }

    public static class WorkResponseExtensions
    {
        public static WorkResponse ToWorkResponse(this Work work, UserResponse assignedBy, UserResponse assignedTo)
        {
            return new WorkResponse
            {
                Id = work.Id,
                Title = work.Title,
                Description = work.Description,
                AssignedBy = assignedBy,
                AssignedTo = assignedTo,
                TeamId = work.TeamId,
                Status = work.Status,
                Deadline = work.Deadline,
                CreatedAt = work.CreatedAt
            };
        }

        public static Work ToWork(this WorkResponse workResponse, UserResponse AssignedBy, UserResponse AssignedTo, RoleResponse roleResponse)
        {
            return new Work
            {
                Id = workResponse.Id,
                Title = workResponse.Title,
                Description = workResponse.Description,
                TeamId = workResponse.TeamId,
                Status = workResponse.Status,
                Deadline = workResponse.Deadline,
                CreatedAt = workResponse.CreatedAt,
                Assigner = AssignedBy.ToUser(roleResponse.ToRole()),
                Assignee = AssignedTo.ToUser(roleResponse.ToRole()),
                AssignedBy = AssignedBy.Id,
                AssignedTo = AssignedTo.Id
            };
        }
    }
}
