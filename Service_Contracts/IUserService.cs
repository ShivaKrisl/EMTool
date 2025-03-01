using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.Tasks;
using DTOs.Users;

namespace Service_Contracts
{
    public interface IUserService
    {

        /// <summary>
        /// Register a new user as Manager
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        public Task<UserResponse> RegisterManager(UserRequest userRequest);

        /// <summary>
        /// Register a new user as Employee
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        public Task<UserResponse> RegisterEmployee(UserRequest userRequest);

        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns></returns>
        public Task<List<UserResponse>>? GetAllEmployees();

        /// <summary>
        /// Get Employee by username
        /// </summary>
        /// <param name="employeeUserName"></param>
        /// <returns></returns>
        public Task<List<UserResponse>>? GetEmployeeByUserName(string? employeeUserName);

        /// <summary>
        /// Update Employee details
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        public Task<UserResponse> UpdateEmployeeDetails(UserRequest userRequest);

        /// <summary>
        /// Delete Employee by Id
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public Task<bool> DeleteUser(Guid userId);

    }
}
