namespace COMMON.DTOs
{
    public class Dtos
    {
        public class UserDto
        {
            public long Id { get; set; }

            public string? UserName { get; set; } = string.Empty;

            public string? Email { get; set; } = string.Empty;

            public DateTime? CreatedDate { get; set; }

            public DateTime? LastModifiedDate { get; set; }

            public List<TaskDto>? Tasks { get; set; }
        }

        public class TaskDto
        {
            public int Id { get; set; }
            public long UserId { get; set; }
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public DateTime DueDate { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime LastModifiedDate { get; set; }
        }
    }
}
