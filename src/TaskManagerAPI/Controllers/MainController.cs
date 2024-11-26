using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TaskManagerAPI.HttpObjects;
using TaskManagerAPI.Notifications;
using TaskManagerAPI.Notifications.Interfaces;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public abstract class MainController : ControllerBase
    {
        private readonly INotifier _notifier;

        protected MainController(INotifier notificador)
        {
            _notifier = notificador;
        }

        protected bool ValidOperation()
        {
            return !_notifier.HasNotification();
        }

        protected ActionResult SimpleResponse(object result = null)
        {
            if (ValidOperation())
            {
                return Ok(new SimpleResponseObject
                {
                    Success = true,
                    Data = result
                });
            }

            return BadRequest(new SimpleResponseObject
            {
                Success = false,
                Errors = _notifier.GetNotifications().Select(n => n.Message)
            });

        }

        protected ActionResult SimpleResponseError(int statusCode, object result = null)
        {
            return StatusCode(statusCode, new SimpleResponseObject
            {
                Success = false,
                Data = result,
                Errors = _notifier.GetNotifications().Select(n => n.Message)
            });
        }

        protected ActionResult SimpleResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotificarErroModelInvalida(modelState);
            return SimpleResponse();
        }

        private void NotificarErroModelInvalida(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(e => e.Errors);

            foreach (var error in errors)
            {
                var errorMsg = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                ReportError(errorMsg);
            }
        }

        protected void ReportError(string message)
        {
            _notifier.Handle(new Notification(message));
        }
    }
}
