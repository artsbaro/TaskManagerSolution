using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TaskManagerAPI.Entities
{
    public abstract class EntityBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}