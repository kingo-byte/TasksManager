using BAL.IServices;
using COMMON;
using DAL.DapperAccess;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static COMMON.Requests;

namespace BAL.Services
{
    public class TaskService : ITaskService
    {
        private readonly DapperAccess _dapperAccess;
        public TaskService(DapperAccess dapperAccess)
        {
            _dapperAccess = dapperAccess;
        }

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
    }
}
