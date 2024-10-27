using BAL.IServices;
using COMMON;
using COMMON.Models;
using DAL.DapperAccess;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static COMMON.Requests;

namespace BAL.Services
{
    public class UserService : IUserService
    {
        private readonly TaskAccess _taskAccess;    
        public UserService(TaskAccess taskAccess)
        {
            _taskAccess = taskAccess;
        }

        public User? GetUserWithTasks(long id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("P__UserId", id);

            User? user = _taskAccess.GetUserWithTasks("sp_GetUserWithTasks", parameters);

            return user;
        }
    }
}
