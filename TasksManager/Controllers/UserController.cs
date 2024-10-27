using BAL.IServices;
using COMMON;
using COMMON.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace TasksManager.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly Configuration _configuration;

        public UserController(IUserService userService, IOptions<Configuration> configuration) 
        {
            _userService = userService;
            _configuration = configuration.Value;
        }

        [HttpGet]
        [Route("{id}/Tasks")]
        public IActionResult GetUserTasks([FromRoute] long id)
        {
            try
            {
                User? user = _userService.GetUserWithTasks(id);
                
                return Ok(user);
            }
            catch (Exception ex)
            {
                return CreateValidationProblemDetails("GetUserTasks", ex.Message, 500);
            }
        }
    }
}
