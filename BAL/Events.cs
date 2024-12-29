using COMMON.Models;
using static COMMON.Requests;
using static COMMON.Responses;

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

        #region GetLookupByTableNames
        public delegate void Prevent_GetLookupByTableNames(ref GetLookupByTableNamesRequest request);
        public delegate void PostEvent_GetLookupByTableNames(GetLookupByTableNamesRequest request, ref GetLookupByTableNamesResponse response, List<Lookup> lookups);
        public event Prevent_GetLookupByTableNames? OnPreEventGetLookupByTableNames;
        public event PostEvent_GetLookupByTableNames? OnPostEventGetLookupByTableNames;
        #endregion
    }
}
