using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace TasksManager.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IActionResult CreateValidationProblemDetails(string source, string errorMessage, int statusCode)
        {
            ModelState.AddModelError(source, errorMessage);

            string traceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            ValidationProblemDetails problemDetails = new ValidationProblemDetails(ModelState)
            {
                Status = statusCode,
            };

            problemDetails.Extensions["traceId"] = traceId;

            return StatusCode(statusCode, problemDetails);
        }
    }
}
