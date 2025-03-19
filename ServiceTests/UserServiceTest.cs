using AutoFixture;
using Service_Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services;
using DTOs.Users;
using DTOs.Roles;
using EfCore;

namespace Testing
{
    public class UserServiceTest
    {
        private readonly IUserService _userService;

        private readonly IRoleService _roleService;

        private readonly IFixture _fixture;

        public UserServiceTest() {
            _roleService = new RoleService();
            _userService = new UserService(_roleService);
            _fixture = new Fixture();
        }

        #region  Register Manager

        /// <summary>
        /// Register a new user as Manager with Null request
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RegisterManager_NullRequest_ToBeArgumentNullException()
        {
            //Arrange
            UserRequest? userRequest = null;

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _userService.RegisterManager(userRequest);
            });
        }

        /// <summary>
        /// Register a new user as Manager with Null Username
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RegisterManager_UserNameNull_ToBeArgumentException()
        {
            // Arrange
            UserRequest userRequest = _fixture.Build<UserRequest>()
            .With(x => x.Username, null as string)
            .With(x => x.Email, "s@gmail.com")
            .Create();

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _userService.RegisterManager(userRequest);
            });
        }

        /// <summary>
        /// Register a new user as Manager with Null Email
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RegisterManager_EmailNull_ToBeArgumentNullException()
        {
            // Arrange
            UserRequest userRequest = _fixture.Build<UserRequest>()
            .With(x => x.Email, null as string)
            .Create();

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _userService.RegisterManager(userRequest);
            });
        }

        /// <summary>
        /// Register a new user as Manager with Null Password
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RegisterManager_PasswordNull_ToBeArgumentNullException()
        {
            // Arrange
            UserRequest userRequest = _fixture.Build<UserRequest>()
            .With(x => x.Password, null as string)
            .With(x => x.Email, "s@gmail.com")
            .Create();

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _userService.RegisterManager(userRequest);
            });
        }

        /// <summary>
        /// Register a new user as Manager with different Role
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RegisterManager_InvalidRole_ToBeArgumentException()
        {
            // Arrange
            RoleRequest roleRequest = _fixture.Build<RoleRequest>()
            .With(x => x.Name,UserRoles.Employee.ToString())
            .Create();

            UserRequest userRequest = _fixture.Build<UserRequest>()
            .With(x => x.Email, "S@gmail.com")
            .Create();

            RoleResponse roleResponse = await _roleService.CreateRole(roleRequest);

            userRequest.RoleId = roleResponse.Id;

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _userService.RegisterManager(userRequest);
            });
        }

        /// <summary>
        /// Register a new user as Manager with Duplicate Username
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RegisterManager_DuplicateUserName_ToBeArgumentException()
        {
            // Arrange
            UserRequest userRequest1 = _fixture.Build<UserRequest>()
            .With(x => x.Username, "sky")
            .With(x => x.Email, "s@gmail.com")
            .Create();

            UserRequest userRequest2 = _fixture.Build<UserRequest>()
                .With(x => x.Username, "sky")
                .With(x => x.Email, "k@gmail.com")
                .Create();

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _userService.RegisterManager(userRequest1);
                await _userService.RegisterManager(userRequest2);
            });
        }

        /// <summary>
        /// Register a new user as Manager with Duplicate Email
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RegisterManager_DuplicateEmail_ToBeArgumentException()
        {
            // Arrange
            UserRequest userRequest1 = _fixture.Build<UserRequest>()
            .With(x => x.Email, "s@gmail.com")
            .Create();

            UserRequest userRequest2 = _fixture.Build<UserRequest>()
            .With(x => x.Email, "s@gmail.com")
            .Create();

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _userService.RegisterManager(userRequest1);
                await _userService.RegisterManager(userRequest2);
            });
        }

        /// <summary>
        /// Register a new user as Manager with Valid Request
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RegisterManager_ValidRequest_ToBeSuccess()
        {
            // Arrange
            RoleRequest roleRequest = _fixture.Build<RoleRequest>()
            .With(x => x.Name, UserRoles.Manager.ToString())
            .Create();

            UserRequest userRequest = _fixture.Build<UserRequest>()
            .With(x => x.Email, "s@gmail.com")
            .Create();

            RoleResponse roleResponse = await _roleService.CreateRole(roleRequest);
            userRequest.RoleId = roleResponse.Id;

            // Act
            UserResponse userResponse = await _userService.RegisterManager(userRequest);

            // Assert
            Assert.NotNull(userResponse);
            Assert.True(userResponse.Id != Guid.Empty);
        }
        #endregion

        #region Register Employee

        /// <summary>
        /// Register a new user as Employee with Null request
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RegisterEmployee_NullRequest_ToBeArgumentNullException()
        {
            //Arrange
            UserRequest? userRequest = null;

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _userService.RegisterEmployee(userRequest);
            });
        }

        /// <summary>
        /// Register a new user as Employee with Null Username
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RegisterEmployee_UserNameNull_ToBeArgumentException()
        {
            // Arrange
            UserRequest userRequest = _fixture.Build<UserRequest>()
            .With(x => x.Username, null as string)
            .With(x => x.Email, "s@gmail.com")
            .Create();

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _userService.RegisterEmployee(userRequest);
            });
        }

        /// <summary>
        /// Register a new user as Employee with Null Email
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RegisterEmployee_EmailNull_ToBeArgumentNullException()
        {
            // Arrange
            UserRequest userRequest = _fixture.Build<UserRequest>()
            .With(x => x.Email, null as string)
            .Create();

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _userService.RegisterEmployee(userRequest);
            });
        }

        /// <summary>
        /// Register a new user as Employee with Null Password
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RegisterEmployee_PasswordNull_ToBeArgumentNullException()
        {
            // Arrange
            UserRequest userRequest = _fixture.Build<UserRequest>()
            .With(x => x.Password, null as string)
            .With(x => x.Email, "s@gmail.com")
            .Create();

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _userService.RegisterEmployee(userRequest);
            });
        }

        /// <summary>
        /// Register a new user as Employee with different Role
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RegisterEmployee_InvalidRole_ToBeArgumentException()
        {
            // Arrange
            RoleRequest roleRequest = _fixture.Build<RoleRequest>()
            .With(x => x.Name, UserRoles.Manager.ToString())
            .Create();

            UserRequest userRequest = _fixture.Build<UserRequest>()
            .With(x => x.Email, "S@gmail.com")
            .Create();

            RoleResponse roleResponse = await _roleService.CreateRole(roleRequest);

            userRequest.RoleId = roleResponse.Id;

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _userService.RegisterEmployee(userRequest);
            });
        }

        /// <summary>
        /// Register a new user as Employee with Duplicate Username
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RegisterEmployee_DuplicateUserName_ToBeArgumentException()
        {
            // Arrange
            UserRequest userRequest1 = _fixture.Build<UserRequest>()
            .With(x => x.Username, "sky")
            .With(x => x.Email, "s@gmail.com")
            .Create();

            UserRequest userRequest2 = _fixture.Build<UserRequest>()
                .With(x => x.Username, "sky")
                .With(x => x.Email, "k@gmail.com")
                .Create();

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _userService.RegisterEmployee(userRequest1);
                await _userService.RegisterEmployee(userRequest2);
            });
        }

        /// <summary>
        /// Register a new user as Employee with Duplicate Email
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RegisterEmployee_DuplicateEmail_ToBeArgumentException()
        {
            // Arrange
            UserRequest userRequest1 = _fixture.Build<UserRequest>()
            .With(x => x.Email, "s@gmail.com")
            .Create();

            UserRequest userRequest2 = _fixture.Build<UserRequest>()
            .With(x => x.Email, "s@gmail.com")
            .Create();

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _userService.RegisterEmployee(userRequest1);
                await _userService.RegisterEmployee(userRequest2);
            });
        }

        /// <summary>
        /// Register a new user as Employee with Valid Request
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RegisterEmployee_ValidRequest_ToBeSuccess()
        {
            // Arrange
            RoleRequest roleRequest = _fixture.Build<RoleRequest>()
            .With(x => x.Name, UserRoles.Employee.ToString())
            .Create();

            UserRequest userRequest = _fixture.Build<UserRequest>()
            .With(x => x.Email, "s@gmail.com")
            .Create();

            RoleResponse roleResponse = await _roleService.CreateRole(roleRequest);
            userRequest.RoleId = roleResponse.Id;

            // Act
            UserResponse userResponse = await _userService.RegisterEmployee(userRequest);

            // Assert
            Assert.NotNull(userResponse);
            Assert.True(userResponse.Id != Guid.Empty);
        }

        #endregion

        #region Get All Employees

        /// <summary>
        /// Get all employees with Empty List
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllEmployees_EmptyList_ToBeEmpty()
        {
            // Act
           List<UserResponse>? userResponses =  await _userService.GetAllEmployees();

            // Assert
            Assert.True(userResponses.Count == 0);
            Assert.Empty(userResponses);
        }

        /// <summary>
        /// Get all employees with List Exists
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllEmployees_ListExists_ToBeSuccess()
        {
            // Arrange

            RoleRequest roleRequest1 = _fixture.Build<RoleRequest>()
            .With(r => r.Name, UserRoles.Manager.ToString())
            .Create();

            RoleRequest roleRequest2 = _fixture.Build<RoleRequest>()
            .With(x => x.Name, UserRoles.Employee.ToString())
            .Create();

            RoleResponse roleResponse1 = await _roleService.CreateRole(roleRequest1);   
            RoleResponse roleResponse2 = await _roleService.CreateRole(roleRequest2);

            UserRequest userRequest1 = _fixture.Build<UserRequest>()
            .With(x => x.Email, "s@gmail.com")
            .With(x => x.RoleId, roleResponse1.Id)
            .Create();

            UserRequest userRequest2 = _fixture.Build<UserRequest>()
            .With(x => x.Email, "k@gmail.com")
            .With(x => x.RoleId, roleResponse2.Id)
            .Create();

            List<UserResponse> userResponses_Expected = new List<UserResponse>()
            {
                await _userService.RegisterManager(userRequest1),
                await _userService.RegisterEmployee(userRequest2)
            };

            // Act
            List<UserResponse>? userResponses_FromTest = await _userService.GetAllEmployees();

            // Assert
            Assert.NotNull(userResponses_FromTest);
            Assert.Equal(userResponses_Expected, userResponses_FromTest);

        }
        #endregion

        #region Get Employee ById

        /// <summary>
        /// Get Employee by Id with Null Id
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetEmployeeById_EmptyId_ToBeArgumentNullException()
        {
            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _userService.GetEmployeeById(Guid.Empty);
            });
        }

        /// <summary>
        /// Get Employee by Id with Employee Not found
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetEmployeeById_EmployeeNotFound_ToBeNull()
        {
            // Arrange
            Guid Id = Guid.NewGuid();

            // Act
            UserResponse? userResponse = await _userService.GetEmployeeById(Id);

            // Assert
            Assert.Null(userResponse);
        }

        /// <summary>
        /// Get Employee By Id Employee Found
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetEmployeeById_EmployeeFound_ToBeSuccess()
        {
            // Arrange
            RoleRequest roleRequest = _fixture.Build<RoleRequest>()
            .With(x => x.Name, UserRoles.Employee.ToString())
            .Create();

            RoleResponse roleResponse = await _roleService.CreateRole(roleRequest);

            UserRequest userRequest = _fixture.Build<UserRequest>()
            .With(x => x.Email, "sky@gmail.com")
            .With(x => x.RoleId, roleResponse.Id)
            .Create();

            UserResponse userResponse_Expected = await _userService.RegisterEmployee(userRequest);

            // Act
            UserResponse? userResponse_FromTest = await _userService.GetEmployeeById(userResponse_Expected.Id);

            // Assert
            Assert.NotNull(userResponse_FromTest);
            Assert.Equal(userResponse_Expected, userResponse_FromTest);

        }

        #endregion

        #region Get Employee By Username

        /// <summary>
        /// Get Employee details by his username when username is null
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetEmployeeByUserName_NullName_ToBeArgumentException()
        {
            // Arrange
            string? username = null;

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _userService.GetEmployeeByUserName(username);
            });
        }

        /// <summary>
        /// Get Employee details by his username when user with a username is not found
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetEmployeeByUserName_NotFound_ToBeNull()
        {
            // Arrange 
            string? username = "sky";

            // Act
            UserResponse? userResponse = await _userService.GetEmployeeByUserName(username);

            // Assert
            Assert.Null(userResponse);
        }

        /// <summary>
        /// Get Employee details by his username when user with a username is found
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetEmployeeByUserName_UserFound_ToBeSuccess()
        {
            // Arrange
            RoleRequest roleRequest = _fixture.Build<RoleRequest>()
            .With(x => x.Name, UserRoles.Employee.ToString())
            .Create();

            RoleResponse roleResponse = await _roleService.CreateRole(roleRequest);

            UserRequest userRequest = _fixture.Build<UserRequest>()
            .With(x => x.Email, "sk@gmail.com")
            .With(x => x.RoleId, roleResponse.Id)
            .Create();

            UserResponse userResponse_Expected = await _userService.RegisterEmployee(userRequest);

            // Act
            UserResponse? userResponse_FromTest = await _userService.GetEmployeeByUserName(userRequest.Username);

            // Assert
            Assert.Equal(userResponse_Expected, userResponse_FromTest);

        }

        #endregion

        #region Update Employee


        
        #endregion

    }
}
