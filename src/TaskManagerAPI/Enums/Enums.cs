namespace TaskManagerAPI.Enums
{
    public class Enum
    {
        public enum TaskStatus
        {
            Pending,      // Tarefa pendente
            InProgress,   // Tarefa em andamento
            Completed     // Tarefa concluída
        }

        public enum TaskPriority
        {
            Low,          // Prioridade baixa
            Medium,       // Prioridade média
            High          // Prioridade alta
        }
    }
}
