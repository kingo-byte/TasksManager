using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace COMMON.Models
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
