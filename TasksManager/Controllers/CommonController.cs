using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static COMMON.Requests;
using static COMMON.Responses;

namespace TasksManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController: BaseController
    {
        private readonly BAL.BAL _bal;
        public CommonController(BAL.BAL bal) 
        {
            _bal = bal;
        }

        [HttpPost("GetLookupByTableNames")]
        public IActionResult GetLookupByTableNames(GetLookupByTableNamesRequest request)
        {
            try
            {
                GetLookupByTableNamesResponse response = _bal.GetLookupByTableNames(request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return CreateValidationProblemDetails("GetLookupByTableNames", ex.Message, 500);
            }
        }   
    }
}
