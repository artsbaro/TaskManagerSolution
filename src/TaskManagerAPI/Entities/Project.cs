
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TaskManagerAPI.Entities
{
    public class Project : EntityBase
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        public string Name { get; set; }

        public List<TaskModel> Tasks { get; set; } = new List<TaskModel>();
    }
}
