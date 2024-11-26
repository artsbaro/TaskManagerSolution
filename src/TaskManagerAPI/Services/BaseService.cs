using System.Diagnostics.CodeAnalysis;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Extensions;
using TaskManagerAPI.Notifications;
using TaskManagerAPI.Notifications.Interfaces;

namespace TaskManagerAPI.Services
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseService
    {
        private readonly INotifier _notifier;

        protected BaseService(INotifier notifier)
        {
            _notifier = notifier;
        }

        protected void AddNotification(string message)
        {
            _notifier.Handle(new Notification(message));
        }

        protected bool HasNotification()
        {
            return _notifier.HasNotification();
        }

        protected bool ValidateId(string id)
        {
            if (id.IsValidBsonId())
                return true;

            AddNotification("O ID fornecido não é válido.");
            return false;
        }

        protected bool ValidateCountTasks(List<TaskModel> tasks)
        {
            if (tasks?.Count >= 20)
            {
                AddNotification("A quantidade máxima de tarefas por projeto é 20");
                return false;
            }
            return true;
        }
    }
}
