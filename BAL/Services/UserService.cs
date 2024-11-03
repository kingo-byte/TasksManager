using BAL.IServices;
using COMMON;
using COMMON.Models;
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
    public class UserService : IUserService
    {
        private readonly DapperAccess _dal;  
        public UserService(DapperAccess dal)
        {
            _dal = dal; 
        }

        public User? GetUserWithTasks(long id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("P__UserId", id);

            User? user = _dal.QueryMultiple<User?>(
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

    }
}
