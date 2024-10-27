using COMMON.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static COMMON.Requests;

namespace BAL.IServices
{
    public interface IUserService
    {
        public User? GetUserWithTasks(long id);
    }
}
