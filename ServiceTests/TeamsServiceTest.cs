using AutoFixture;
using DTOs.Roles;
using DTOs.Teams;
using DTOs.Users;
using EfCore;
using Microsoft.Identity.Client;
using Service_Contracts;
using Services;

namespace Testing
{
    public class TeamsServiceTest
    {
        private readonly ITeamService _teamService;

        private readonly IFixture _fixture;

        private readonly IRoleService _roleService;

        private readonly IUserService _userService;

        public TeamsServiceTest()
        {
            _roleService = new RoleService();
            _userService = new UserService(_roleService);
            _teamService = new TeamService(_userService, _roleService);
            _fixture = new Fixture();
        }


        #region Create Team

        /// <summary>
        /// Create a new team with Null request
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateTeam_NullRequest_ArgumentNullException()
        {
            // Arrange
            TeamRequest? teamRequest = null;

            // Act + Assert

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _teamService.CreateTeam(teamRequest);
            });
        }

        /// <summary>
        /// Create a new team with team name null
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateTeam_TeamNameEmpty_ToBeArgumentException()
        {
            // Arrange
            TeamRequest teamRequest = _fixture.Build<TeamRequest>()
            .With(x => x.TeamName, null as string)
            .Create();

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _teamService.CreateTeam(teamRequest);
            });

        }

        /// <summary>
        /// Create a Team without Manager
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateTeam_ManagerNotFound_ToBeArgumentException()
        {
            // Arrange
            TeamRequest teamRequest = _fixture.Build<TeamRequest>()
            .Create();

            // Act
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _teamService.CreateTeam(teamRequest);
            });
        }

        /// <summary>
        /// Create a new Team with different role other than manager
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateTeam_InvalidRole_ToBeArgumentException()
        {
            // Arrange

            RoleRequest roleRequest = _fixture.Build<RoleRequest>()
            .With(r => r.Name, UserRoles.Employee.ToString())
            .Create();

            RoleResponse roleResponse = await _roleService.CreateRole(roleRequest);

            UserRequest userRequest = _fixture.Build<UserRequest>()
            .With(u => u.Email, "s@gmail.com")
            .With(u => u.RoleId, roleResponse.Id)
            .Create();

            UserResponse userResponse = await _userService.RegisterEmployee(userRequest);

            TeamRequest teamRequest = _fixture.Build<TeamRequest>()
            .With(t => t.ManagerId, userResponse.Id)
            .Create();

            // Act + Assert

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _teamService.CreateTeam(teamRequest);
            });
        }


        /// <summary>
        /// Create a new Team with Duplicate Team Name
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateTeam_DuplicateTeamName_ToBeArgumentException()
        {
            // Arrange
            RoleRequest roleRequest = _fixture.Build<RoleRequest>()
            .With(r => r.Name, UserRoles.Manager.ToString())
            .Create();

            RoleResponse roleResponse = await _roleService.CreateRole(roleRequest);

            UserRequest userRequest = _fixture.Build<UserRequest>()
           .With(u => u.Email, "s@gmail.com")
           .With(u => u.RoleId, roleResponse.Id)
           .Create();

            UserResponse userResponse = await _userService.RegisterManager(userRequest);

            TeamRequest teamRequest = _fixture.Build<TeamRequest>()
            .With(t => t.TeamName, "sky")
            .With(t => t.ManagerId, userResponse.Id)
            .Create();

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _teamService.CreateTeam(teamRequest);
                await _teamService.CreateTeam(teamRequest);
            });

        }

        /// <summary>
        /// Create a new Team with a Valid Request
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateTeam_ValidRequest_ToBeSuccess()
        {
            // Arrange

            RoleRequest roleRequest = _fixture.Build<RoleRequest>()
            .With(r => r.Name, UserRoles.Manager.ToString())
            .Create();

            RoleResponse roleResponse = await _roleService.CreateRole(roleRequest);

            UserRequest userRequest = _fixture.Build<UserRequest>()
            .With(u => u.Email, "s@gmail.com")
            .With(u => u.RoleId, roleResponse.Id)
            .Create();

            UserResponse userResponse = await _userService.RegisterManager(userRequest);

            TeamRequest teamRequest = _fixture.Build<TeamRequest>()
            .With(t => t.ManagerId, userResponse.Id)
            .Create();

            // Act
            TeamResponse teamResponse = await _teamService.CreateTeam(teamRequest);

            // Assert
            Assert.NotNull(teamResponse);
            Assert.True(teamResponse.Id != Guid.Empty);
        }

        #endregion

        #region Get Team By Team Name

        /// <summary>
        /// Get Team Details with Team Name as Null
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetTeamDetails_TeamNToBeArgumentException()
        {
            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _teamService.GetTeamByTeamName(null);
            });
        }

        /// <summary>
        /// Get Team Details with Team Name not found
        /// </summary>
        /// <returns></returns>
        [Fact]  
        public async Task GetTeamDetails_TeamNotFound_ToBeNull()
        {
            // Act 
            TeamResponse? teamResponse = await _teamService.GetTeamByTeamName("Home UI");

            // Assert
            Assert.Null(teamResponse);
        }

        /// <summary>
        /// Get Team Details with Team Name found
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetTeamDetails_TeamFound_ToBeSuccess()
        {
            RoleRequest roleRequest = _fixture.Build<RoleRequest>()
            .With(r => r.Name, UserRoles.Manager.ToString())
            .Create();

            RoleResponse roleResponse = await _roleService.CreateRole(roleRequest);

            UserRequest userRequest = _fixture.Build<UserRequest>()
            .With(u => u.Email, "s@gmail.com")
            .With(u => u.RoleId, roleResponse.Id)
            .Create();

            UserResponse userResponse = await _userService.RegisterManager(userRequest);

            TeamRequest teamRequest = _fixture.Build<TeamRequest>()
            .With(t => t.ManagerId, userResponse.Id)
            .Create();

            TeamResponse teamResponse_Expected = await _teamService.CreateTeam(teamRequest);

            // Act
            TeamResponse? teamResponse_FromTest = await _teamService.GetTeamByTeamName(teamRequest.TeamName);

            // Assert
            Assert.NotNull(teamResponse_FromTest);
            Assert.Equal(teamResponse_Expected, teamResponse_FromTest);

        }
        #endregion

        #region Get Team By Team Id

        /// <summary>
        /// Get Team Details with Team Id as Empty
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetTeamById_EmptyId_ToBeArgumentException()
        {
            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _teamService.GetTeamById(Guid.Empty);
            });
        }

        /// <summary>
        /// Get Team Details with Team Id not found
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetTeamById_TeamNotFound_ToBeNull()
        {
            // Act
            TeamResponse? teamResponse = await _teamService.GetTeamById(Guid.NewGuid());

            // Assert
            Assert.Null(teamResponse);
        }

        /// <summary>
        /// Get Team Details with Team Id found
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetTeamById_TeamFound_ToBeSuccess()
        {
            // Arrange
            RoleRequest roleRequest = _fixture.Build<RoleRequest>()
            .With(r => r.Name, UserRoles.Manager.ToString())
            .Create();

            RoleResponse roleResponse = await _roleService.CreateRole(roleRequest);

            UserRequest userRequest = _fixture.Build<UserRequest>()
            .With(u => u.Email, "s@gmail.com")
            .With(u => u.RoleId, roleResponse.Id)
            .Create();

            UserResponse userResponse = await _userService.RegisterManager(userRequest);

            TeamRequest teamRequest = _fixture.Build<TeamRequest>()
            .With(t => t.ManagerId, userResponse.Id)
            .Create();

            TeamResponse teamResponse_Expected = await _teamService.CreateTeam(teamRequest);

            // Act
            TeamResponse? teamResponse_FromTest = await _teamService.GetTeamById(teamResponse_Expected.Id);

            // Assert
            Assert.NotNull(teamResponse_FromTest);
            Assert.Equal(teamResponse_Expected, teamResponse_FromTest);
        }

        #endregion

        #region Delete Team By Id

        /// <summary>
        /// Delete Team with Empty Id
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteTeam_EmptyId_ToBeArgumentException()
        {
            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _teamService.DeleteTeam(Guid.Empty);
            });
        }

        /// <summary>
        /// Delete Team with Team Id not found
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteTeam_TeamNotFound_ToBeFalse()
        {
            // Act
            bool isDeleted = await _teamService.DeleteTeam(Guid.NewGuid());

            // Assert
            Assert.False(isDeleted);
        }

        /// <summary>
        /// Delete Team with Team Id found
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteTeam_TeamFound_ToBeSuccess()
        {
            // Arrange

            RoleRequest roleRequest = _fixture.Build<RoleRequest>()
           .With(r => r.Name, UserRoles.Manager.ToString())
           .Create();

            RoleResponse roleResponse = await _roleService.CreateRole(roleRequest);

            UserRequest userRequest = _fixture.Build<UserRequest>()
            .With(u => u.Email, "s@gmail.com")
            .With(u => u.RoleId, roleResponse.Id)
            .Create();

            UserResponse userResponse = await _userService.RegisterManager(userRequest);

            TeamRequest teamRequest = _fixture.Build<TeamRequest>()
            .With(t => t.ManagerId, userResponse.Id)
            .Create();

            TeamResponse teamResponse_Expected = await _teamService.CreateTeam(teamRequest);

            // Act

            bool isDeleted = await _teamService.DeleteTeam(teamResponse_Expected.Id);

            // Assert
            Assert.True(isDeleted);
        }
        #endregion

        #region Update Team Details

        /// <summary>
        /// Update Team Details With Empty Team Id
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateTeam_EmptyId_ToBeArgumentException()
        {
            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _teamService.UpdateTeamDetails(Guid.Empty, new TeamRequest());
            });
        }

        /// <summary>
        /// Update Team details with null Request
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateTeamDetails_NullRequest_ToBeArgumentNullException()
        {
            // Arrange
            Guid teamId = Guid.NewGuid();
            TeamRequest? teamRequest = null;

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _teamService.UpdateTeamDetails(teamId, teamRequest);
            });
        }

        /// <summary>
        /// Update Team Details with Invalid Details
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateTeamDetails_InvalidRequest_ToBeArgumentException()
        {
            // Arrange
            Guid teamId = Guid.NewGuid();
            TeamRequest teamRequest = _fixture.Build<TeamRequest>()
            .With(t => t.TeamName, null as string)
            .Create();

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _teamService.UpdateTeamDetails(teamId, teamRequest);
            });
        }

        /// <summary>
        /// Update TeamDetails With Team not found
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateTeamDetails_TeamNotFound_ToBeArgumentException()
        {
            // Arrange
            Guid teamId = Guid.NewGuid();
            TeamRequest teamRequest = _fixture.Build<TeamRequest>()
            .Create();

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _teamService.UpdateTeamDetails(teamId, teamRequest);
            });

        }

        /// <summary>
        /// Update Team Details with Manager not found
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateEmployeeDetails_ManagerNotFound_ToBeArgumentException()
        {
            // Arrange
            RoleRequest roleRequest = _fixture.Build<RoleRequest>()
            .With(r => r.Name, UserRoles.Manager.ToString())
            .Create();

            RoleResponse roleResponse = await _roleService.CreateRole(roleRequest);

            UserRequest userRequest = _fixture.Build<UserRequest>()
            .With(u => u.Email, "s@gmail.com")
            .With(u => u.RoleId, roleResponse.Id)
            .Create();

            UserResponse userResponse = await _userService.RegisterManager(userRequest);

            TeamRequest teamRequest = _fixture.Build<TeamRequest>()
            .With(t => t.ManagerId, userResponse.Id)
            .Create();

            TeamResponse teamResponse = await _teamService.CreateTeam(teamRequest);
            teamRequest.ManagerId = Guid.NewGuid();
            teamRequest.TeamName = "sky";

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _teamService.UpdateTeamDetails(teamResponse.Id, teamRequest);
            });
        }

        /// <summary>
        /// Update Team Details with Duplicate Team Names
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateTeamDetails_DuplicateTeamNames_ToBeArgumentException()
        {
            // Arrange
            RoleRequest roleRequest = _fixture.Build<RoleRequest>()
            .With(t => t.Name, UserRoles.Manager.ToString())
            .Create();

            RoleResponse roleResponse = await _roleService.CreateRole(roleRequest);

            UserRequest userRequest = _fixture.Build<UserRequest>()
            .With(u => u.Email, "s@gmail.com")
            .With(u => u.RoleId, roleResponse.Id)
            .Create();

            UserResponse userResponse = await _userService.RegisterManager(userRequest);

            TeamRequest teamRequest1 = _fixture.Build<TeamRequest>()
            .With(t => t.ManagerId, userResponse.Id)
            .With(t => t.TeamName, "UI")
            .Create();

            TeamRequest teamRequest2 = _fixture.Build<TeamRequest>()
            .With(t => t.ManagerId, userResponse.Id)
            .With(t => t.TeamName, "UX")
            .Create();

            TeamResponse teamResponse1 = await _teamService.CreateTeam(teamRequest1);
            TeamResponse teamResponse2 = await _teamService.CreateTeam(teamRequest2);

            teamRequest2.TeamName = "UI";

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _teamService.UpdateTeamDetails(teamResponse2.Id, teamRequest2);
            });

        }

        /// <summary>
        /// Update Team Details with Valid Request
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateTeamDetails_ValidRequest_ToBeSuccess()
        {
            // Arrange

            RoleRequest roleRequest = _fixture.Build<RoleRequest>()
            .With(r => r.Name, UserRoles.Manager.ToString())
            .Create();

            RoleResponse roleResponse = await _roleService.CreateRole(roleRequest);

            UserRequest userRequest = _fixture.Build<UserRequest>()
                .With(u => u.Email, "s@gmail.com")
                .With(u => u.RoleId, roleResponse.Id)
                .Create();

            UserResponse userResponse = await _userService.RegisterManager(userRequest);

            TeamRequest teamRequest = _fixture.Build<TeamRequest>()
                .With(t => t.ManagerId, userResponse.Id)
                .Create();

            TeamResponse teamResponse = await _teamService.CreateTeam(teamRequest);

            teamRequest.TeamName = "UI";

            // Act

            TeamResponse teamResponse_Updated = await _teamService.UpdateTeamDetails(teamResponse.Id, teamRequest);

            // Assert
            Assert.NotNull(teamResponse_Updated);
            Assert.Equal(teamRequest.TeamName, teamResponse_Updated.TeamName);
        }

        #endregion

        #region Get All Teams Of a Manager

        /// <summary>
        /// Get All Teams of Manager with Empty Id
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllTeamsOfManager_EmptyId_ToBeArgumentException()
        {
            // Act + Assert

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _teamService.GetAllTeamsOfManager(Guid.Empty);
            });

        }

        /// <summary>
        /// Get All Teams of Manager with Manager not found
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllTeamsOfManager_ManagerNotFound_ToBeNull()
        {
            // Act
            List<TeamResponse> teamResponses = await _teamService.GetAllTeamsOfManager(Guid.NewGuid());
            // Assert
            Assert.Empty(teamResponses);
        }

        /// <summary>
        /// Get All Teams of Manager with Teams found
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllTeamsOfManager_TeamsFound_ToBeSuccess()
        {
            // Arrange

            RoleRequest roleRequest = _fixture.Build<RoleRequest>()
            .With(r => r.Name, UserRoles.Manager.ToString())
            .Create();

            RoleResponse roleResponse = await _roleService.CreateRole(roleRequest);

            UserRequest userRequest = _fixture.Build<UserRequest>()
                .With(u => u.Email, "s@gmail.com")
                .With(u => u.RoleId, roleResponse.Id)
                .Create();

            UserResponse userResponse = await _userService.RegisterManager(userRequest);

            TeamRequest teamRequest = _fixture.Build<TeamRequest>()
                .With(t => t.ManagerId, userResponse.Id)
                .Create();

            TeamResponse teamResponse = await _teamService.CreateTeam(teamRequest);

            TeamRequest teamRequest2 = _fixture.Build<TeamRequest>()
                .With(t => t.ManagerId, userResponse.Id)
                .Create();

            TeamResponse teamResponse2 = await _teamService.CreateTeam(teamRequest2);

            List<TeamResponse> teamResponses_Expected = new List<TeamResponse> { teamResponse, teamResponse2 };

            // Act  
            List<TeamResponse> teamResponses_FromTest = await _teamService.GetAllTeamsOfManager(userResponse.Id);

            // Assert
            Assert.NotEmpty(teamResponses_FromTest);
            Assert.Equal(teamResponses_Expected, teamResponses_FromTest);

        }
        #endregion
    }
}
