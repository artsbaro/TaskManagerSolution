using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using static TaskManagerAPI.Enums.Enum;

namespace TaskManagerAPI.Entities
{
    public class TaskModel : EntityBase
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }

        public Enums.Enum.TaskStatus Status { get; set; } // Usando o enum TaskStatus
        public TaskPriority Priority { get; set; } // Usando o enum TaskPriority

        [BsonRepresentation(BsonType.ObjectId)]
        public string ModifiedByUserId { get; set; } // Usuário que realizou a modificação

    }
}
