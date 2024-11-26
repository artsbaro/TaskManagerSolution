using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Repositories.Interfaces;

namespace TaskManagerAPI.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TaskRepository : ITaskRepository
    {
        private readonly IMongoCollection<TaskModel> _taskCollection;

        public TaskRepository(IMongoDatabase database)
        {
            _taskCollection = database.GetCollection<TaskModel>("Tasks");
        }

        public async Task<List<TaskModel>> GetTasksByProjectIdAsync(string projectId)
        {
            var filter = Builders<TaskModel>.Filter.Eq(t => t.ProjectId, projectId);
            return await _taskCollection.Find(filter).ToListAsync();
        }

        public Task AddAsync(TaskModel task)
        {
            ArgumentNullException.ThrowIfNull(task);
            return _taskCollection.InsertOneAsync(task);
        }

        public Task UpdateAsync(TaskModel task)
        {
            if (string.IsNullOrWhiteSpace(task.Id)) throw new ArgumentNullException(nameof(task.Id));
            ArgumentNullException.ThrowIfNull(task);

            var filter = Builders<TaskModel>.Filter.Eq(t => t.Id, task.Id);
            return _taskCollection.ReplaceOneAsync(filter, task);
        }

        public  Task DeleteAsync(string taskId)
        {
            if (string.IsNullOrWhiteSpace(taskId)) throw new ArgumentNullException(nameof(taskId));

            var filter = Builders<TaskModel>.Filter.Eq(t => t.Id, taskId);
             return _taskCollection.DeleteOneAsync(filter);
        }

        public async Task<TaskModel> GetByIdAsync(string taskId)
        {
            var obj = _taskCollection.Find(t => t.Id == taskId).FirstOrDefault();
            return obj;
        }

        public async Task<List<TaskModel>> GetTasksByFilterAsync(Expression<Func<TaskModel, bool>> filter)
        {
            return await _taskCollection.Find(filter).ToListAsync();
        }
    }
}
