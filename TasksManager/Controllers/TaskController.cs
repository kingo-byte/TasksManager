using BAL.IServices;
using BAL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static COMMON.Requests;

namespace TasksManager.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TaskController : BaseController
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        [Route("EditTask")]
        public IActionResult EditTask(EditTaskRequest request)
        {
            try 
            {
                long taskId = _taskService.EditTask(request);

                return Ok(taskId);  
            }
            catch (Exception ex)
            {
                return CreateValidationProblemDetails("EditTask", ex.Message, 500);
            }   
        }
    }
}
