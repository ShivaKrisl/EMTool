using DTOs.Reports;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service_Contracts
{
    public interface IReportService
    {
        /// <summary>
        /// Get overall team performance report within a specific time range
        /// </summary>
        /// <param name="reportRequest"></param>
        /// <returns></returns>
        public Task<ReportResponse> GetTeamReport(ReportRequest reportRequest);

        /// <summary>
        /// Get an individual employee's performance report
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public Task<EmployeePerformance> GetEmployeeReport(Guid employeeId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Get top-performing employees based on completed tasks and merged PRs
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public Task<List<EmployeePerformance>> GetTopPerformers(Guid teamId, DateTime startDate, DateTime endDate);
    }
}
