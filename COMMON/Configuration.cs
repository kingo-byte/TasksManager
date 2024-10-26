using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON
{
    public class Configuration
    {
        public JWT? JWT { get; set; }
        public List<string>? AllowedOrigins { get; set; }
        public ConnectionStrings? ConnectionStrings { get; set; }    
    }

    public class ConnectionStrings
    {
        public string? TasksManager { get; set; }
    }

    public class JWT 
    {
        public string? PrivateKey { get; set; }
        public int? ExpiryTimeInMinutes { get; set; } 
    }
}
