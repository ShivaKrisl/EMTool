using Service_Contracts;
using Services;
using AutoFixture;
using DTOs.TeamMembers;
using DTOs.Roles;
using EfCore;
using DTOs.Users;
using DTOs.Teams;
using System.Runtime.CompilerServices;

namespace Testing
{
    public class TeamMemberServiceTest
    {
        private readonly ITeamMemberService _teamMemberService;

        private readonly IUserService _userService;

        private readonly ITeamService _teamService;

        private readonly IRoleService _roleService;

        private readonly IFixture _fixture;

        public TeamMemberServiceTest()
        {
            _roleService = new RoleService();
            _userService = new UserService(_roleService);
            _teamService = new TeamService(_userService, _roleService);
            _teamMemberService = new TeamMemberService(_userService, _teamService, _roleService);
            _fixture = new Fixture();
        }

        #region Add Team Member

        /// <summary>
        /// Add a team member to Team  with a null request
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddTeamMember_NullRequest_ToBeArgumentNullException()
        {
            // Act + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _teamMemberService.AddTeamMember(null);
            });
        }

        /// <summary>
        /// Add a team member to Team  with an invalid request
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddTeamMember_InvalidRequest_ToBeArgumentException()
        {
            // Arrange
            TeamMemberRequest teamMemberRequest = _fixture.Create<TeamMemberRequest>();
            teamMemberRequest.TeamId = Guid.Empty;

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _teamMemberService.AddTeamMember(teamMemberRequest);
            });
        }

        /// <summary>
        /// Add Team member to a Team but not Team found
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddTeamMember_TeamDoesNotExist_ToBeArgumentException()
        {
            // Arrange
            TeamMemberRequest teamMemberRequest = _fixture.Build<TeamMemberRequest>()
            .Create();

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _teamMemberService.AddTeamMember(teamMemberRequest);
            });
        }

        /// <summary>
        /// Add Team member to a Team but no Manager found
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddTeamMember_ManagerNotFound_ToBeArgumentException()
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

            TeamMemberRequest teamMemberRequest = _fixture.Build<TeamMemberRequest>()
            .With(m => m.TeamId, teamResponse.Id)
            .Create();

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _teamMemberService.AddTeamMember(teamMemberRequest);
            });
        }

        /*
         * Todo:
         * 1. Check if manager is part of same team
         * 2. Check if Employee Exists
         * 3. Check if already added
         * 4. Check for Valid Request
         */

        /// <summary>
        /// Add Team member to a Team but Manager who is adding is not a part of same team
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddTeamMember_ManagerIsNotOfSameTeam_ToBeArgumentException()
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

            // Create another manager -  not part of this team

            UserRequest userRequestDummy = _fixture.Build<UserRequest>()
            .With(u => u.Email, "sk@gmail.com")
            .With(u => u.RoleId, roleResponse.Id)
            .Create();

            UserResponse userResponseDummy = await _userService.RegisterManager(userRequestDummy);

            TeamMemberRequest teamMemberRequest = _fixture.Build<TeamMemberRequest>()
            .With(m => m.TeamId, teamResponse.Id)
            .With(m => m.AddedByUserId, userResponseDummy.Id)
            .Create();

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _teamMemberService.AddTeamMember(teamMemberRequest);
            });
        }

        /// <summary>
        /// Add a team memeber but Employee is not found
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddTeamMember_EmployeeNotFound_ToBeArgumentException()
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

            TeamMemberRequest teamMemberRequest = _fixture.Build<TeamMemberRequest>()
            .With(m => m.TeamId, teamResponse.Id)
            .With(m => m.AddedByUserId, userResponse.Id)
            .Create();

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _teamMemberService.AddTeamMember(teamMemberRequest);
            });

        }

        /// <summary>
        /// Add a team memeber but Employee is already member of team
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddTeamMember_EmployeeAlreadyPartOfTeam_ToBeArgumentException()
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

            // Create 2 employees

            RoleRequest roleRequest1 = _fixture.Build<RoleRequest>()
            .With(t => t.Name, UserRoles.Employee.ToString())
            .Create();

            RoleResponse roleResponse1 = await _roleService.CreateRole(roleRequest1);

            UserRequest userRequest1 = _fixture.Build<UserRequest>()
            .With(u => u.Email, "skk@gmail.com")
            .With(u => u.RoleId, roleResponse1.Id)
            .Create();

            UserResponse userResponse1 = await _userService.RegisterEmployee(userRequest1);

            TeamMemberRequest teamMemberRequest = _fixture.Build<TeamMemberRequest>()
           .With(m => m.TeamId, teamResponse.Id)
           .With(m => m.AddedByUserId, userResponse.Id)
           .With(m => m.UserId, userResponse1.Id)
           .Create();

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _teamMemberService.AddTeamMember(teamMemberRequest);
                await _teamMemberService.AddTeamMember(teamMemberRequest);
            });

        }

        /// <summary>
        /// Add a Team Member
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddTeamMember_ValidRequest_ToBeSuccess()
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

            // Create 2 employees

            RoleRequest roleRequest1 = _fixture.Build<RoleRequest>()
            .With(t => t.Name, UserRoles.Employee.ToString())
            .Create();

            RoleResponse roleResponse1 = await _roleService.CreateRole(roleRequest1);

            UserRequest userRequest1 = _fixture.Build<UserRequest>()
            .With(u => u.Email, "skk@gmail.com")
            .With(u => u.RoleId, roleResponse1.Id)
            .Create();

            UserResponse userResponse1 = await _userService.RegisterEmployee(userRequest1);

            TeamMemberRequest teamMemberRequest = _fixture.Build<TeamMemberRequest>()
           .With(m => m.TeamId, teamResponse.Id)
           .With(m => m.AddedByUserId, userResponse.Id)
           .With(m => m.UserId, userResponse1.Id)
           .Create();

            // Act
            TeamMemberResponse teamMemberResponse = await _teamMemberService.AddTeamMember(teamMemberRequest);

            // Assert
            Assert.NotNull(teamMemberResponse);
            Assert.True(teamMemberResponse.Id != Guid.Empty);
        }


        #endregion

        #region Get All Team Members Of Team By Team Id

        /// <summary>
        /// Get all team members of team when Team Id is empty
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllTeamMembers_EmptyTeamId_ToBeArgumentException()
        {
            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _teamMemberService.GetAllTeamMembers(Guid.Empty);
            });
        }

        /// <summary>
        /// Get all team members of team when Team does not exist
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllTeamMembers_TeamDoesNotExist_ToBeEmpty()
        {
            // Act
            List<TeamMemberResponse> teamMemberResponses = await _teamMemberService.GetAllTeamMembers(Guid.NewGuid());

            // Assert
            Assert.Empty(teamMemberResponses);
        }

        /// <summary>
        /// Get all team members of team when Team exists but no members
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllTeamMembers_TeamExists_ToBeSuccess()
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

            TeamResponse teamResponse = await _teamService.CreateTeam(teamRequest);

            // Create 2 employees

            RoleRequest roleRequest1 = _fixture.Build<RoleRequest>()
            .With(t => t.Name, UserRoles.Employee.ToString())
            .Create();

            RoleResponse roleResponse1 = await _roleService.CreateRole(roleRequest1);

            UserRequest userRequest1 = _fixture.Build<UserRequest>()
            .With(u => u.Email, "skk@gmail.com")
            .With(u => u.RoleId, roleResponse1.Id)
            .Create();

            UserResponse userResponse1 = await _userService.RegisterEmployee(userRequest1);

            UserRequest userRequest2 = _fixture.Build<UserRequest>()
            .With(u => u.Email, "syk@gmail.com")
            .With(u => u.RoleId, roleResponse1.Id)
            .Create();

            UserResponse userResponse2 = await _userService.RegisterEmployee(userRequest2);

            TeamMemberRequest teamMemberRequest = _fixture.Build<TeamMemberRequest>()
           .With(m => m.TeamId, teamResponse.Id)
           .With(m => m.AddedByUserId, userResponse.Id)
           .With(m => m.UserId, userResponse1.Id)
           .Create();

            TeamMemberRequest teamMemberRequest1 = _fixture.Build<TeamMemberRequest>()
           .With(m => m.TeamId, teamResponse.Id)
           .With(m => m.AddedByUserId, userResponse.Id)
           .With(m => m.UserId, userResponse2.Id)
           .Create();


            TeamMemberResponse teamMemberResponse = await _teamMemberService.AddTeamMember(teamMemberRequest);

            TeamMemberResponse teamMemberResponse1 = await _teamMemberService.AddTeamMember(teamMemberRequest1);

            List<TeamMemberResponse> teamMemberResponses_Expected = new List<TeamMemberResponse>()
            {
                teamMemberResponse,
                teamMemberResponse1
            };

            // Act
            List<TeamMemberResponse> teamMemberResponses_FromTest = await _teamMemberService.GetAllTeamMembers(teamResponse.Id);

            // Assert
            Assert.NotNull(teamMemberResponses_FromTest);
            Assert.Equal(teamMemberResponses_Expected, teamMemberResponses_FromTest);
        }

        #endregion

        #region Get Team Member By User Id

        /// <summary>
        /// Get Team member by Employee Id when Employee Id is Null
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetTeamMemberByUserId_EmptyId_ToBeArgumentException()
        {
            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _teamMemberService.GetTeamMemberByUserId(Guid.Empty);
            });
        }

        /// <summary>
        /// Get Team member by Employee Id when Employee does not exist
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetTeamMemberByUserId_UserNotFound_ToBeNull()
        {
            // Act 
            TeamMemberResponse? teamMemberResponse = await _teamMemberService.GetTeamMemberByUserId(Guid.NewGuid());

            // Assert
            Assert.Null(teamMemberResponse);
        }

        /// <summary>
        /// Get Team member by Employee Id when Employee exists
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetTeamMemberByUserId_UserFound_ToBeSuccess()
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

            RoleRequest roleRequest1 = _fixture.Build<RoleRequest>()
            .With(t => t.Name, UserRoles.Employee.ToString())
            .Create();

            RoleResponse roleResponse1 = await _roleService.CreateRole(roleRequest1);

            UserRequest userRequest1 = _fixture.Build<UserRequest>()
            .With(u => u.Email, "skk@gmail.com")
            .With(u => u.RoleId, roleResponse1.Id)
            .Create();

            UserResponse userResponse1 = await _userService.RegisterEmployee(userRequest1);

            TeamMemberRequest teamMemberRequest = _fixture.Build<TeamMemberRequest>()
           .With(m => m.TeamId, teamResponse.Id)
           .With(m => m.AddedByUserId, userResponse.Id)
           .With(m => m.UserId, userResponse1.Id)
           .Create();

            TeamMemberResponse teamMemberResponse_Expected = await _teamMemberService.AddTeamMember(teamMemberRequest);

            // Act

            TeamMemberResponse? teamMemberResponse_FromTest = await _teamMemberService.GetTeamMemberByUserId(userResponse1.Id);

            // Assert
            Assert.NotNull(teamMemberResponse_FromTest);
            Assert.Equal(teamMemberResponse_Expected, teamMemberResponse_FromTest);
        }

        #endregion

        #region Delete Team Member

        /// <summary>
        /// Delete Team Member when Team Member Id is Empty
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteTeamMember_EmptyId_ToBeArgumentException()
        {
            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _teamMemberService.DeleteTeamMember(Guid.Empty);
            });
        }

        /// <summary>
        /// Delete Team Member when Team Member does not exist
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteTeamMemeber_MemberNotFound_ToBeFalse()
        {
            // Act
            bool isDeleted = await _teamMemberService.DeleteTeamMember(Guid.NewGuid());

            // Assert
            Assert.False(isDeleted);
        }

        /// <summary>
        /// Delete Team Member when Team Member exists
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteTeamMember_MemberExists_ToBeSuccess()
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

            RoleRequest roleRequest1 = _fixture.Build<RoleRequest>()
            .With(t => t.Name, UserRoles.Employee.ToString())
            .Create();

            RoleResponse roleResponse1 = await _roleService.CreateRole(roleRequest1);

            UserRequest userRequest1 = _fixture.Build<UserRequest>()
            .With(u => u.Email, "skk@gmail.com")
            .With(u => u.RoleId, roleResponse1.Id)
            .Create();

            UserResponse userResponse1 = await _userService.RegisterEmployee(userRequest1);

            TeamMemberRequest teamMemberRequest = _fixture.Build<TeamMemberRequest>()
           .With(m => m.TeamId, teamResponse.Id)
           .With(m => m.AddedByUserId, userResponse.Id)
           .With(m => m.UserId, userResponse1.Id)
           .Create();

            TeamMemberResponse teamMemberResponse_Expected = await _teamMemberService.AddTeamMember(teamMemberRequest);

            // Act
            bool isDeleted = await _teamMemberService.DeleteTeamMember(teamMemberResponse_Expected.Id);

            // Assert
            Assert.True(isDeleted);
        }

        #endregion

    }
}
