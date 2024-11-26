using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using TaskManagerAPI.HttpObjects.Tasks;

namespace TaskManagerAPI.HttpObjects.Projects
{
    public class ProjectResponse : ResponseBase
    {
        public string Name { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        public List<TaskModelResponse> Tasks { get; set; } = new List<TaskModelResponse>();

    }
}
