using DTOs.WorkAttachments;
using EfCore;
using Service_Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.Users;
using DTOs.Roles;
using DTOs.Tasks;

namespace Services
{
    public class WorkAttachmentService : IWorkAttachmentService
    {

        private readonly List<WorkAttachment> _workAttachments;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IWorkService _workService;

        public WorkAttachmentService(IUserService userService, IRoleService roleService, IWorkService workService)
        {
            _workAttachments = new List<WorkAttachment>();
            _userService = userService;
            _roleService = roleService;
            _workService = workService;
        }

        /// <summary>
        /// Upload a File
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<WorkAttachmentResponse> UploadFile(WorkAttachmentRequest workAttachmentRequest)
        {
            if(workAttachmentRequest == null)
            {
                throw new ArgumentNullException(nameof(workAttachmentRequest));
            }

            bool isModelValid = ValidationHelper.IsStateValid(workAttachmentRequest);

            if (!isModelValid)
            {
                throw new ArgumentException("Invaid Work Attachment");
            }

            UserResponse? userResponse = await _userService.GetEmployeeById(workAttachmentRequest.UserId);

            if (userResponse == null)
            {
                throw new ArgumentException("User Not Found!");
            }

            RoleResponse? roleResponse = await _roleService.GetRoleById(userResponse.RoleId);

            if (roleResponse == null)
            {
                throw new ArgumentException("Role Not Found!");
            }
            WorkResponse? workResponse = await _workService.GetWorkById(workAttachmentRequest.TaskId);
            if (workResponse == null)
            {
                throw new ArgumentException("Work Not Found!");
            }

            // Check if the user is assigned to the task
            if (workResponse.AssignedTo.Id != workAttachmentRequest.UserId)
            {
                throw new ArgumentException("User is not assigned to this task!");
            }

            WorkAttachment workAttachment = workAttachmentRequest.ToWorkAttachment();
            workAttachment.Id = Guid.NewGuid();

            workAttachment.CreatedAt = DateTime.UtcNow;
            workAttachment.User = userResponse.ToUser(roleResponse.ToRole());
            workAttachment.UserId = userResponse.Id;
            workAttachment.TaskId = workAttachmentRequest.TaskId;
            workAttachment.Task = workResponse.ToWork(workResponse.AssignedBy, workResponse.AssignedTo, roleResponse);

            _workAttachments.Add(workAttachment);

            return workAttachment.ToWorkAttachmentResponse(workResponse, userResponse);
        }

        /// <summary>
        /// Get Uploaded Files of Task
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<WorkAttachmentResponse>?> GetUploadedFilesOfTask(Guid taskId, Guid UserId)
        {
            if(taskId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(taskId));
            }

            WorkResponse? workResponse = await _workService.GetWorkById(taskId);
            if (workResponse == null)
            {
                throw new ArgumentException("Task Not Found!");
            }

            UserResponse? userResponse1 = await _userService.GetEmployeeById(UserId);
            if (userResponse1 == null)
            {
                throw new ArgumentException("User Not Found!");
            }
            if(userResponse1.userRole != UserRoles.Manager.ToString())
            {
                throw new ArgumentException("Only Manager can see all file attachments of Task");
            }

            List<WorkAttachment>? workAttachments = _workAttachments.Where(wa => wa.TaskId == taskId).ToList();

            if(!workAttachments.Any())
            {
                return null;
            }

            List<WorkAttachmentResponse> workAttachmentResponses = new List<WorkAttachmentResponse>();
            foreach (WorkAttachment workAttachment in workAttachments)
            {
                UserResponse userResponse = await _userService.GetEmployeeById(workAttachment.UserId);
                workAttachmentResponses.Add(workAttachment.ToWorkAttachmentResponse(workResponse, userResponse));
            }
            return workAttachmentResponses;
        }

