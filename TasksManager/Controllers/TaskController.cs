using BAL.IServices;
using BAL.Services;
using COMMON.Models;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUserService _userService;

        public TaskController(ITaskService taskService, IUserService userService)
        {
            _taskService = taskService;
            _userService = userService;
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
