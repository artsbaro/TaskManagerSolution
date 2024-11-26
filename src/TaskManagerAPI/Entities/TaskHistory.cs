using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TaskManagerAPI.Entities
{
    public class TaskHistory : EntityBase
    {
        public string TaskId { get; set; } // Referência à Task original
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public byte Status { get; set; }
        public byte Priority { get; set; }
        public string ProjectId { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public string ModifiedByUserId { get; set; } // Usuário que realizou a modificação
        public string Action { get; set; } // Ação realizada (criação, atualização, exclusão, etc.)
    }
}
