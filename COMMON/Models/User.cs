using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class User
    {
        [Key]
        public long Id { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [StringLength(250)]
        public string? UserName { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(250)")]
        [StringLength(250)]
        public string? Email { get; set; } = string.Empty;

        [JsonIgnore]
        public required byte[] PasswordHash { get; set; }

        [JsonIgnore]
        public required byte[] PasswordSalt { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
