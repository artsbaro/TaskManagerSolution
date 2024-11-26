using TaskManagerAPI.Entities;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using TaskManagerAPI.Repositories.Interfaces;

namespace TaskManagerAPI.Repositories
{
    [ExcludeFromCodeCoverage]
    public class ProjectRepository : IProjectRepository
    {
        private readonly IMongoCollection<Project> _collection;

        public ProjectRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Project>("Projects");
        }

        public Task<List<Project>> GetProjectsByUserIdAsync(string userId)
        {
            var filter = Builders<Project>.Filter.Eq(p => p.UserId, userId);
            return _collection.Find(filter).ToListAsync();
        }

        public Task<Project> GetByIdAsync(string id)
        {
            return _collection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public Task AddAsync(Project project)
        {
            return _collection.InsertOneAsync(project);
        }

        public Task UpdateAsync(string id, Project updatedProject)
        {
            var filter = Builders<Project>.Filter.Eq(p => p.Id, id);
            return _collection.ReplaceOneAsync(filter, updatedProject);
        }

        public Task DeleteAsync(string id)
        {
            var filter = Builders<Project>.Filter.Eq(p => p.Id, id);
            return _collection.DeleteOneAsync(filter);
        }
    }

}
