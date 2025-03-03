using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using EfCore;

namespace DTOs.Users
{
    public class UserResponse
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        public string? userRole { get; set; }

        [Required]
        public Guid RoleId { get; set; }
    }

    public static class UserResponseExtensions
    {
        public static UserResponse ToUserResponse(this User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username,
                userRole = user.Role.Name,
                RoleId = user.RoleId
            };
        }

        public static User ToUser(this UserResponse userResponse, Role role)
        {
            return new User
            {
                Id = userResponse.Id,
                FirstName = userResponse.FirstName,
                LastName = userResponse.LastName,
                Email = userResponse.Email,
                Username = userResponse.Username,
                RoleId = role.Id,
                Role = role
            };
        }
    }
}
