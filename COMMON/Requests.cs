using System.ComponentModel.DataAnnotations;

namespace COMMON
{
    public class Requests
    {
        public class SignUpRequest
        {
            public string? UserName { get; set; }

            [EmailAddress]
            [Required(AllowEmptyStrings = false)]
            public string? Email { get; set; }
            public required string Password { get; set; }
        }   
    }
}
