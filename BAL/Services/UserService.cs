using BAL.IServices;
using COMMON;
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
        private readonly DapperAccess _dapperAccess;    
        public UserService(DapperAccess dapperAccess)
        {
            _dapperAccess = dapperAccess;
        }
    }
}
