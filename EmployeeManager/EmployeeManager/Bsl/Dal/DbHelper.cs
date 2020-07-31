using EmployeeManager.Bsl.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Bsl.Dal
{
    public class DbHelper: IDbHelper
    {
        private SqlConnection _con;
        private SqlCommand _cmd;
        private SqlDataAdapter _adapter;
        private readonly int _connectDBTimeOut = 120;

        private static string _connectionString = "";

        public DbHelper()
        {
            _connectionString = AppSettings.Instance.GetConnection(Const.ConnectionString);
        }

        public async Task<bool> ExecuteNonQuery(string query, List<SqlParameter> parameters, DbHelperEnum type)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand(query, con))
                {
                    await con.OpenAsync();
                    cmd.Connection = con;
                    cmd.CommandType = type == DbHelperEnum.StoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
                    cmd.CommandText = query;
                    cmd.CommandTimeout = _connectDBTimeOut;

                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters.ToArray());

                    int result = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                    con.Dispose();

                    if (con.State == ConnectionState.Open)
                        con.Close();

                    return result > 0;
                }
            }
        }

        public async Task<T> ExecuteScalarFunction<T>(string query, List<SqlParameter> parameters, DbHelperEnum type, string outParams)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand(query, con))
                {
                    await con.OpenAsync();
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = type == DbHelperEnum.StoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
                    cmd.CommandText = query;
                    cmd.CommandTimeout = _connectDBTimeOut;

                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters.ToArray());
                    SqlParameter returnValue = cmd.Parameters.Add(new SqlParameter(outParams, 0));
                    returnValue.Direction = ParameterDirection.Output;

                    await cmd.ExecuteNonQueryAsync();

                    con.Dispose();
                    if (con.State == ConnectionState.Open) con.Close();

                    return (T)returnValue.Value;
                }
            }
        }

        public async Task<IEnumerable<T>> ExecuteToTableAsync<T>(string query, List<SqlParameter> parameters, DbHelperEnum type) where T : class
        {
            try
            {
                IEnumerable<T> result = new List<T>();

                using (var con = new SqlConnection(_connectionString))
                {
                    using (var cmd = new SqlCommand(query, con))
                    {
                        Console.WriteLine("Open connecting ......");
                        var watch = System.Diagnostics.Stopwatch.StartNew();
                        await con.OpenAsync();
                        watch.Stop();
                        Console.WriteLine(watch.ElapsedMilliseconds);
                        Console.WriteLine("Open connected");
                        cmd.Parameters.Clear();
                        cmd.CommandType = type == DbHelperEnum.StoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
                        cmd.CommandTimeout = _connectDBTimeOut;

                        if (parameters != null) cmd.Parameters.AddRange(parameters.ToArray());

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                result = await Mapper<T>(reader);
                                reader.Close();
                            }
                        }

                        if (con.State == ConnectionState.Open) con.Close();
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Private func

        public async Task<IList<T>> Mapper<T>(SqlDataReader reader, bool close = true) where T : class
        {
            try
            {
                IList<T> entities = new List<T>();

                if (reader != null && reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        T item = default(T);
                        if (item == null)
                            item = Activator.CreateInstance<T>();
                        Mapper(reader, item);
                        entities.Add(item);
                    }

                    if (close)
                    {
                        reader.Close();
                    }
                }

                return entities;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Mapper<T>(IDataRecord reader, T entity) where T : class
        {
            Type type = typeof(T);

            if (entity != null)
            {
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    var fieldName = reader.GetName(i);
                    try
                    {
                        var propertyInfo = type.GetProperties().FirstOrDefault(info => info.Name.Equals(fieldName, StringComparison.InvariantCultureIgnoreCase));

                        if (propertyInfo != null)
                        {
                            var value = reader[i];
                            if ((reader[i] != null) && (reader[i] != DBNull.Value))
                            {
                                propertyInfo.SetValue(entity, reader[i], null);
                            }
                            else
                            {
                                if (propertyInfo.PropertyType == typeof(System.DateTime) ||
                                    propertyInfo.PropertyType == typeof(System.DateTime?))
                                {
                                    propertyInfo.SetValue(entity, System.DateTime.MinValue, null);
                                }
                                else if (propertyInfo.PropertyType == typeof(string))
                                {
                                    propertyInfo.SetValue(entity, string.Empty, null);
                                }
                                else if (propertyInfo.PropertyType == typeof(bool) ||
                                    propertyInfo.PropertyType == typeof(bool?))
                                {
                                    propertyInfo.SetValue(entity, false, null);
                                }
                                else if (propertyInfo.PropertyType == typeof(decimal) ||
                                    propertyInfo.PropertyType == typeof(decimal?))
                                {
                                    propertyInfo.SetValue(entity, decimal.Zero, null);
                                }
                                else if (propertyInfo.PropertyType == typeof(double) ||
                                propertyInfo.PropertyType == typeof(double?))
                                {
                                    propertyInfo.SetValue(entity, double.Parse("0"), null);
                                }
                                else if (propertyInfo.PropertyType == typeof(float) ||
                           propertyInfo.PropertyType == typeof(float?))
                                {
                                    propertyInfo.SetValue(entity, 0, null);
                                }
                                else if (propertyInfo.PropertyType == typeof(short) ||
                           propertyInfo.PropertyType == typeof(short?))
                                {
                                    propertyInfo.SetValue(entity, short.Parse("0"), null);
                                }
                                else if (propertyInfo.PropertyType == typeof(long) ||
                           propertyInfo.PropertyType == typeof(long?))
                                {
                                    propertyInfo.SetValue(entity, long.Parse("0"), null);
                                }
                                else if (propertyInfo.PropertyType == typeof(int) ||
                           propertyInfo.PropertyType == typeof(int?))
                                {
                                    propertyInfo.SetValue(entity, int.Parse("0"), null);
                                }
                                else
                                {
                                    propertyInfo.SetValue(entity, 0, null);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
