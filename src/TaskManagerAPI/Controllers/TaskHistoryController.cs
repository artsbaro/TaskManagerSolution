using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Extensions;
using TaskManagerAPI.HttpObjects.Tasks;
using TaskManagerAPI.HttpObjects;
using TaskManagerAPI.Notifications.Interfaces;
using TaskManagerAPI.Services.Interfaces;
using TaskManagerAPI.Entities;

namespace TaskManagerAPI.Controllers
{
    public class TaskHistoryController : MainController
    {
        private readonly ITaskHistoryService _taskHistoryService;

        public TaskHistoryController(ITaskHistoryService taskHistoryService, INotifier notifier) : base(notifier)
        {
            _taskHistoryService = taskHistoryService;
        }

        /// <summary>
        /// Obtém o histórico de alterações de uma tarefa pelo seu ID.
        /// </summary>
        /// <param name="taskId">ID da tarefa.</param>
        /// <returns>Histórico de alterações.</returns>
        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetHistoryByTaskId(string taskId)
        {
            var history = await _taskHistoryService.GetHistoryByTaskIdAsync(taskId);

            return SimpleResponse(history);
        }

        /// <summary>
        /// Obtém o histórico de alterações de todas as tarefas de um projeto pelo ID do projeto.
        /// </summary>
        /// <param name="projectId">ID do projeto.</param>
        /// <returns>Histórico de alterações.</returns>
        [HttpGet("project/{projectId}")]
        [ProducesResponseType(typeof(SimpleResponseObject<IEnumerable<TaskHistory>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponseObject), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetHistoryByProjectId(string projectId)
        {
            var history = await _taskHistoryService.GetHistoryByProjectIdAsync(projectId);

            return SimpleResponse(history);
        }
    }
}
