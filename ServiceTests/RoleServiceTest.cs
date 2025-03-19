using DTOs.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service_Contracts;
using Services;
using AutoFixture;
using Microsoft.Identity.Client;
using EfCore;

namespace Testing
{
    public class RoleServiceTest
    {

        private readonly IRoleService _roleService;
        private readonly IFixture _fixture;

        public RoleServiceTest() {
            _roleService = new RoleService();
            _fixture = new Fixture();
        }

        #region AddRole

        /// <summary>
        /// Create a new Role with null request
        /// </summary>
        /// <returns></returns>

        [Fact]
        public async Task CreateRole_NullRequest_ToBeArgumentNullException()
        {
            // Arrange
            RoleRequest? roleRequest = null;

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _roleService.CreateRole(roleRequest);
            });

        }

        /// <summary>
        /// Create a new Role with invalid Role name
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateRole_InvalidRole_ToBeArgumentException()
        {
            // Arrange
            RoleRequest roleRequest = _fixture.Create<RoleRequest>();

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _roleService.CreateRole(roleRequest);
            });
        }

        /// <summary>
        /// Create a new Role with existing Role name
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateRole_RoleExists_ToBeArgumentException()
        {
            // Arrange
            RoleRequest roleRequest1 = _fixture.Create<RoleRequest>();
            roleRequest1.Name = UserRoles.Employee.ToString();

            RoleRequest roleRequest2 = _fixture.Create<RoleRequest>();
            roleRequest2.Name = UserRoles.Employee.ToString();

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _roleService.CreateRole(roleRequest1);
                await _roleService.CreateRole(roleRequest2);
            });

        }

        /// <summary>
        /// Create a new Role with valid request
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateRole_ValidRequest_ToBeSuccess()
        {
            // Arrange
            RoleRequest roleRequest = _fixture.Create<RoleRequest>();
            roleRequest.Name = UserRoles.Employee.ToString();

            // Act
            RoleResponse roleResponse = await _roleService.CreateRole(roleRequest);

            // Assert
            Assert.NotNull(roleResponse);
            Assert.True(roleResponse.Id!=Guid.Empty);

        }
        #endregion

        #region GetRoleById

        /// <summary>
        /// Get a Role by empty Id
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetRoleById_EmptyId_ToBeArgumentNullException()
        {
            // Arrange
            Guid roleId = Guid.Empty;

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _roleService.GetRoleById(roleId);
            });
        }

        /// <summary>
        /// Get a Role by Id that does not exist
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetRoleById_RoleNotFound_ToBeNull()
        {
            // Arrange
            Guid roleId = Guid.NewGuid();

            // Act
            RoleResponse? roleResponse = await _roleService.GetRoleById(roleId);

            // Assert
            Assert.Null(roleResponse);
        }

        /// <summary>
        /// Get a Role by Id that exists
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetRoleById_RoleFound_ToBeSuccess()
        {
            // Arrange
            RoleRequest roleRequest = _fixture.Build<RoleRequest>()
            .With(x => x.Name, UserRoles.Employee.ToString())
            .Create();
            RoleResponse roleResponse_Expected = await _roleService.CreateRole(roleRequest);

            // Act
            RoleResponse? roleResponse_FromTest = await _roleService.GetRoleById(roleResponse_Expected.Id);

            // Assert
            Assert.NotNull(roleResponse_FromTest);
            Assert.Equal(roleResponse_Expected.Id, roleResponse_FromTest.Id);

        }
        #endregion

        #region GetRoleByName

        /// <summary>
        /// Get a Role by empty Name
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetRoleByName_EmptyName_ToBeArgumentNullException()
        {
            // Arrange
            string roleName = string.Empty;
            // Act + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _roleService.GetRoleByName(roleName);
            });
        }

        /// <summary>
        /// Get a Role by Name that does not exist
        /// </summary>
        /// <returns></returns>

        [Fact]
        public async Task GetRoleByName_RoleNotFound_ToBeNull()
        {
            // Arrange
            string roleName = _fixture.Create<string>();

            // Act
            RoleResponse? roleResponse = await _roleService.GetRoleByName(roleName);

            // Assert
            Assert.Null(roleResponse);
        }

        /// <summary>
        /// Get a Role By Name 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetRoleByName_RoleFound_ToBeSuccess()
        {
            // Arrange
            RoleRequest roleRequest = _fixture.Build<RoleRequest>()
            .With(x => x.Name, UserRoles.Employee.ToString())
            .Create();

            RoleResponse roleResponse_Expected = await _roleService.CreateRole(roleRequest);

            // Act
            RoleResponse? roleResponse_FromTest = await _roleService.GetRoleByName(roleResponse_Expected.Name);

            // Assert
            Assert.NotNull(roleResponse_FromTest);
            Assert.Equal(roleResponse_Expected, roleResponse_FromTest);
        }

        #endregion
    }
}
