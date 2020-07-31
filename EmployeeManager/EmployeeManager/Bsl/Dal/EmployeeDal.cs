using EmployeeManager.Bsl.Model;
using EmployeeManager.Bsl.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Bsl.Dal
{
    public class EmployeeDal : IEmployeeDal
    {
        private IDbHelper _db;
        public EmployeeDal(IDbHelper db)
        {
            this._db = db;
        }

        public async Task<EmployeeModel> GetById(int id)
        {
            try
            {
                string sp = "Employee_GetById";
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("Id", id));
                var data = await _db.ExecuteToTableAsync<EmployeeModel>(sp, parms, DbHelperEnum.StoredProcedure);
                return data != null ? data.FirstOrDefault() : null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EmployeeModel> GetByEmail(string email)
        {
            try
            {
                string sp = "Employee_GetByEmail";
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("Email", email));
                var data = await _db.ExecuteToTableAsync<EmployeeModel>(sp, parms, DbHelperEnum.StoredProcedure);
                return data != null ? data.FirstOrDefault() : null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
