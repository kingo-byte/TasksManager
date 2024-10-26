using COMMON;
using DAL.DapperAccess;
using DAL.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static COMMON.Requests;

namespace BAL.Services
{
    public class AuthService : IAuthService
    {
        private readonly DapperAccess _dapperAccess;    
        public AuthService(DapperAccess dapperAccess)
        {
            _dapperAccess = dapperAccess;
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
            parameters.Add("P__Username", user.UserName);
            parameters.Add("P__Email", user.Email);
            parameters.Add("P__PasswordHash", user.PasswordHash);
            parameters.Add("P__PasswordSalt", user.PasswordSalt);

            _dapperAccess.Execute("sp_SignUp", parameters); 

            long userId = parameters.Get<long>("P__Id"); 

            return userId;
        }

        public bool ValidateSignUp(SignUpRequest request, out string message) 
        {
            message = string.Empty;

            if(string.IsNullOrWhiteSpace(request.UserName) && string.IsNullOrWhiteSpace(request.Email))
            {
                message = $"Either UserName or Email is required for SignUp";
                return false;
            }

            if (string.IsNullOrWhiteSpace(request.Password)) 
            {
                message = "Password is required";
                return false;
            }

            return true;    
        }

        private (byte[], byte[]) CreatePasswordHash(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                byte[] passwordSalt = hmac.Key;
                byte[] passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                return (passwordSalt, passwordHash);
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
