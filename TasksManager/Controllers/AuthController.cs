using BAL.IServices;
using COMMON;
using COMMON.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using static COMMON.Models.Records;
using static COMMON.Requests;

namespace TasksManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
                return CreateValidationProblemDetails("SignUp", ex.ToString(), 500);
            }
        }

        [HttpPost]
        [Route("SignIn")]
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

                return Ok(new SignInResponse(token, refreshToken));
            }
            catch (Exception ex)
            {
                return CreateValidationProblemDetails("SignIn", ex.ToString(), 500);
            }
        }

        [HttpPost]
        [Route("RefreshToken")]
        public IActionResult RefreshToken(RefreshTokenRequest request) 
        {
            try
            {
                bool result = _authService.RefreshToken(out RefreshTokenResponse response, request);

                if (!result)
                {
                    return CreateValidationProblemDetails("RefreshToken", "Invalid Refresh Token", 400);
                }

                return Ok(new RefreshTokenResponse(response.AccessToken, response.RefreshToken));
            }
            catch (Exception ex)
            {
                return CreateValidationProblemDetails("RefreshToken", ex.ToString(), 500);
            }
        }
    }
}
