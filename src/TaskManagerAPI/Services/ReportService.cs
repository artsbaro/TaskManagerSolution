using TaskManagerAPI.Notifications.Interfaces;
using TaskManagerAPI.Repositories.Interfaces;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Services
{
    public class ReportService : BaseService, IReportService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        public ReportService(ITaskRepository taskRepository,
            INotifier notifier,
            IUserRepository userRepository) : base(notifier)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        public async Task<Dictionary<string, double>> GetAverageCompletedTasksByUserAsync(string userId)
        {
            if (!ValidateId(userId))
                return null;

            // Validar se o usuário é "gerente"
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || user.Category != "gerente")
            {
                throw new UnauthorizedAccessException("Apenas usuários com a categoria 'gerente' podem acessar este relatório.");
            }

            // Obtenha todas as tarefas concluídas nos últimos 30 dias
            var completedTasks = await _taskRepository.GetTasksByFilterAsync(
                t => t.Status == Enums.Enum.TaskStatus.Completed && t.DueDate >= DateTime.UtcNow.AddDays(-30));

            // Agrupe as tarefas por usuário e calcule a média
            var groupedByUser = completedTasks
                .GroupBy(t => t.ModifiedByUserId)
                .ToDictionary(
                    g => g.Key,
                    g => Math.Round((double)g.Count(), 2) // Média de tarefas concluídas
                );

            return groupedByUser;
        }
    }
}
