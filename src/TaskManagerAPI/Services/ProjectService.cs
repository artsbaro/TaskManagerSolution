using MongoDB.Bson;
using TaskManagerAPI.Entities;
using TaskManagerAPI.HttpObjects.Projects;
using TaskManagerAPI.Notifications.Interfaces;
using TaskManagerAPI.Repositories.Interfaces;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Services
{
    public class ProjectService : BaseService, IProjectService
    {
        private readonly ITaskService _taskService;
        private readonly IProjectRepository _repository;

        public ProjectService(IProjectRepository repository,
            INotifier notifier,
            ITaskService taskService) : base(notifier)
        {
            _repository = repository;
            _taskService = taskService;
        }

        public async Task<List<ProjectResponse>> GetProjectsByUserIdAsync(string userId)
        {
            if (!ValidateId(userId))
                return null;

            var projects =  await _repository.GetProjectsByUserIdAsync(userId);
            return projects.Select(MapToResponse).ToList();
        }

        public async Task<ProjectResponse> GetProjectByIdAsync(string id)
        {
            if (!ValidateId(id))
                return null;

            var project =  await _repository.GetByIdAsync(id);
            return MapToResponse(project);
        }

        public async Task<ProjectResponse> AddProjectAsync(ProjectCreateRequest projectCreateRequest)
        {

            if (!ValidateId(projectCreateRequest.UserId))
                return default;

            var project = new Project
            {
                Name = projectCreateRequest.Name,
                UserId = projectCreateRequest.UserId,
                Id = ObjectId.GenerateNewId().ToString()
            };

            await _repository.AddAsync(project);

            var projectCreated = await _repository.GetByIdAsync(project.Id);

            return MapToResponse(projectCreated);
        }

        private ProjectResponse MapToResponse(Project projectCreated)
        {
            return new ProjectResponse
            {
                Id = projectCreated.Id,
                Name = projectCreated.Name,
                UserId = projectCreated.UserId,
                Tasks = _taskService.GetTasksByProjectIdAsync(projectCreated.Id).GetAwaiter().GetResult()
            };
        }

        public async Task<bool> UpdateProjectAsync(string id, Project updatedProject)
        {
            if (!ValidateId(id))
                return false;

            if (!ValidateCountTasks(updatedProject.Tasks))
                return false;

            await _repository.UpdateAsync(id, updatedProject);
            return true;
        }

        public async Task<bool> DeleteProjectAsync(string id)
        {
            if (!ValidateId(id))
                return false;

            var project = await _repository.GetByIdAsync(id);
            if (!ValidateProject(project))
                return false;

            if (!ValidateTasks(project.Tasks))
                return false;

            await _repository.DeleteAsync(id);
            return true;
        }

        #region Validations
        private bool ValidateProject(Project project)
        {
            if (project != null)
                return true;

            AddNotification("O projeto não foi encontrado.");
            return false;
        }

        private bool ValidateTasks(IEnumerable<TaskModel> tasks)
        {
            if (tasks == null || !tasks.Any())
                return true;

            AddNotification("Não é possível excluir o projeto pois existem tarefas associadas a ele.");
            return false;
        }
        #endregion
    }
}
