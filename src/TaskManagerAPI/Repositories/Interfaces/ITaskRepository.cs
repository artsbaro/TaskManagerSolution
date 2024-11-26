using System.Linq.Expressions;
using TaskManagerAPI.Entities;

namespace TaskManagerAPI.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<List<TaskModel>> GetTasksByProjectIdAsync(string projectId);
        Task<TaskModel> GetByIdAsync(string taskId);
        Task AddAsync(TaskModel task);
        Task UpdateAsync(TaskModel task);
        Task DeleteAsync(string taskId);

        Task<List<TaskModel>> GetTasksByFilterAsync(Expression<Func<TaskModel, bool>> filter);
    }

}
