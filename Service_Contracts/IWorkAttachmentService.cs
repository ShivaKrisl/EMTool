using DTOs.WorkAttachments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Contracts
{
    public interface IWorkAttachmentService
    {
        /// <summary>
        /// Upload File
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<WorkAttachmentResponse> UploadFile(WorkAttachmentRequest request);

        /// <summary>
        /// Get Uploaded Files of User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<List<WorkAttachmentResponse>?> GetUploadedFilesOfUser(Guid userId);

        /// <summary>
        /// Get Uploaded Files of Task
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public Task<List<WorkAttachmentResponse>?> GetUploadedFilesOfTask(Guid taskId, Guid userId);

        /// <summary>
        /// Search File By Name
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Task<List<WorkAttachmentResponse>?> SearchFileByName(string fileName);

        /// <summary>
        /// Edit File By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<WorkAttachmentResponse> EditFileById(Guid id, WorkAttachmentRequest request);

        /// <summary>
        /// Delete File By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> DeleteFile(Guid id);

        /// <summary>
        /// Download File
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<byte[]> DownloadFile(Guid id);
    }
}
