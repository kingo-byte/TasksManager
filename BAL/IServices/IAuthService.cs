using COMMON.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static COMMON.Models.Records;
using static COMMON.Requests;

namespace BAL.IServices
{
    public interface IAuthService
    {
        public long SignUp(SignUpRequest request);
        public bool ValidateSignUp(SignUpRequest request, out string message);
        public User? GetUserByCredentials(string? userName, string? email);
        public User? GetUserById(long userId);
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        public string CreateRefreshToken(long userId);
        public bool RefreshToken(out RefreshTokenResponse response, RefreshTokenRequest request);
        public string CreateToken(User user);
    }
}
