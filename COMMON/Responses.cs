using COMMON.Models;

namespace COMMON
{
    public class Responses
    {
        public class RefreshTokenResponse 
        {
            public string AccessToken { get; set; } = string.Empty;
            public string RefreshToken { get; set; } = string.Empty;
        }

        public class SignInResponse 
        {
            public string AccessToken { get; set; } = string.Empty;
            public string RefreshToken { get; set; } = string.Empty;
        }

        public class GetLookupByTableNamesResponse() 
        {
            public Dictionary<string, List<Lookup>> Lookups { get; set; } = [];
        }
    }
}
