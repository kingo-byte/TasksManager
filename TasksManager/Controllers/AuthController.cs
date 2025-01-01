using BAL.IServices;
using COMMON;
using COMMON.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static COMMON.Models.Model;
using static COMMON.Requests;
using static COMMON.Responses;

namespace TasksManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;
        private readonly Configuration _configuration;

        public AuthController(ILogger<AuthController> logger, IAuthService authService, IOptions<Configuration> configuration)
        {
            _logger = logger;
            _authService = authService;
            _configuration = configuration.Value;
        }

        [HttpPost("SignUp")]
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
                return CreateValidationProblemDetails("SignUp", ex.ToString(), 500);
            }
        }

        [HttpPost("SignIn")]
        public IActionResult SignIn(SignInRequest request)
        {
            try
            {
                User? checkUser = _authService.GetUserByCredentials(request.UserName, request.Email);

                if (checkUser == null)
                {
                    return CreateValidationProblemDetails("SignIn", "Invalid Credentials", 400);
                }

                if (!_authService.VerifyPasswordHash(request.Password, checkUser.PasswordHash!, checkUser.PasswordSalt!))
                {
                    return BadRequest("Invalid Password");
                }

                string token = _authService.CreateToken(checkUser);
                string refreshToken = _authService.CreateRefreshToken(checkUser.Id);

                return Ok(new SignInResponse() { AccessToken = token, RefreshToken = refreshToken });
            }
            catch (Exception ex)
            {
                return CreateValidationProblemDetails("SignIn", ex.ToString(), 500);
            }
        }

        [HttpPost("RefreshToken")]
        public IActionResult RefreshToken(RefreshTokenRequest request)
        {
            try
            {
                bool result = _authService.RefreshToken(out RefreshTokenResponse response, request);

                if (!result)
                {
                    return CreateValidationProblemDetails("RefreshToken", "Invalid Refresh Token", 400);
                }

                return Ok(new RefreshTokenResponse() { AccessToken = response.AccessToken, RefreshToken = response.RefreshToken });
            }
            catch (Exception ex)
            {
                return CreateValidationProblemDetails("RefreshToken", ex.ToString(), 500);
            }
        }
    }
}
