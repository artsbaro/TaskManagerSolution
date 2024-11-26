using TaskManagerAPI.Entities;
using TaskManagerAPI.HttpObjects.Projects;

namespace TaskManagerAPI.Services.Interfaces
{
    public interface IProjectService 
    {
        Task<List<ProjectResponse>> GetProjectsByUserIdAsync(string userId);
        Task<ProjectResponse> GetProjectByIdAsync(string id);
        Task<bool> UpdateProjectAsync(string id, Project updatedProject);
        Task<bool> DeleteProjectAsync(string id);

        Task<ProjectResponse> AddProjectAsync(ProjectCreateRequest projectCreateRequest);
    }

}
