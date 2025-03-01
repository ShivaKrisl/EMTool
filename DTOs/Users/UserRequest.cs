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
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

        public User ToUser()
        {
            return new User {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Username = Username,
            };
        }
    }
}
