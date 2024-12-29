using COMMON;
using COMMON.Models;
using static COMMON.Requests;
using static COMMON.Responses;

namespace BAL
{
    public partial class BAL
    {
        public void InitializeEvents() 
        {
            OnPostEventGetLookupByTableNames += BAL_OnPostEventGetLookupByTableNames;
        }

        #region GetLookupByTableNames
        private void BAL_OnPostEventGetLookupByTableNames(GetLookupByTableNamesRequest request, ref GetLookupByTableNamesResponse response, List<Lookup> lookups)
        {
            Dictionary<string, List<Lookup>> result = [];

            List<string> lookupList = request.TableNames.Split(',').ToList();

            foreach (string lookup in lookupList)
            {
                result.Add(lookup, lookups.Where(x => x.TableName == lookup).ToList());
            }

            response.Lookups = result;
        }
        #endregion
    }
}
