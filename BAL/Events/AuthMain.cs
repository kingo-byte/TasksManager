using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static COMMON.Requests;

namespace BAL.Events
{
    public class AuthMain
    {
        public readonly AuthEvents _AuthEvents;
        public AuthMain(AuthEvents AuthEvents)
        {
            _AuthEvents = AuthEvents;
        }

        public void InitializeEvents()
        {
            _AuthEvents.OnPreEventSignUp += AuthEvents_OnPreEventSignUp;
        }

        private void AuthEvents_OnPreEventSignUp(ref SignUpRequest signUpRequest)
        {
            Console.WriteLine("Hello from prevent sign up");
        }
    }
}
