using TaskManagerAPI.Entities;

namespace TaskManagerAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(string userId);
        Task<List<User>> GetAllUsersAsync();
    }
}
