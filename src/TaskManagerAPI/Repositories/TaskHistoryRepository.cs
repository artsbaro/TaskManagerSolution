using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Repositories.Interfaces;

namespace TaskManagerAPI.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TaskHistoryRepository : ITaskHistoryRepository
    {
        private readonly IMongoCollection<TaskHistory> _taskHistoryCollection;

        public TaskHistoryRepository(IMongoDatabase database)
        {
            _taskHistoryCollection = database.GetCollection<TaskHistory>("TaskHistories");
        }

        public  Task AddAsync(TaskHistory history)
        {
             return _taskHistoryCollection.InsertOneAsync(history);
        }

        public Task<List<TaskHistory>> GetHistoryByTaskIdAsync(string taskId)
        {
            var filter = Builders<TaskHistory>.Filter.Eq(h => h.TaskId, taskId);
            return _taskHistoryCollection.Find(filter).ToListAsync();
        }

        public Task<List<TaskHistory>> GetHistoryByProjectIdAsync(string projectId)
        {
            var filter = Builders<TaskHistory>.Filter.Eq(h => h.ProjectId, projectId);
            return _taskHistoryCollection.Find(filter).ToListAsync();
        }
    }
}
