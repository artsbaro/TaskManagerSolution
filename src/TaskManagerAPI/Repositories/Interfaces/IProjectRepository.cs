using TaskManagerAPI.Entities;

namespace TaskManagerAPI.Repositories.Interfaces
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetProjectsByUserIdAsync(string userId);
        Task<Project> GetByIdAsync(string id);
        Task AddAsync(Project project);
        Task UpdateAsync(string id, Project updatedProject);
        Task DeleteAsync(string id);
    }
}
