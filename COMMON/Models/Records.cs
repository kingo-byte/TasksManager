using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.Models
{
    public class Records
    {
        public record SignInResponse(string AccessToken, string RefreshToken);
    }
}
