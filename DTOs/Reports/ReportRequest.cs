using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Reports
{
    public class ReportRequest
    {
        [Required]
        public Guid TeamId { get; set; }  // The team for which the report is generated

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
