using COMMON.Models;

namespace COMMON
{
    public class Responses
    {
        public record RefreshTokenResponse(string AccessToken, string RefreshToken);
        public record SignInResponse(string AccessToken, string RefreshToken);

        public class GetLookupByTableNamesResponse() 
        {
            public Dictionary<string, List<Lookup>> Lookups { get; set; } = [];
        }
    }
}
