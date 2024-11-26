using MongoDB.Bson;
using System.Threading.Tasks;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Extensions;
using TaskManagerAPI.HttpObjects.Tasks;
using TaskManagerAPI.Notifications.Interfaces;
using TaskManagerAPI.Repositories.Interfaces;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Services
{
    public class TaskService : BaseService, ITaskService
    {
        private readonly ITaskRepository _repository;
        private readonly ITaskHistoryRepository _taskHistoryRepository;
        private readonly IProjectRepository _projectRepository;

        public TaskService(ITaskRepository repository,
            IProjectRepository projectRepository,
            INotifier notifier,
            ITaskHistoryRepository taskHistoryRepository) : base(notifier)
        {
            _repository = repository;
            _projectRepository = projectRepository;
            _taskHistoryRepository = taskHistoryRepository;
        }

        public async Task<List<TaskModelResponse>> GetTasksByProjectIdAsync(string projectId)
        {
            if (!ValidateId(projectId))
                return default;

            var tasks = await _repository.GetTasksByProjectIdAsync(projectId);
            return tasks.Select(MapToTaskModelResponse).ToList();
        }

        public async Task<TaskModelResponse> AddTaskAsync(TaskModelCreateRequest task)
        {
            if (!ValidateId(task.ProjectId))
                return default;

            if (!task.ModifiedByUserId.IsValidBsonId())
            {
                AddNotification("O ID do usuário que modificou a tarefa é inválido.");
                return default;
            }

            if (!ValidateTasksOnProject(task.ProjectId))
                return default;

            TaskModel taskModel = MapToTaskModel(task);

            await _repository.AddAsync(taskModel);

            // Grava no histórico
            await _taskHistoryRepository.AddAsync(MapToTaskHistory(taskModel, ActionHistory.Created));

            return MapToTaskModelResponse(taskModel);
        }

        public async Task<bool> UpdateTaskAsync(TaskModelUpdateRequest taskModelUpdateRequest)
        {
            if ((!ValidateId(taskModelUpdateRequest.Id)) || (!ValidateId(taskModelUpdateRequest.ProjectId)))
                return false;

            if (!taskModelUpdateRequest.ModifiedByUserId.IsValidBsonId())
            {
                AddNotification("O ID do usuário que modificou a tarefa é inválido.");
                return false;
            }

            if (!ValidateTasksOnProject(taskModelUpdateRequest.ProjectId))
                return false;

            TaskModel taskModel = MapToTaskModel(taskModelUpdateRequest);

            await _repository.UpdateAsync(taskModel);

            await _taskHistoryRepository.AddAsync(MapToTaskHistory(taskModel, ActionHistory.Updated));

            return true;
        }

        public async Task<bool> DeleteTaskAsync(string taskId, string modifiedByUserId)
        {
            if (!ValidateId(taskId))
                return false;

            if(!modifiedByUserId.IsValidBsonId())
                AddNotification("O ID do usuário que modificou a tarefa é inválido.");

            var task = await _repository.GetByIdAsync(taskId);

            await _repository.DeleteAsync(taskId);

            if (task != null)
            {
                var taskHistory = MapToTaskHistory(task, ActionHistory.Deleted);
                taskHistory.ModifiedByUserId = modifiedByUserId;

                await _taskHistoryRepository.AddAsync(taskHistory);
            }
            return true;
        }

        #region Private
        private TaskModel MapToTaskModel(TaskModelCreateRequest task)
        {
            return new TaskModel
            {
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                ProjectId = task.ProjectId,
                Id = ObjectId.GenerateNewId().ToString(),
                DueDate = task.DueDate,
                Priority = task.Priority,
                ModifiedByUserId = task.ModifiedByUserId
            };
        }

        private TaskModelResponse MapToTaskModelResponse(TaskModel taskModel)
        {
            return new TaskModelResponse
            {
                Id = taskModel.Id,
                Title = taskModel.Title,
                Description = taskModel.Description,
                Status = taskModel.Status,
                ProjectId = taskModel.ProjectId,
                DueDate = taskModel.DueDate,
                Priority = taskModel.Priority
            };
        }

        private TaskModel MapToTaskModel(TaskModelUpdateRequest taskModelUpdateRequest)
        {
            return new TaskModel
            {
                Id = taskModelUpdateRequest.Id,
                Title = taskModelUpdateRequest.Title,
                Description = taskModelUpdateRequest.Description,
                Status = taskModelUpdateRequest.Status,
                ProjectId = taskModelUpdateRequest.ProjectId,
                DueDate = taskModelUpdateRequest.DueDate,
                ModifiedByUserId = taskModelUpdateRequest.ModifiedByUserId,
            };
        }

        private TaskHistory MapToTaskHistory(TaskModel taskModel, ActionHistory actionHistory)
        {
            return new TaskHistory
            {
                TaskId = taskModel.Id,
                ModifiedAt = DateTime.UtcNow,
                ModifiedByUserId = taskModel.ModifiedByUserId,
                Status = (byte)taskModel.Status,
                Title = taskModel.Title,
                Description = taskModel.Description,
                DueDate = taskModel.DueDate,
                Priority = (byte)taskModel.Priority,
                Action = actionHistory.ToString()
            };
        }
        private bool ValidateTasksOnProject(string projectId)
        {
            var project = _projectRepository.GetByIdAsync(projectId).GetAwaiter().GetResult();

            if (project == null)
            {
                AddNotification("O projeto associado à tarefa não foi encontrado.");
                return false;
            }

            var tasks = _repository.GetTasksByProjectIdAsync(projectId).GetAwaiter().GetResult();

            if (!ValidateCountTasks(tasks))
                return false;

            return true;
        }

        enum ActionHistory
        {
            Created,
            Updated,
            Deleted
        } 
        #endregion
    }
}
