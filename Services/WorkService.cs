using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service_Contracts;
using DTOs.Tasks;
using EfCore;
using DTOs.Users;
using DTOs.Teams;
using DTOs.TeamMembers;
using DTOs.Roles;

namespace Services
{

    public class WorkService : IWorkService
    {

        private readonly IUserService _userService;
        private readonly ITeamService _teamService;
        private readonly IRoleService _roleService;
        private readonly ITeamMemberService _memberService;
        private readonly List<Work> _works;

        public WorkService()
        {
            _userService = new UserService();
            _teamService = new TeamService();
            _roleService = new RoleService();
            _memberService = new TeamMemberService();
            _works = new List<Work>();
        }

        /// <summary>
        /// Create a new work (Task assigned by a Manager)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<WorkResponse> CreateWork(WorkRequest workRequest)
        {
            if(workRequest == null)
            {
                throw new ArgumentNullException(nameof(workRequest));
            }

            bool isModelValid = ValidationHelper.IsStateValid(workRequest);
            if (!isModelValid)
            {
                throw new ArgumentException("Invalid WorkRequest model");
            }

            bool workExists = _works.Any(w => w.Title == workRequest.Title && w.TeamId == workRequest.TeamId);
            if (workExists)
            {
                throw new ArgumentException("A task with the same title already exists in the Team.");
            }

            Work work = workRequest.ToWork();
            work.Id = Guid.NewGuid();

            UserResponse assignedByUserResponse = await _userService.GetEmployeeById(work.AssignedBy);

            if (assignedByUserResponse == null || assignedByUserResponse.userRole != UserRoles.Manager.ToString())
            {
                throw new ArgumentException("Only Managers can assign tasks");
            }

            UserResponse assignedToUserResponse = await _userService.GetEmployeeById(work.AssignedTo);
            if (assignedToUserResponse == null || assignedToUserResponse.userRole != UserRoles.Employee.ToString())
            {
                throw new ArgumentException("Only Employees can be assigned tasks");
            }

            // check if he exists in that Team

            TeamMemberResponse? assignedToResponse = await _memberService.GetTeamMemberByUserId(workRequest.AssignedTo);

            if(assignedToResponse == null || assignedToResponse.TeamId != workRequest.TeamId)
            {
                throw new ArgumentException("User does not belongs to same Team");
            }

            TeamMemberResponse? assignedByResponse = await _memberService.GetTeamMemberByUserId(workRequest.AssignedBy);

            if (assignedByResponse == null ||  assignedByResponse.TeamId != workRequest.TeamId)
            {
                throw new ArgumentException("Manager does not belongs to same Team");
            }

            RoleResponse? assignedByRoleResponse = await _roleService.GetRoleById(assignedByUserResponse.RoleId);

            if(assignedByRoleResponse == null || assignedByRoleResponse.Name != UserRoles.Manager.ToString())
            {
                throw new ArgumentException("Only Manager can add task");
            }

            RoleResponse? assignedToRoleResponse = await _roleService.GetRoleById(assignedToUserResponse.RoleId);

            work.CreatedAt = DateTime.UtcNow;
            work.AssignedBy = assignedByUserResponse.Id;
            work.Assigner = assignedByUserResponse.ToUser(assignedByRoleResponse.ToRole());
            work.AssignedTo = assignedToUserResponse.Id;
            work.Assignee = assignedToUserResponse.ToUser(assignedToRoleResponse.ToRole());

            TeamResponse teamResponse = await _teamService.GetTeamById(workRequest.TeamId);
            if (teamResponse == null)
            {
                throw new ArgumentException("Invalid Team Id");
            }
            work.TeamId = teamResponse.Id;

            _works.Add(work);

            return work.ToWorkResponse(assignedByUserResponse, assignedToUserResponse);

        }

        /// <summary>
        /// Get work by Id (Retrieve a specific task)
        /// </summary>
        /// <param name="workId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<WorkResponse> GetWorkById(Guid workId)
        {
           if(workId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(workId));
            }

