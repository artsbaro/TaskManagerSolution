using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Extensions;
using TaskManagerAPI.HttpObjects;
using TaskManagerAPI.HttpObjects.Tasks;
using TaskManagerAPI.Notifications.Interfaces;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : MainController
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService, INotifier notifier) : base(notifier)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Obtém todas as tarefas de um projeto.
        /// </summary>
        [HttpGet("project/{projectId}")]
        [ProducesResponseType(typeof(SimpleResponseObject<IEnumerable<TaskModelResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponseObject), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTasksByProject(string projectId)
        {

            var tasks = await _taskService.GetTasksByProjectIdAsync(projectId);
            return SimpleResponse(tasks);
        }

        /// <summary>
        /// Adiciona uma nova tarefa.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(SimpleResponseObject<TaskModelResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponseObject), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTask([FromBody] TaskModelCreateRequest taskModelCreateRequest)
        {
            var taskResponse = await _taskService.AddTaskAsync(taskModelCreateRequest);

            return SimpleResponse(taskResponse);
        }

        /// <summary>
        /// Atualiza uma tarefa existente.
        /// </summary>
        [HttpPut()]
        [ProducesResponseType(typeof(SimpleResponseObject<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponseObject), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTask([FromBody] TaskModelUpdateRequest updatedTaskModel)
        {
            if (!updatedTaskModel.Id.IsValidBsonId())
                return BadRequest("O TaskId fornecido não é válido.");

            if (updatedTaskModel == null)
                return BadRequest("O modelo da tarefa não pode ser nulo.");

            var result = await _taskService.UpdateTaskAsync(updatedTaskModel);

            return SimpleResponse(result);
        }

        /// <summary>
        /// Remove uma tarefa.
        /// </summary>
        [HttpDelete("{taskId}/{modifiedByUserId}")]
        [ProducesResponseType(typeof(SimpleResponseObject<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponseObject), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteTask(string taskId, string modifiedByUserId)
        {
            await _taskService.DeleteTaskAsync(taskId, modifiedByUserId);

            return SimpleResponse();
        }
    }
}
