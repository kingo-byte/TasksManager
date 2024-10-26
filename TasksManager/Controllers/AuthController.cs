using BAL.Services;
using Microsoft.AspNetCore.Mvc;
using static COMMON.Requests;

namespace TasksManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost]
        [Route("SignUp")]
        public IActionResult SignUp(SignUpRequest request) 
        {
            try
            {
                if (!_authService.ValidateSignUp(request, out string message)) 
                {
                    return CreateValidationProblemDetails("SignUp", message, 400);
                }

                long userId = _authService.SignUp(request);

                return Ok(userId);
            }
            catch (Exception ex) 
            {
                return CreateValidationProblemDetails("SignUp", ex.Message, 500);
            }
        }
    }
}
