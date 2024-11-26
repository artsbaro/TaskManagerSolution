using MongoDB.Driver;
using TaskManagerAPI.Entities;

namespace TaskManagerAPI.Repositories
{
    public abstract class BaseRepository<T>
    {
        protected readonly IMongoCollection<T> Collection;

        protected BaseRepository(IMongoDatabase database, string collectionName)
        {
            Collection = database.GetCollection<T>(collectionName);
        }

        public async Task<List<T>> GetAllAsync() => await Collection.Find(_ => true).ToListAsync();
        public async Task<T> GetByIdAsync(string id) =>
            await Collection.Find(Builders<T>.Filter.Eq("_id", id)).FirstOrDefaultAsync();
        public async Task AddAsync(T entity) => await Collection.InsertOneAsync(entity);        public async Task UpdateAsync(string id, T entity) =>
            await Collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", id), entity);
        public async Task DeleteAsync(string id) =>
            await Collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", id));
    }


}
