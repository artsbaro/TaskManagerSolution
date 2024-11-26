using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.HttpObjects.Tasks
{
    public class TaskModelUpdateRequest
    {
        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProjectId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public Enums.Enum.TaskStatus Status { get; set; } // Usando o enum TaskStatus

        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ModifiedByUserId { get; set; } // Usuário que realizou a modificação
    }
}
