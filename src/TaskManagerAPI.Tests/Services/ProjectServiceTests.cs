using Moq;
using TaskManagerAPI.Entities;
using TaskManagerAPI.HttpObjects.Projects;
using TaskManagerAPI.Notifications.Interfaces;
using TaskManagerAPI.Repositories.Interfaces;
using TaskManagerAPI.Services.Interfaces;
using TaskManagerAPI.Services;
using TaskManagerAPI.Notifications;

namespace TaskManagerAPI.Tests.Services
{
    public class ProjectServiceTests
    {
        private readonly Mock<IProjectRepository> _repositoryMock;
        private readonly Mock<ITaskService> _taskServiceMock;
        private readonly Mock<INotifier> _notifierMock;
        private readonly ProjectService _projectService;

        public ProjectServiceTests()
        {
            _repositoryMock = new Mock<IProjectRepository>();
            _taskServiceMock = new Mock<ITaskService>();
            _notifierMock = new Mock<INotifier>();
            _projectService = new ProjectService(
                _repositoryMock.Object,
                _notifierMock.Object,
                _taskServiceMock.Object
            );
        }

        [Fact]
        public async Task GetProjectsByUserIdAsync_ShouldReturnNull_WhenUserIdIsInvalid()
        {
            // Arrange
            var invalidUserId = "64b9f0e3d6a6341c8b3d9c0";

            // Act
            var result = await _projectService.GetProjectsByUserIdAsync(invalidUserId);

            // Assert
            Assert.Null(result);
            _notifierMock.Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
        }

        [Fact]
        public async Task GetProjectsByUserIdAsync_ShouldReturnProjects_WhenUserIdIsValid()
        {
            // Arrange
            var userId = "64b9f0e3d6a6341c8b3d9c01";
            var projects = new List<Project>
            {
                new Project { Id = "1", Name = "Project 1", UserId = userId },
                new Project { Id = "2", Name = "Project 2", UserId = userId }
            };

            _repositoryMock.Setup(repo => repo.GetProjectsByUserIdAsync(userId))
                .ReturnsAsync(projects);

            // Act
            var result = await _projectService.GetProjectsByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Project 1", result[0].Name);
        }

        [Fact]
        public async Task AddProjectAsync_ShouldReturnDefault_WhenUserIdIsInvalid()
        {
            // Arrange
            var projectCreateRequest = new ProjectCreateRequest
            {
                Name = "New Project",
                UserId = "64b9f0e3d6a6341c8b3d9c0"
            };

            // Act
            var result = await _projectService.AddProjectAsync(projectCreateRequest);

            // Assert
            Assert.Null(result);
            _notifierMock.Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
        }

        [Fact]
        public async Task AddProjectAsync_ShouldAddProject_WhenValidRequest()
        {
            // Arrange
            var projectCreateRequest = new ProjectCreateRequest
            {
                Name = "New Project",
                UserId = "64b9f0e3d6a6341c8b3d9c01"
            };

            var createdProject = new Project
            {
                Id = "123",
                Name = "New Project",
                UserId = "64b9f0e3d6a6341c8b3d9c01"
            };

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Project>())).Returns(Task.CompletedTask);
            _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(createdProject);

            // Act
            var result = await _projectService.AddProjectAsync(projectCreateRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Project", result.Name);
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Project>()), Times.Once);
        }

        [Fact]
        public async Task DeleteProjectAsync_ShouldReturnFalse_WhenProjectDoesNotExist()
        {
            // Arrange
            var projectId = "64b9f0e3d6a6341c8b3d9c01";
            _repositoryMock.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync((Project)null);

            // Act
            var result = await _projectService.DeleteProjectAsync(projectId);

            // Assert
            Assert.False(result);
            _notifierMock.Verify(n => n.Handle(It.Is<Notification>(
                notification => notification.Message == "O projeto não foi encontrado."
            )), Times.Once);
        }

        [Fact]
        public async Task DeleteProjectAsync_ShouldReturnFalse_WhenProjectHasTasks()
        {
            // Arrange
            var projectId = "64b9f0e3d6a6341c8b3d9c01";
            var project = new Project
            {
                Id = projectId,
                Name = "Test Project",
                Tasks = new List<TaskModel>
                {
                    new TaskModel { Id = "1", Title = "Task 1" }
                }
            };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync(project);

            // Act
            var result = await _projectService.DeleteProjectAsync(projectId);

            // Assert
            Assert.False(result);
            _notifierMock.Verify(n => n.Handle(It.Is<Notification>(
                notification => notification.Message == "Não é possível excluir o projeto pois existem tarefas associadas a ele."
            )), Times.Once);
        }

        [Fact]
        public async Task DeleteProjectAsync_ShouldDeleteProject_WhenValid()
        {
            // Arrange
            var projectId = "64b9f0e3d6a6341c8b3d9c01";
            var project = new Project
            {
                Id = projectId,
                Name = "Test Project",
                Tasks = new List<TaskModel>() // No tasks
            };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync(project);
            _repositoryMock.Setup(repo => repo.DeleteAsync(projectId)).Returns(Task.CompletedTask);

            // Act
            var result = await _projectService.DeleteProjectAsync(projectId);

            // Assert
            Assert.True(result);
            _repositoryMock.Verify(repo => repo.DeleteAsync(projectId), Times.Once);
        }

        [Fact]
        public async Task UpdateProjectAsync_ShouldReturnFalse_WhenTaskCountExceedsLimit()
        {
            // Arrange
            var projectId = "64b9f0e3d6a6341c8b3d9c01";
            var updatedProject = new Project
            {
                Id = projectId,
                Tasks = Enumerable.Repeat(new TaskModel { Id = "task" }, 21).ToList() // Exceeds limit
            };

            // Act
            var result = await _projectService.UpdateProjectAsync(projectId, updatedProject);

            // Assert
            Assert.False(result);
            _notifierMock.Verify(n => n.Handle(It.Is<Notification>(
                notification => notification.Message == "A quantidade máxima de tarefas por projeto é 20"
            )), Times.Once);
        }
    }
}
