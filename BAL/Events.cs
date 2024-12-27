using static COMMON.Requests;

namespace BAL
{
    public partial class BAL
    {
        #region SignUp
        public delegate void Prevent_SignUp(ref SignUpRequest signUpRequest);
        public delegate void PostEvent_SignUp(ref SignUpRequest signUpRequest);
        public event Prevent_SignUp? OnPreEventSignUp;
        public event PostEvent_SignUp? OnPostEventSignUp;
        #endregion
    }
}
