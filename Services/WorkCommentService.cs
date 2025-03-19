using DTOs.Tasks;
using DTOs.WorkComments;
using EfCore;
using Service_Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.TeamMembers;
using DTOs.Users;
using DTOs.Roles;


namespace Services
{
    public class WorkCommentService : IWorkCommentService
    {
        private readonly List<WorkComment> _workComments;
        private readonly IWorkService _workService;
        private readonly ITeamMemberService _teamMemberService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public WorkCommentService(ITeamMemberService teamMemberService, IWorkService workService, IUserService userService, IRoleService roleService)
        {
            _workComments = new List<WorkComment>();
            _workService = workService;
            _teamMemberService = teamMemberService;
            _userService = userService;
            _roleService = roleService;
        }

        /// <summary>
        /// Add Comment on a Work
        /// </summary>
        /// <param name="workCommentRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<WorkCommentResponse> AddComment(WorkCommentRequest workCommentRequest)
        {
           if(workCommentRequest == null)
           {
                throw new ArgumentNullException(nameof(workCommentRequest));
           }
           bool isValidModel = ValidationHelper.IsStateValid(workCommentRequest);

            if (!isValidModel)
            {
                throw new ArgumentException("Invalid model state");
            }
            // check if comment is made on the task exists
            WorkResponse? workResponse =  await _workService.GetWorkById(workCommentRequest.TaskId);

            if (workResponse == null)
            {
                throw new ArgumentException("Task not found");
            }

            // check if the user is part of the team
            TeamMemberResponse? teamMemberResponse = await _teamMemberService.GetTeamMemberByUserId(workCommentRequest.UserId);
            UserResponse? userResponse = await _userService.GetEmployeeById(workCommentRequest.UserId);
            if (teamMemberResponse == null || userResponse == null)
            {
                throw new ArgumentException("User not found");
            }

            if(teamMemberResponse.TeamId != workResponse.TeamId)
            {
                throw new ArgumentException("User not part of the team");
            }


            WorkComment workComment = workCommentRequest.ToWorkComment();
            workComment.Id = Guid.NewGuid();
            workComment.CommentedOn = DateTime.UtcNow;
            RoleResponse? roleResponse = await _roleService.GetRoleById(userResponse.RoleId);
            if (roleResponse == null)
            {
                throw new ArgumentException("Role not found");
            }
            workComment.User = userResponse.ToUser(roleResponse.ToRole());
            workComment.Task = workResponse.ToWork(workResponse.AssignedBy,workResponse.AssignedTo,roleResponse);

            _workComments.Add(workComment);

            return workComment.ToWorkCommentResponse(workResponse, userResponse);
        }

        public async Task<List<WorkCommentResponse>?> GetCommentsOnWork(Guid taskId)
        {
           if(taskId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(taskId));
            }

           List<WorkComment> workComments = _workComments.Where(x => x.TaskId == taskId).ToList();
            if (!workComments.Any())
            {
                return null;
            }
            List<WorkCommentResponse> workCommentResponses = new List<WorkCommentResponse>();
            foreach(WorkComment workComment in workComments)
            {
                WorkResponse? workResponse = await _workService.GetWorkById(taskId);
                UserResponse? userResponse =  await _userService.GetEmployeeById(workComment.UserId);

                workCommentResponses.Add(workComment.ToWorkCommentResponse(workResponse, userResponse));
            }
            return workCommentResponses;
        }

        public async Task<WorkCommentResponse> EditComment(Guid commentId, WorkCommentRequest workCommentRequest)
        {
            if(commentId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(commentId));
            }

            if (workCommentRequest == null)
            {
                throw new ArgumentNullException(nameof(workCommentRequest));
            }

            bool isValidModel = ValidationHelper.IsStateValid(workCommentRequest);
            if (!isValidModel)
            {
                throw new ArgumentException("Invalid Request");
            }

            WorkComment? workComment = _workComments.FirstOrDefault(c => c.Equals(commentId));
            if (workComment == null)
            {
                throw new ArgumentException("Comment doest not exists!!");
            }

            if (workComment.UserId != workCommentRequest.UserId)
            {
                throw new ArgumentException("Only Comment created user can create the task!");
            }

            if (workComment.TaskId != workCommentRequest.TaskId)
            {
                throw new ArgumentException("Only Comment from the same task can be edited!");
            }

            UserResponse? userResponse = await _userService.GetEmployeeById(workCommentRequest.UserId);
            if (userResponse == null)
            {
                throw new ArgumentException("User not found!");
            }

            RoleResponse? roleResponse =  await _roleService.GetRoleById(userResponse.RoleId);

            WorkResponse? workResponse =  await _workService.GetWorkById(workCommentRequest.UserId);

            if (roleResponse == null)
            {
                throw new ArgumentException("Role not found!");
            }

            if (workResponse == null)
            {
                throw new ArgumentException("Task not found!");
            }

            WorkComment workCommentToEdit = workCommentRequest.ToWorkComment();
            workCommentToEdit.User = userResponse.ToUser(roleResponse.ToRole());
            workCommentToEdit.Task = workResponse.ToWork(workResponse.AssignedBy, userResponse, roleResponse);
            workCommentToEdit.CommentedOn = DateTime.UtcNow;
            workCommentToEdit.Comment = workCommentRequest.Comment;
           
            return workCommentToEdit.ToWorkCommentResponse(workResponse, userResponse);
        }

        public Task<bool> DeleteComment(Guid commentId)
        {
            if (commentId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(commentId));
            }

            WorkComment? workComment = _workComments.FirstOrDefault(x => x.Id == commentId);

            if (workComment == null) {
                return Task.FromResult(false);
            }

            _workComments.Remove(workComment);
            return Task.FromResult(true);

        }
        
    }
}
