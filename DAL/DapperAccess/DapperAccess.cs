﻿using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMMON;
using Microsoft.Extensions.Options;

namespace DAL.DapperAccess
{
    public class DapperAccess
    {
        private readonly Configuration _configuration;

        public DapperAccess(IOptions<Configuration> configuration)
        {
            _configuration = configuration.Value;
        }

        public T QueryFirst<T>(string storedProcedure, DynamicParameters? parameters)
        {
            using (IDbConnection db = new SqlConnection(_configuration.ConnectionStrings!.TasksManager))
            {
                db.Open();

                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        return db.QueryFirstOrDefault<T>(storedProcedure, parameters = null, commandType: CommandType.StoredProcedure)!;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        public IEnumerable<T> Query<T>(string storedProcedure, DynamicParameters? parameters)
        {
            using (IDbConnection db = new SqlConnection(_configuration.ConnectionStrings!.TasksManager))
            {
                db.Open();

                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        return db.Query<T>(storedProcedure, parameters = null, commandType: CommandType.StoredProcedure);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        public int Execute(string storedProcedure, DynamicParameters? parameters)
        {
            using (IDbConnection db = new SqlConnection(_configuration.ConnectionStrings!.TasksManager))
            {
                db.Open();

                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        int result = db.Execute(storedProcedure, parameters, transaction, commandType: CommandType.StoredProcedure);

                        transaction.Commit();

                        return result;
                    }
                    catch
                    {
                        transaction.Rollback(); 

                        throw;
                    }
                }
            }
        }

        public void BulkExecute(string storedProcedure, List<object>? parameters)
        {
            using (IDbConnection db = new SqlConnection(_configuration.ConnectionStrings!.TasksManager))
            {
                db.Open();

                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        db.Execute(storedProcedure, parameters, transaction, commandType: CommandType.StoredProcedure);

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();

                        throw;
                    }
                }
            }
        }
    }
}