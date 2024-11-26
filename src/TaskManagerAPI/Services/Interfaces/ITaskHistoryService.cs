using TaskManagerAPI.Entities;

namespace TaskManagerAPI.Services.Interfaces
{
    public interface ITaskHistoryService
    {
        //Task AddHistoryAsync(TaskHistory history);
        Task<List<TaskHistory>> GetHistoryByTaskIdAsync(string taskId);
        Task<List<TaskHistory>> GetHistoryByProjectIdAsync(string projectId);
    }
}
