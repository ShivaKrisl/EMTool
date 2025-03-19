using DTOs.Users;
using Service_Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfCore;
using BCrypt.Net;
using DTOs.Roles;

namespace Services
{
    public class UserService : IUserService
    {

        private readonly List<User> _users;
        private readonly IRoleService _roleService;

        public UserService(IRoleService roleService)
        {
            _users = new List<User>();
            _roleService = roleService;
        }

        /// <summary>
        /// Register a new user as Manager
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>

        public async Task<UserResponse> RegisterManager(UserRequest userRequest)
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

            // Check Username already exists
            if (_users.Any(u => u.Username == userRequest.Username))
            {
                throw new ArgumentException("Username already exists");
            }

            // Check Email already exists
            if (_users.Any(u => u.Email == userRequest.Email))
            {
                throw new ArgumentException("Email already exists");
            }

            User manager = userRequest.ToUser();
            manager.Id = Guid.NewGuid();
            manager.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRequest.Password);
            // To verify use BCrypt.Net.BCrypt.Verify(userRequest.Password, hashedPassword)

            RoleResponse? ManagerRole = await _roleService.GetRoleById(userRequest.RoleId);

            if (ManagerRole == null)
            {
                ManagerRole = await _roleService.CreateRole(new RoleRequest()
                {
                    Name = UserRoles.Manager.ToString()
                });
            }
            if(ManagerRole.Name != UserRoles.Manager.ToString())
            {
                throw new ArgumentException("Only Manager can create a Team");
            }
            manager.Role = ManagerRole.ToRole();
            manager.RoleId = ManagerRole.Id;
            manager.CreatedAt = DateTime.UtcNow;
            manager.UpdatedAt = DateTime.UtcNow;

            // Save to database
            _users.Add(manager);

            return manager.ToUserResponse();

        }

        /// <summary>
        /// Register a new user as Employee
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<UserResponse> RegisterEmployee(UserRequest userRequest)
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

            // check user name already exists
            if (_users.Any(u => u.Username == userRequest.Username))
            {
                throw new ArgumentException("Username already exists");
            }

            // check email already exists
            if (_users.Any(u => u.Email == userRequest.Email))
            {
                throw new ArgumentException("Email already exists");
            }

            User employee = userRequest.ToUser();
            employee.Id = Guid.NewGuid();
            employee.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRequest.Password);
            // To verify use BCrypt.Net.BCrypt.Verify(userRequest.Password, hashedPassword)

            RoleResponse? employeeRole = await _roleService.GetRoleById(userRequest.RoleId);

            if (employeeRole == null)
            {
                employeeRole = await _roleService.CreateRole(new RoleRequest()
                {
                    Name = UserRoles.Employee.ToString()
                });
            }

            employee.Role = employeeRole.ToRole();
            employee.RoleId = employeeRole.Id;
            employee.CreatedAt = DateTime.UtcNow;
            employee.UpdatedAt = DateTime.UtcNow;

            // Save to database
            _users.Add(employee);

            return employee.ToUserResponse();

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
        public Task<UserResponse> GetEmployeeByUserName(string? employeeUserName)
        {
            if (string.IsNullOrEmpty(employeeUserName))
            {
                throw new ArgumentNullException(nameof(employeeUserName));
            }

            User? filteredUser = _users.FirstOrDefault(u => u.Username == employeeUserName);

            if (filteredUser == null)
            {
                throw new ArgumentException("User not found");
            }

            UserResponse userResponse = filteredUser.ToUserResponse();

            return Task.FromResult(userResponse);
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
        public Task<UserResponse> UpdateEmployeeDetails(UserRequest userRequest, Guid userId)
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

            User? userToUpdate = _users.FirstOrDefault(u => u.Id == userId);

            if (userToUpdate == null)
            {
                throw new ArgumentException("User not found");
            }

            // Check Username already exists
            if (_users.Any(u => u.Username == userRequest.Username && u.Id != userId))
            {
                throw new ArgumentException("Username already exists");
            }

            // Check Email already exists
            if (_users.Any(u => u.Email == userRequest.Email && u.Id != userId))
            {
                throw new ArgumentException("Email already exists");
            }

            userToUpdate.FirstName = userRequest.FirstName;
            userToUpdate.Email = userRequest.Email;
            userToUpdate.Username = userRequest.Username;
            userToUpdate.LastName = userRequest.LastName;
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
