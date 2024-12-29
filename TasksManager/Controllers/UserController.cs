using COMMON;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static COMMON.Models.Model;

namespace TasksManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly BAL.BAL _bal;
        private readonly Configuration _configuration;

        public UserController(BAL.BAL bal, IOptions<Configuration> configuration) 
        {
            _bal = bal;
            _configuration = configuration.Value;
        }

        [HttpGet]
        [Route("{id}/Tasks")]
        public IActionResult GetUserTasks([FromRoute] long id)
        {
            try
            {
                User? user = _bal.GetUserWithTasks(id);
                
                return Ok(user);
            }
            catch (Exception ex)
            {
                return CreateValidationProblemDetails("GetUserTasks", ex.Message, 500);
            }
        }
    }
}
