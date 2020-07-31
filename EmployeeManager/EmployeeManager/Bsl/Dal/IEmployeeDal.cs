using EmployeeManager.Bsl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Bsl.Dal
{
   public interface IEmployeeDal
    {
        Task<EmployeeModel> GetById(int id);
        Task<EmployeeModel> GetByEmail(string email);
    }
}
