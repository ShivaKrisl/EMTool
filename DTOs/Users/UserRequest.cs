using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfCore;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Users
{
    public class UserRequest
    {
        [Required]
        [MaxLength(100)]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string? LastName { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Username { get; set; }

        [Required]
        public Guid RoleId { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string? Password { get; set; }

        public User ToUser()
        {
            return new User {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Username = Username,
                RoleId = RoleId,
                PasswordHash = Password
            };
        }
    }
}
