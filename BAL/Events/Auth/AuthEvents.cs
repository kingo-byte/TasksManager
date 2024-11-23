using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static COMMON.Requests;

namespace BAL.Events.Auth
{
    public class AuthEvents
    {
        #region SignUp
        public delegate void Prevent_SignUp(ref SignUpRequest signUpRequest);
        public delegate void PostEvent_SignUp(ref SignUpRequest signUpRequest);
        public event Prevent_SignUp? OnPreEventSignUp;
        public event PostEvent_SignUp? OnPostEventSignUp;

        public void InvokePreEventSignUp(ref SignUpRequest signUpRequest)
        {
            OnPreEventSignUp?.Invoke(ref signUpRequest);
        }
        #endregion
    }
}
