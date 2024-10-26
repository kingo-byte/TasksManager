using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DapperAccess
{
    public class DapperAccess
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperAccess(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("ShipHiveConnectionString")!;
        }

        public T QueryFirst<T>(string storedProcedure, DynamicParameters? parameters)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
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
            using (IDbConnection db = new SqlConnection(_connectionString))
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
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Open();

                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        return db.Execute(storedProcedure, parameters = null, commandType: CommandType.StoredProcedure);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        public void BulkExecute(string storedProcedure, List<object>? parameters)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Open();

                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        db.Execute(storedProcedure, parameters, transaction, commandType: CommandType.StoredProcedure);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        throw new Exception(ex.Message);
                    }
                }
            }
        }
    }
}
