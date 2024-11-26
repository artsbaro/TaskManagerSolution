using TaskManagerAPI.Entities;
using TaskManagerAPI.HttpObjects.Tasks;

namespace TaskManagerAPI.Services.Interfaces
{
    public interface ITaskService
    {
        Task<List<TaskModelResponse>> GetTasksByProjectIdAsync(string projectId);
        Task<TaskModelResponse> AddTaskAsync(TaskModelCreateRequest task);
        Task<bool> UpdateTaskAsync(TaskModelUpdateRequest taskModelUpdateRequest);
        Task<bool> DeleteTaskAsync(string taskId, string modifiedByUserId);
    }

}
