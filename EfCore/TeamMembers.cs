using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCore
{
    public class TeamMembers
    {
        /// <summary>
        /// Team Members Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Team Id
        /// </summary>
        [Required]
        public Guid TeamId { get; set; }

        /// <summary>
        /// User Id
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        // Navigation Properties

        /// <summary>
        /// Team
        /// </summary>
        [ForeignKey("TeamId")]
        public Teams teams { get; set; }

        /// <summary>
        /// User
        /// </summary>
        [ForeignKey("UserId")]
        public Users users { get; set; }


    }
}