        /// <summary>
        /// Get Uploaded Files of User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<WorkAttachmentResponse>?> GetUploadedFilesOfUser(Guid userId)
        {
            if(userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            UserResponse? userResponse = await _userService.GetEmployeeById(userId);
            if (userResponse == null)
            {
                throw new ArgumentException("User Not Found!");
            }

            List<WorkAttachment>? workAttachments = _workAttachments.Where(wa => wa.UserId == userId).ToList();

            if (workAttachments == null)
            {
                return null;
            }

            List<WorkAttachmentResponse> workAttachmentResponses = new List<WorkAttachmentResponse>();
            foreach(WorkAttachment workAttachment in workAttachments)
            {
                WorkResponse workResponse = await _workService.GetWorkById(workAttachment.TaskId);
                workAttachmentResponses.Add(workAttachment.ToWorkAttachmentResponse(workResponse, userResponse));
            }
            return workAttachmentResponses;

        }

        /// <summary>
        /// Search File By Name
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<List<WorkAttachmentResponse>?> SearchFileByName(string fileName)
        {
            if(string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            List<WorkAttachment>? workAttachments = _workAttachments.Where(wa => wa.FileName.Contains(fileName)).ToList();

            if (workAttachments == null)
            {
                return null;
            }

            List<WorkAttachmentResponse> workAttachmentResponses = new List<WorkAttachmentResponse>();
            foreach (WorkAttachment workAttachment in workAttachments)
            {
                WorkResponse workResponse = _workService.GetWorkById(workAttachment.TaskId).Result;
                UserResponse userResponse = _userService.GetEmployeeById(workAttachment.UserId).Result;
                workAttachmentResponses.Add(workAttachment.ToWorkAttachmentResponse(workResponse, userResponse));
            }
            return Task.FromResult<List<WorkAttachmentResponse>?>(workAttachmentResponses);

        }

        /// <summary>
        /// Edit File By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<WorkAttachmentResponse> EditFileById(Guid id, WorkAttachmentRequest workAttachmentRequest)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (workAttachmentRequest == null)
            {
                throw new ArgumentNullException(nameof(workAttachmentRequest));
            }

            bool isModelValid = ValidationHelper.IsStateValid(workAttachmentRequest);
            if (isModelValid)
            {
                throw new ArgumentException("Invalid Work Attachment");
            }

            WorkAttachment? workAttachment = _workAttachments.FirstOrDefault(wa => wa.Id == id);
            if (workAttachment == null)
            {
                throw new ArgumentException("File Not Found!");
            }

            if (workAttachment.UserId != workAttachmentRequest.UserId)
            {
                throw new ArgumentException("User is not authorized to edit this file!");
            }

            if (workAttachment.TaskId != workAttachmentRequest.TaskId)
            {
                throw new ArgumentException("Task Id cannot be changed!");
            }

            workAttachment.FileName = workAttachmentRequest.FileName;
            workAttachment.FilePath = workAttachmentRequest.FilePath;
            workAttachment.FileType = workAttachmentRequest.FileType;
            workAttachment.CreatedAt = DateTime.UtcNow;

            UserResponse? userResponse = await _userService.GetEmployeeById(workAttachment.UserId);

            if (userResponse == null)
            {
                throw new ArgumentException("User Not Found!");
            }
            WorkResponse? workResponse = await _workService.GetWorkById(workAttachment.TaskId);
            if (workResponse == null)
            {
                throw new ArgumentException("Work Not Found!");
            }
            return workAttachment.ToWorkAttachmentResponse(workResponse, userResponse);

        }

        /// <summary>
        /// Delete File By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public Task<bool> DeleteFile(Guid id)
        {
            if(id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            WorkAttachment? workAttachment = _workAttachments.FirstOrDefault(wa => wa.Id == id);
            if (workAttachment == null)
            {
                throw new ArgumentException("File Not Found!");
            }

            _workAttachments.Remove(workAttachment);
            return Task.FromResult(true);
        }

        /// <summary>
        /// Download File By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<byte[]> DownloadFile(Guid id)
        {
            if(id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            WorkAttachment? workAttachment = _workAttachments.FirstOrDefault(wa => wa.Id == id);
            if (workAttachment == null)
            {
                throw new ArgumentException("File Not Found!");
            }

            return Task.FromResult(Encoding.ASCII.GetBytes(workAttachment.FilePath));

        }

        
    }
}
