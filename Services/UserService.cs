using DTOs.Users;
using Service_Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfCore;
using BCrypt.Net;

namespace Services
{
    public class UserService : IUserService
    {

        private readonly List<User> _users;
        private readonly List<Role> _mockroles;

        public UserService()
        {
            _users = new List<User>();
            _mockroles= new List<Role>
            {
                new Role { Id = Guid.NewGuid(), Name = UserRoles.Manager.ToString() },
                new Role { Id = Guid.NewGuid(), Name = UserRoles.Employee.ToString() }
            };
        }

        /// <summary>
        /// Register a new user as Manager
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>

        public Task<UserResponse> RegisterManager(UserRequest userRequest)
        {
            if(userRequest == null)
            {
                throw new ArgumentNullException(nameof(userRequest));
            }

            bool isModelValid = ValidationHelper.IsStateValid(userRequest);
            if (!isModelValid)
            {
                throw new ArgumentException("UserRequest is not valid");
            }

            User manager = userRequest.ToUser();
            manager.Id = Guid.NewGuid();
            manager.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRequest.Password);
            // To verify use BCrypt.Net.BCrypt.Verify(userRequest.Password, hashedPassword)
            
            Role? ManagerRole = _mockroles.FirstOrDefault(r => r.Name == UserRoles.Manager.ToString());
            if (ManagerRole == null)
            {
                throw new ArgumentException("Manager Role not found");
            }
            manager.Role = ManagerRole;
            manager.RoleId = ManagerRole.Id;
            manager.CreatedAt = DateTime.UtcNow;
            manager.UpdatedAt = DateTime.UtcNow;

            // Save to database
            _users.Add(manager);

            return Task.FromResult(manager.ToUserResponse());

        }

        /// <summary>
        /// Register a new user as Employee
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<UserResponse> RegisterEmployee(UserRequest userRequest)
        {
            if (userRequest == null)
            {
                throw new ArgumentNullException(nameof(userRequest));
            }

            bool isModelValid = ValidationHelper.IsStateValid(userRequest);
            if (!isModelValid)
            {
                throw new ArgumentException("UserRequest is not valid");
            }

            User employee = userRequest.ToUser();
            employee.Id = new Guid();
            employee.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRequest.Password);
            // To verify use BCrypt.Net.BCrypt.Verify(userRequest.Password, hashedPassword)

            Role? employeeRole = _mockroles.FirstOrDefault(role => role.Name == UserRoles.Employee.ToString());

            if(employeeRole == null)
            {
                throw new ArgumentException("Employee Role not found");
            }

            employee.Role = employeeRole;
            employee.RoleId = employeeRole.Id;
            employee.CreatedAt = DateTime.UtcNow;
            employee.UpdatedAt = DateTime.UtcNow;

            // Save to database
            _users.Add(employee);

            return Task.FromResult(employee.ToUserResponse());

        }

        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<List<UserResponse>> GetAllEmployees()
        {
            if(_users == null)
            {
                return Task.FromResult(new List<UserResponse>());
            }
            return Task.FromResult(_users.Select(u => u.ToUserResponse()).ToList());
        }

        /// <summary>
        /// Get employee by username
        /// </summary>
        /// <param name="employeeUserName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<List<UserResponse>> GetEmployeeByUserName(string? employeeUserName)
        {
            if (string.IsNullOrEmpty(employeeUserName))
            {
                return this.GetAllEmployees();
            }

            List<User> filteredUsers = _users.Where(u => u.Username == employeeUserName).ToList();

            if (filteredUsers == null)
            {
                return Task.FromResult(new List<UserResponse>());
            }

            List<UserResponse> userResponses = filteredUsers.Select(u => u.ToUserResponse()).ToList();

            return Task.FromResult(userResponses);
        }

        /// <summary>
        /// Get employee by Id
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Task<UserResponse> GetEmployeeById(Guid UserId)
        {
            if (UserId == Guid.Empty)
            {
                throw new ArgumentException("Invalid User ID", nameof(UserId));
            }

            User? user = _users.FirstOrDefault(u => u.Id == UserId);
            if (user == null)
            {
                return Task.FromResult(new UserResponse());
            }
            return Task.FromResult(user.ToUserResponse());
        }

        /// <summary>
        /// Update employee details
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<UserResponse> UpdateEmployeeDetails(UserRequest userRequest)
        {
            if (userRequest == null)
            {
                throw new ArgumentNullException(nameof(userRequest));
            }

            bool isModelValid = ValidationHelper.IsStateValid(userRequest);
            if (!isModelValid)
            {
                throw new ArgumentException("UserRequest is not valid");
            }

            User? userToUpdate = _users.FirstOrDefault(u => u.Email == userRequest.Email);

            if (userToUpdate == null)
            {
                throw new ArgumentException("User not found");
            }

            userToUpdate.FirstName = userRequest.FirstName;
            userToUpdate.LastName = userRequest.LastName;
            userToUpdate.Username = userRequest.Username;
            userToUpdate.UpdatedAt = DateTime.UtcNow;

            return Task.FromResult(userToUpdate.ToUserResponse());

        }

        /// <summary>
        /// Delete employee by Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public Task<bool> DeleteUser(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("Invalid user ID", nameof(userId));
            }

            User? userToDelete = _users.FirstOrDefault(u => u.Id == userId);

            if(userToDelete == null)
            {
                return Task.FromResult(false);
            }

            _users.Remove(userToDelete);
            return Task.FromResult(true);
        }
        
    }
}
