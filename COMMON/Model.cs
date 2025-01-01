using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace COMMON.Models
{
    #region User
    public class Model
    {
        public class User
        {
            [Key]
            public long Id { get; set; }

            public string? UserName { get; set; } = string.Empty;

            public string? Email { get; set; } = string.Empty;

            [JsonIgnore]
            public byte[]? PasswordHash { get; set; }

            [JsonIgnore]
            public byte[]? PasswordSalt { get; set; }

            public DateTime? CreatedDate { get; set; }

            public DateTime? LastModifiedDate { get; set; }

            public List<Task>? Tasks { get; set; }
        }
    }
    #endregion

    #region Task
    public class Task
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
    #endregion

    #region RefreshToken
    public class RefreshToken
    {
        public long Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public long UserId { get; set; }
        public DateTime ExpiresOnUtc { get; set; }
    }
    #endregion

    #region Lookup
    public class Lookup
    {
        public int Id { get; set; }
        public string TableName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
    #endregion
}
