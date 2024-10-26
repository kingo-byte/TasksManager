using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static COMMON.Requests;

namespace BAL.Services
{
    public interface IAuthService
    {
        public long SignUp(SignUpRequest request);
        public bool ValidateSignUp(SignUpRequest request, out string message);
    }
}
