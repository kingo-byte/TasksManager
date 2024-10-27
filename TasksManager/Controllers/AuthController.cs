using BAL.IServices;
using COMMON;
using COMMON.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

                string token = CreateToken(checkUser);

                return Ok(token);
            }
            catch (Exception ex)
            {
                return CreateValidationProblemDetails("SignIn", ex.Message, 500);
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("UserId", user.Id.ToString()),
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.JWT!.PrivateKey!));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            JwtSecurityToken token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: creds);

            string jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
