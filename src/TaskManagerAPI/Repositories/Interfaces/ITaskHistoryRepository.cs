using TaskManagerAPI.Entities;

namespace TaskManagerAPI.Repositories.Interfaces
{
    public interface ITaskHistoryRepository
    {
        Task AddAsync(TaskHistory history);
        Task<List<TaskHistory>> GetHistoryByTaskIdAsync(string taskId);
        Task<List<TaskHistory>> GetHistoryByProjectIdAsync(string projectId);
    }
}
