using EmployeeManager.Bsl.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Bsl.Dal
{
    public interface IDbHelper
    {
        Task<bool> ExecuteNonQuery(string query, List<SqlParameter> parameters, DbHelperEnum type);

        Task<T> ExecuteScalarFunction<T>(string query, List<SqlParameter> parameters, DbHelperEnum type, string outParams);

        Task<IEnumerable<T>> ExecuteToTableAsync<T>(string query, List<SqlParameter> parameters, DbHelperEnum type) where T : class;
    }
}
