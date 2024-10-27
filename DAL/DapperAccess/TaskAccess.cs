using COMMON;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMMON.Models;

namespace DAL.DapperAccess
{
    public class TaskAccess
    {
        private readonly Configuration _configuration;
        public TaskAccess(IOptions<Configuration> configuration)
        {
            _configuration = configuration.Value;
        }

        public User? GetUserWithTasks(string storedProcedure, DynamicParameters? dynamicParameters)
        {
            using (IDbConnection db = new SqlConnection(_configuration.ConnectionStrings!.TasksManager))
            {
                db.Open();

                try
                {
                    using (var multi = db.QueryMultiple(storedProcedure, dynamicParameters)) 
                    {
                        User? user = multi.ReadFirstOrDefault<User>();

                        if (user != null) 
                        {
                            List<COMMON.Models.Task> tasks = multi.Read<COMMON.Models.Task>().ToList();
                            user.Tasks = tasks;
                        }

                        return user;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
