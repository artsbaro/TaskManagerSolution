using TaskManagerAPI.Entities;
using TaskManagerAPI.Notifications.Interfaces;
using TaskManagerAPI.Repositories.Interfaces;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Services
{
    public class TaskHistoryService : BaseService, ITaskHistoryService
    {
        private readonly ITaskHistoryRepository _taskHistoryRepository;

        public TaskHistoryService(ITaskHistoryRepository taskHistoryRepository,
            INotifier notifier) : base(notifier)
        {
            _taskHistoryRepository = taskHistoryRepository;
        }

        /*
         * A Adição é feita diretamente no repositório, pois é uma operação que não precisa de validação
         * e não é necessário notificar o usuário.
        public async Task AddHistoryAsync(TaskHistory history)
        {
            await _taskHistoryRepository.AddAsync(history);
        }
        */

        public async Task<List<TaskHistory>> GetHistoryByTaskIdAsync(string taskId)
        {
            if (!ValidateId(taskId))
                return null;

            return await _taskHistoryRepository.GetHistoryByTaskIdAsync(taskId);
        }

        public async Task<List<TaskHistory>> GetHistoryByProjectIdAsync(string projectId)
        {
            if (!ValidateId(projectId))
                return null;

            return await _taskHistoryRepository.GetHistoryByProjectIdAsync(projectId);
        }
    }
}
