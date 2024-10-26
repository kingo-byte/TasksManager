using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON
{
    public static class Configuration
    {
        public static JWT? JWT { get; set; }
        public static List<string>? AllowedOrigins { get; set; }
        public static ConnectionStrings? ConnectionStrings { get; set; }    
    }

    public class ConnectionStrings
    {
        private string? TasksManager { get; set; }
    }

    public class JWT 
    {
        public string? PrivateKey { get; set; }
        public int? ExpiryTimeInMinutes { get; set; } 
    }
}
