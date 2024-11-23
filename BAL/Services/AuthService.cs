using BAL.Events.Auth;
using BAL.IServices;
using COMMON.Models;
using DAL.DapperAccess;
using Dapper;
using System.Data;
using System.Security.Cryptography;
using static COMMON.Requests;

namespace BAL.Services
{
    public class AuthService : IAuthService
    {
        private readonly DapperAccess _dapperAccess;
        private readonly AuthMain _authMain;
        private readonly AuthEvents _authEvents; 

        public AuthService(DapperAccess dapperAccess, AuthMain authMain, AuthEvents authEvents)
        {
            _dapperAccess = dapperAccess;
            _authMain = authMain;
            _authEvents = authEvents;
            _authMain.InitializeEvents();
        }

        public User? GetUserByCredentials(SignInRequest request)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("P__UserName", request.UserName);
            parameters.Add("P__Email", request.Email);

            return _dapperAccess.QueryFirst<User>("sp_GetUserByCredentials", parameters);    
        }

        public long SignUp(SignUpRequest request)
        {
            _authEvents.InvokePreEventSignUp(ref request);

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
