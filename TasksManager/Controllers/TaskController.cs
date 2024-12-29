using Microsoft.AspNetCore.Mvc;
using static COMMON.Requests;

namespace TasksManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : BaseController
    {
        private readonly BAL.BAL _bal;
        public TaskController(BAL.BAL bal)
        {
            _bal = bal;
        }

        [HttpPost]
        [Route("EditTask")]
        public IActionResult EditTask(EditTaskRequest request)
        {
            try
            {
                long taskId = _bal.EditTask(request);

                return Ok(taskId);
            }
            catch (Exception ex)
            {
                return CreateValidationProblemDetails("EditTask", ex.Message, 500);
            }
        }
    }
}
