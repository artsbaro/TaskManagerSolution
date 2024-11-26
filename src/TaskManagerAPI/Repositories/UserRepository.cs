using MongoDB.Bson;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Repositories.Interfaces;

namespace TaskManagerAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new()
        {
            new User { UserId = ObjectId.Parse("64b9f0e3d6a6341c8b3d9c01").ToString(), Name = "Alice", Category = "gerente" },
            new User { UserId = ObjectId.Parse("64b9f0e3d6a6341c8b3d9c02").ToString(), Name = "Bob", Category = "desenvolvedor" },
            new User { UserId = ObjectId.Parse("64b9f0e3d6a6341c8b3d9c03").ToString(), Name = "Charlie", Category = "analista" }
        };
        public UserRepository()
        {

        }

        public Task<User> GetUserByIdAsync(string userId)
        {
            var user = _users.FirstOrDefault(u => u.UserId == userId);
            return Task.FromResult(user);
        }

        public Task<List<User>> GetAllUsersAsync()
        {
            return Task.FromResult(_users);
        }
    }
}
