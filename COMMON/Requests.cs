using System.ComponentModel.DataAnnotations;

namespace COMMON
{
    public class Requests
    {
        public class SignUpRequest
        {
            [MaxLength(250)]
            public string? UserName { get; set; }

            [EmailAddress]
            [MaxLength(250)]
            public string? Email { get; set; }
            public required string Password { get; set; }
        }

        public class SignInRequest
        {
            [MaxLength(250)]
            public string? UserName { get; set; }

            [EmailAddress]
            [MaxLength(250)]
            public string? Email { get; set; }
            public required string Password { get; set; }
        }

        public class EditTaskRequest
        {
            public required long TaskId { get; set; }
            public required long UserId { get; set; }
            public required string CategoryCode { get; set; }

            [MaxLength(250)]
            public required string Title { get; set; }

            [MaxLength(500)]
            public required string Description { get; set; }
            public DateTime DueDate { get; set; }
        }

        public class RefreshTokenRequest
        {
            public required string RefreshToken { get; set; }
        }

        public class GetLookupByTableNamesRequest
        {
            public required string TableNames { get; set; }
        }
    }
}
