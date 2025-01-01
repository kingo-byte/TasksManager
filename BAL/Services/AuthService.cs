using BAL.IServices;
using COMMON;
using COMMON.Models;
using DAL.DapperAccess;
using Dapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using static COMMON.Models.Model;
using static COMMON.Requests;
using static COMMON.Responses;

namespace BAL.Services
{
    public class AuthService : IAuthService
    {
        private readonly DapperAccess _dapperAccess;
        private readonly Configuration _configuration;

        public AuthService(DapperAccess dapperAccess, IOptions<Configuration> configuration)
        {
            _dapperAccess = dapperAccess;
            _configuration = configuration.Value;
        }

        public User? GetUserByCredentials(string? userName, string? email)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("P__UserName", userName);
            parameters.Add("P__Email", email);

            return _dapperAccess.QueryFirst<User>("sp_GetUserByCredentials", parameters);
        }

        public User? GetUserById(long userId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("P__UserId", userId);

            return _dapperAccess.QueryFirst<User>("sp_GetUserById", parameters);
        }

        public long SignUp(SignUpRequest request)
        {
            (byte[] passwordSalt, byte[] passwordHash) = CreatePasswordHash(request.Password);

            User user = new User()
            {
                Email = request.Email,
                UserName = request.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("P__Id", user.Id, direction: ParameterDirection.InputOutput);
            parameters.Add("P__Username", user.UserName?.Trim());
            parameters.Add("P__Email", user.Email?.Trim());
            parameters.Add("P__PasswordHash", user.PasswordHash);
            parameters.Add("P__PasswordSalt", user.PasswordSalt);

            _dapperAccess.Execute("sp_SignUp", parameters);

            long userId = parameters.Get<long>("P__Id");

            return userId;
        }

        public string CreateRefreshToken(long userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            string refreshToken = $"{Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))}-{Guid.NewGuid()}";

            parameters.Add("P__Token", refreshToken);
            parameters.Add("P__UserId", userId);
            parameters.Add("P__ExpiresOnUtc", DateTime.UtcNow.AddDays(7));

            _dapperAccess.Execute("sp_CreateRefreshToken", parameters);

            return refreshToken;
        }

        public bool RefreshToken(out RefreshTokenResponse response, RefreshTokenRequest request)
        {
            response = new RefreshTokenResponse() { AccessToken = string.Empty, RefreshToken = string.Empty };

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("P__Token", request.RefreshToken);

            RefreshToken? refreshToken = _dapperAccess.QueryFirst<RefreshToken>("sp_GetRefreshToken", parameters);

            if (refreshToken == null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
            {
                return false;
            }

            User loggedInUser = GetUserById(refreshToken.UserId)!;

            string updatedAccessToken = CreateToken(loggedInUser);
            string updatedRefreshToken = CreateRefreshToken(refreshToken.UserId);

            response = new RefreshTokenResponse() { AccessToken = updatedAccessToken, RefreshToken = updatedRefreshToken };

            return true;
        }

        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("Guid", Guid.NewGuid().ToString()),
                new Claim("UserId", user.Id.ToString()),
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.JWT!.PrivateKey!));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            JwtSecurityToken token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes((double)_configuration.JWT.ExpiryTimeInMinutes!),
                signingCredentials: creds);

            string jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        public bool ValidateSignUp(SignUpRequest request, out string message)
        {
            message = string.Empty;

            if (string.IsNullOrWhiteSpace(request.UserName) && string.IsNullOrWhiteSpace(request.Email))
            {
                message = $"Either UserName or Email is required for SignUp";
                return false;
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                message = "Password is required";
                return false;
            }

            //Check user Email if it is alreay taken
            DynamicParameters parametersOne = new DynamicParameters();

            parametersOne.Add("P__Email", request.Email?.Trim());
            User? checkUserEmail = _dapperAccess.QueryFirst<User>("sp_GetUserByEmail", parametersOne);

            if (checkUserEmail != null)
            {
                message = "Email is not available";
                return false;
            }


            //Check user Email if it is alreay taken
            DynamicParameters parametersTwo = new DynamicParameters();

            parametersTwo.Add("P__UserName", request.UserName?.Trim());
            User? checkUserUserName = _dapperAccess.QueryFirst<User>("sp_GetUserByUserName", parametersTwo);

            if (checkUserUserName != null)
            {
                message = "UserName is not available";
                return false;
            }

            return true;
        }
        public (byte[], byte[]) CreatePasswordHash(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                byte[] passwordSalt = hmac.Key;
                byte[] passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                return (passwordSalt, passwordHash);
            }
        }
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
