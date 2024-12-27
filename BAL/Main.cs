using COMMON.Models;
using Dapper;
using System.Data;
using static COMMON.Requests;

namespace BAL
{
    public partial class BAL
    {
        #region GetUserWithTasks
        public User? GetUserWithTasks(long id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("P__UserId", id);

            User? user = _dapperAccess.QueryMultiple<User?>(
                "sp_GetUserWithTasks",
                parameters,
                CommandType.StoredProcedure,
                multi =>
                {

                    User? user = multi.ReadFirstOrDefault<User>();

                    if (user != null)
                    {
                        user.Tasks = multi.Read<COMMON.Models.Task>().ToList();
                    }

                    return user;
                });

            return user;
        }
        #endregion

        #region EditTask
        public long EditTask(EditTaskRequest request)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("P__TaskId", request.TaskId, direction: ParameterDirection.InputOutput);
            parameters.Add("P__UserId", request.UserId);
            parameters.Add("P__CategoryCode", request.CategoryCode);
            parameters.Add("P__Title", request.Title);
            parameters.Add("P__Description", request.Description);
            parameters.Add("P__DueDate", request.DueDate);

            _dapperAccess.Execute("sp_EditTask", parameters);

            long taskId = parameters.Get<long>("P__TaskId");

            return taskId;
        }
        #endregion
    }
}
