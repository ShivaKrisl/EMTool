using System;
using System.Collections.Generic;

namespace DTOs.Reports
{
    public class ReportResponse
    {
        public Guid TeamId { get; set; }



        public int TotalTasksAssigned { get; set; }
        public int CompletedTasks { get; set; }
        public int PendingTasks { get; set; }
        public int TotalPRsRaised { get; set; }
        public int MergedPRs { get; set; }

        public int MeetingsHeld { get; set; }

        public List<EmployeePerformance> TopPerformers { get; set; } = new List<EmployeePerformance>();
    }

    public class EmployeePerformance
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int TasksCompleted { get; set; }
        public int PRsMerged { get; set; }
        public int MeetingsAttended { get; set; }
    }
}
