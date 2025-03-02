using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using EfCore;

namespace DTOs.Teams
{
    public class TeamRequest
    {
        /// <summary>
        /// Team Name
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string TeamName { get; set; }

        /// <summary>
        /// Team Manager Id
        /// </summary>
        [Required]
        public Guid ManagerId { get; set; }

        public Team ToTeam()
        {
            return new Team
            {
                TeamName = TeamName,
                ManagerId = ManagerId
            };
        }
    }
}
