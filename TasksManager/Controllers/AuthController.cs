using BAL.IServices;
using COMMON.Models;
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

        [HttpPost]
        [Route("SignIn")]
        public IActionResult SignIn(SignInRequest request) 
        {
            try
            {
                User? checkUser = _authService.GetUserByCredentials(request);

                if (checkUser == null)
                {
                    return CreateValidationProblemDetails("SignIn", "Invalid Credentials", 400);
                }

                if (!_authService.VerifyPasswordHash(request.Password, checkUser.PasswordHash, checkUser.PasswordSalt))
                {
                    return BadRequest("Invalid Password");
                }

                //string token = CreateToken(checkUser);

                return Ok("test token");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
