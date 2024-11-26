using Moq;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Notifications.Interfaces;
using TaskManagerAPI.Repositories.Interfaces;
using TaskManagerAPI.Services;
using System.Linq.Expressions;
using TaskManagerAPI.Notifications;

namespace TaskManagerAPI.Tests.Services
{
    public class ReportServiceTests
    {
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<INotifier> _notifierMock;
        private readonly ReportService _reportService;

        public ReportServiceTests()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _notifierMock = new Mock<INotifier>();
            _reportService = new ReportService(_taskRepositoryMock.Object, _notifierMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAverageCompletedTasksByUserAsync_ShouldReturnNull_WhenUserIdIsInvalid()
        {
            // Arrange
            var invalidUserId = "invalid-id";

            // Act
            var result = await _reportService.GetAverageCompletedTasksByUserAsync(invalidUserId);

            // Assert
            Assert.Null(result);
            _notifierMock.Verify(n => n.Handle(It.Is<Notification>(
                notification => notification.Message == "O ID fornecido não é válido."
            )), Times.Once);
        }

        [Fact]
        public async Task GetAverageCompletedTasksByUserAsync_ShouldThrowUnauthorizedAccessException_WhenUserIsNotManager()
        {
            // Arrange
            var userId = "64b9f0e3d6a6341c8b3d9c02";
            var user = new User { UserId = userId, Name = "Bob", Category = "analista" };

            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync(user);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _reportService.GetAverageCompletedTasksByUserAsync(userId));

            _notifierMock.Verify(n => n.Handle(It.IsAny<Notification>()), Times.Never);
        }

        [Fact]
        public async Task GetAverageCompletedTasksByUserAsync_ShouldReturnGroupedTasks_WhenUserIsManager()
        {
            // Arrange
            var userId = "64b9f0e3d6a6341c8b3d9c01";
            var user = new User { UserId = userId, Name = "Alice", Category = "gerente" };

            var completedTasks = new List<TaskModel>
            {
                new TaskModel { Id = "1", ModifiedByUserId = "user1", Status = Enums.Enum.TaskStatus.Completed, DueDate = DateTime.UtcNow },
                new TaskModel { Id = "2", ModifiedByUserId = "user1", Status = Enums.Enum.TaskStatus.Completed, DueDate = DateTime.UtcNow },
                new TaskModel { Id = "3", ModifiedByUserId = "user2", Status = Enums.Enum.TaskStatus.Completed, DueDate = DateTime.UtcNow }
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync(user);

            _taskRepositoryMock.Setup(repo => repo.GetTasksByFilterAsync(It.IsAny<Expression<Func<TaskModel, bool>>>()))
                .ReturnsAsync(completedTasks);

            // Act
            var result = await _reportService.GetAverageCompletedTasksByUserAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count); // Dois usuários com tarefas
            Assert.Equal(2, result["user1"]);
            Assert.Equal(1, result["user2"]);

            _notifierMock.Verify(n => n.Handle(It.IsAny<Notification>()), Times.Never);
        }

        [Fact]
        public async Task GetAverageCompletedTasksByUserAsync_ShouldReturnEmpty_WhenNoTasksAreCompleted()
        {
            // Arrange
            var userId = "64b9f0e3d6a6341c8b3d9c01";
            var user = new User { UserId = userId, Name = "Alice", Category = "gerente" };

            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync(user);

            _taskRepositoryMock.Setup(repo => repo.GetTasksByFilterAsync(It.IsAny<Expression<Func<TaskModel, bool>>>()))
                .ReturnsAsync(new List<TaskModel>());

            // Act
            var result = await _reportService.GetAverageCompletedTasksByUserAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _notifierMock.Verify(n => n.Handle(It.IsAny<Notification>()), Times.Never);
        }
    }
}
