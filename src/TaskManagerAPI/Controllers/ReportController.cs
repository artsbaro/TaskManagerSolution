using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Entities;
using TaskManagerAPI.Extensions;
using TaskManagerAPI.HttpObjects;
using TaskManagerAPI.Notifications.Interfaces;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : MainController
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService,
            INotifier notifier) : base(notifier)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Relatório de desempenho: número médio de tarefas concluídas por usuário nos últimos 30 dias.
        /// </summary>
        /// <returns>Média de tarefas concluídas por usuário.</returns>
        [HttpGet("average-completed-tasks")]
        [ProducesResponseType(typeof(SimpleResponseObject<Dictionary<string, double>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponseObject), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SimpleResponseObject), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAverageCompletedTasksByUser([FromQuery] string userId)
        {
            try
            {
                var result = await _reportService.GetAverageCompletedTasksByUserAsync(userId);

                return SimpleResponse(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return SimpleResponseError(403, ex.Message);
            }
        }
    }
}