            Work? work = _works.FirstOrDefault(w => w.Id == workId);
            if (work == null)
            {
                throw new ArgumentException("Task not found");
            }

            if (work.Assigner == null || work.Assignee == null)
            {
                throw new InvalidOperationException("Task is missing assigned users.");
            }

            WorkResponse workResponse = work.ToWorkResponse(work.Assigner.ToUserResponse(), work.Assignee.ToUserResponse());
            return Task.FromResult(workResponse);

        }

        /// <summary>
        /// Get all works assigned to a specific employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<WorkResponse>> GetEmployeeWorks(Guid employeeId)
        {
            if (employeeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(employeeId));
            }

            UserResponse userResponse = await _userService.GetEmployeeById(employeeId);

            if (userResponse == null)
            {
                throw new ArgumentException("Employee not found");
            }

            List<Work> employeeWorks = _works.Where(w => w.AssignedTo == employeeId).ToList();

            List<WorkResponse> workResponses = _works
               .Where(w => w.AssignedTo == employeeId)
               .Select(work => work.ToWorkResponse(work.Assigner.ToUserResponse(), work.Assignee.ToUserResponse()))
               .ToList();
            return workResponses;

        }

        /// <summary>
        /// Update an existing work (Only assigned tasks can be updated)
        /// </summary>
        /// <param name="workRequest"></param>
        /// <param name="workId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<WorkResponse> UpdateWork(WorkRequest workRequest, Guid workId, Guid userId)
        {
            if(workRequest == null)
            {
                throw new ArgumentNullException(nameof(workRequest));
            }

            if (workId == Guid.Empty) {
                throw new ArgumentNullException(nameof(workId));
            }

            if (userId == Guid.Empty) {
                throw new ArgumentNullException(nameof(userId));
            }

            bool isModelValid = ValidationHelper.IsStateValid(workRequest);
            if (!isModelValid)
            {
                throw new ArgumentException("Invalid WorkRequest model");
            }

            Work? work = _works.FirstOrDefault(w => w.Id == workId);
            if (work == null)
            {
                throw new ArgumentException("Task not found");
            }

            UserResponse updater = await _userService.GetEmployeeById(userId);
            if (updater == null)
            {
                throw new ArgumentException("User performing update not found");
            }

            TeamMemberResponse? teamMember = await _memberService.GetTeamMemberByUserId(userId);

            if(teamMember == null || teamMember.TeamId != workRequest.TeamId)
            {
                throw new ArgumentException("User does not belong to same team");
            }

            if (updater.userRole == UserRoles.Manager.ToString())
            {
                work.Title = workRequest.Title;
                work.Description = workRequest.Description;
                work.Status = workRequest.Status;
                work.Deadline = workRequest.Deadline;
                work.AssignedTo = workRequest.AssignedTo;
                work.TeamId = workRequest.TeamId;
                UserResponse Assignee = await _userService.GetEmployeeById(workRequest.AssignedTo);
                RoleResponse responseRole = await _roleService.GetRoleById(Assignee.RoleId);
                work.Assignee = Assignee.ToUser(responseRole.ToRole());

            }
            else if(updater.userRole == UserRoles.Employee.ToString())
            {
                if (work.AssignedTo != updater.Id)
                {
                    throw new ArgumentException("You can only update your assigned tasks");
                }
                work.Status = workRequest.Status;
            }

            else
            {
                throw new ArgumentException("Unauthorized user role");
            }

            return work.ToWorkResponse(work.Assigner.ToUserResponse(), work.Assignee.ToUserResponse());
        }

        /// <summary>
        /// Delete a work (Only managers can delete work)
        /// </summary>
        /// <param name="workId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<bool> DeleteWork(Guid workId)
        {
            if(workId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(workId));
            }

            Work? work = _works.FirstOrDefault(w => w.Id == workId);
            if (work == null)
            {
                return Task.FromResult(false);
            }
            if(work.Assignee.Role.Name != UserRoles.Manager.ToString())
            {
                throw new ArgumentException("Only Managers can delete tasks");
            }
            _works.Remove(work);
            return Task.FromResult(true);

        }
    }
}
