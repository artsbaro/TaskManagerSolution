using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.HttpObjects;
using TaskManagerAPI.HttpObjects.Projects;
using TaskManagerAPI.Notifications.Interfaces;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    public class ProjectsController : MainController
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService, INotifier notifier) : base(notifier)
        {
            _projectService = projectService;
        }

        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(SimpleResponseObject<IEnumerable<ProjectResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponseObject), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProjects(string userId)
        {
            var projects = await _projectService.GetProjectsByUserIdAsync(userId);
            return SimpleResponse(projects);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SimpleResponseObject<ProjectResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SimpleResponseObject), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProject([FromBody] ProjectCreateRequest project)
        {
            var projectCreated =  await _projectService.AddProjectAsync(project);

            return SimpleResponse(projectCreated);
        }
    }
}
