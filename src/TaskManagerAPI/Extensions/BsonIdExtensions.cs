using MongoDB.Bson;

namespace TaskManagerAPI.Extensions
{
    public static class BsonIdExtensions
    {
        /// <summary>
        /// Verifica se um ID é válido como BsonId (ObjectId do MongoDB).
        /// </summary>
        /// <param name="id">ID a ser validado.</param>
        /// <returns>True se o ID for válido, caso contrário, false.</returns>
        public static bool IsValidBsonId(this string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;            

            return ObjectId.TryParse(id, out _);
        }
    }

}
