using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.HttpObjects.Projects
{
    public class ProjectCreateRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
    }
}
