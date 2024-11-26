using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TaskManagerAPI.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        public string Name { get; set; }
        public string Category { get; set; } // Categoria: Exemplo "gerente"
    }
}
