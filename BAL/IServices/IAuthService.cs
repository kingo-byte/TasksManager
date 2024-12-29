using static COMMON.Models.Model;
using static COMMON.Requests;
using static COMMON.Responses;

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
