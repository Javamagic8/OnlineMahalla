using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace OnlineMahalla.Data.Utility
{
    public class DatabaseExt
    {
        private string _connectionString = "";
        public DatabaseExt(string ConnectionString)
        {
            _connectionString = ConnectionString;
        }
        public dynamic GetFirstDataFromSql(string sql, string[] paramnames, object[] paramvalues, System.Data.CommandType CommandType = System.Data.CommandType.Text, System.Data.SqlClient.SqlTransaction ts = null)
        {
            return GetDataFromSql(sql, ArrayParamsToDict(paramnames, paramvalues), CommandType, ts, true).FirstOrDefault();
        }
        public dynamic GetFirstDataFromSql(string sql, Dictionary<string, object> Parameters, System.Data.CommandType CommandType = System.Data.CommandType.Text, System.Data.SqlClient.SqlTransaction ts = null)
        {
            return GetDataFromSql(sql, Parameters, CommandType, ts, true).FirstOrDefault();
        }
        public IEnumerable<dynamic> GetDataFromSql(string sql, string[] paramnames, object[] paramvalues, System.Data.CommandType CommandType = System.Data.CommandType.Text, System.Data.SqlClient.SqlTransaction ts = null, bool GetFirst = false)
        {
            return GetDataFromSql(sql, ArrayParamsToDict(paramnames, paramvalues), CommandType, ts, GetFirst);
        }
        public IEnumerable<dynamic> GetDataFromSql(string sql, Dictionary<string, object> Parameters, System.Data.CommandType CommandType = System.Data.CommandType.Text, System.Data.SqlClient.SqlTransaction ts = null, bool GetFirst = false)
        {
            //sql = CorrectPSql(sql);
            if (ts == null)
            {
                using (System.Data.SqlClient.SqlConnection myConn = new System.Data.SqlClient.SqlConnection(_connectionString))
                {
                    myConn.Open();

                    using (System.Data.SqlClient.SqlCommand myCommand = new System.Data.SqlClient.SqlCommand(sql, myConn))
                    {
                        myCommand.CommandTimeout = 500;
                        myCommand.CommandType = CommandType;
                        foreach (KeyValuePair<string, object> param in Parameters)
                        {
                            DbParameter dbParameter = myCommand.CreateParameter();
                            dbParameter.ParameterName = param.Key;
                            dbParameter.Value = CheckForDbNull(param.Value);
                            if (param.Value?.GetType() == typeof(string))
                                dbParameter.Size = 300;
                            myCommand.Parameters.Add(dbParameter);
                        }
                        using (var dataReader = myCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = GetReadedData(dataReader);
                                yield return dataRow;
                                if (GetFirst)
                                    yield break;
                            }
                        }
                    }

                }
            }
            else
            {
                using (System.Data.SqlClient.SqlCommand myCommand = new System.Data.SqlClient.SqlCommand(sql, ts.Connection,ts))
                {
                    myCommand.CommandTimeout = 500;
                    myCommand.CommandType = CommandType;
                    foreach (KeyValuePair<string, object> param in Parameters)
                    {
                        DbParameter dbParameter = myCommand.CreateParameter();
                        dbParameter.ParameterName = param.Key;
                        dbParameter.Value = CheckForDbNull(param.Value);
                        if (param.Value?.GetType() == typeof(string))
                            dbParameter.Size = 300;
                        myCommand.Parameters.Add(dbParameter);
                    }

                    using (var dataReader = myCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var dataRow = GetReadedData(dataReader);
                            yield return dataRow;
                            if (GetFirst)
                                yield break;
                        }
                    }
                }
            }
        }
        private ExpandoObject GetReadedData(DbDataReader dataReader)
        {
            var dataRow = new ExpandoObject() as IDictionary<string, object>;
            for (var fieldCount = 0; fieldCount < dataReader.FieldCount; fieldCount++)
            {
                var dbdata = dataReader[fieldCount];
                if (dataReader.GetFieldType(fieldCount) == typeof(decimal))
                {
                    if (dbdata == DBNull.Value || dbdata == null)
                        dbdata = default(decimal?);
                }
                if (dataReader.GetFieldType(fieldCount) == typeof(double))
                {
                    if (dbdata == DBNull.Value || dbdata == null)
                        dbdata = default(double?);
                }
                else if (dataReader.GetFieldType(fieldCount) == typeof(int))
                {
                    if (dbdata == DBNull.Value || dbdata == null)
                        dbdata = default(int?);
                }
                else if (dataReader.GetFieldType(fieldCount) == typeof(long))
                {
                    if (dbdata == DBNull.Value || dbdata == null)
                        dbdata = default(long?);
                }
                else
                {
                    if (dbdata == DBNull.Value || dbdata == null)
                        dbdata = default(string);
                }
                dataRow.Add(dataReader.GetName(fieldCount), dbdata);
            }

            return (ExpandoObject)dataRow;
        }

        public object CheckForDbNull(object data)
        {
            return data == null ? DBNull.Value : data;
        }
        public void ExecuteNonQuery( string sql, string[] paramnames, object[] paramvalues, System.Data.CommandType CommandType = System.Data.CommandType.Text, System.Data.SqlClient.SqlTransaction ts = null)
        {
            ExecuteNonQuery(sql, ArrayParamsToDict(paramnames, paramvalues), CommandType, ts);
        }
        public void ExecuteNonQuery(string sql, Dictionary<string, object> Parameters, System.Data.CommandType CommandType = System.Data.CommandType.Text, System.Data.SqlClient.SqlTransaction ts = null)
        {
            if (ts == null)
            {
                using (System.Data.SqlClient.SqlConnection myConn = new System.Data.SqlClient.SqlConnection(_connectionString))
                {
                    myConn.Open();

                    using (System.Data.SqlClient.SqlCommand myCommand = new System.Data.SqlClient.SqlCommand(sql, myConn))
                    {
                        myCommand.CommandType = CommandType;
                        myCommand.CommandTimeout =1200;
                        foreach (KeyValuePair<string, object> param in Parameters)
                        {
                            DbParameter dbParameter = myCommand.CreateParameter();
                            dbParameter.ParameterName = param.Key;
                            dbParameter.Value = param.Value;
                            if (param.Value?.GetType() == typeof(string))
                            {
                                if (param.Value?.ToString().Length > 4000)
                                    dbParameter.Size = -1;
                                else
                                    dbParameter.Size = 4000;
                            }
                            else if (param.Value?.GetType() == typeof(decimal))
                            {
                                dbParameter.Value = param.Value;
                                dbParameter.Precision = 18;
                                dbParameter.Scale = 4;
                            }
                            myCommand.Parameters.Add(dbParameter);
                        }
                        myCommand.ExecuteNonQuery();


                    }

                }
            }
            else
            {
                using (System.Data.SqlClient.SqlCommand myCommand = new System.Data.SqlClient.SqlCommand(sql, ts.Connection, ts))
                {
                    myCommand.CommandType = CommandType;
                    myCommand.CommandTimeout = 1200;
                    foreach (KeyValuePair<string, object> param in Parameters)
                    {
                        DbParameter dbParameter = myCommand.CreateParameter();
                        dbParameter.ParameterName = param.Key;
                        dbParameter.Value = param.Value;
                        if (param.Value?.GetType() == typeof(string))
                        {
                            if (param.Value?.ToString().Length > 4000)
                                dbParameter.Size = -1;
                            else
                                dbParameter.Size = 4000;
                        }
                        else if (param.Value?.GetType() == typeof(decimal))
                        {
                            dbParameter.Value = param.Value;
                            dbParameter.Precision = 18;
                            dbParameter.Scale = 4;
                        }
                        myCommand.Parameters.Add(dbParameter);
                    }
                    myCommand.ExecuteNonQuery();
                }
            }
        }
        public object ExecuteScalar(string sql, string[] paramnames, object[] paramvalues, System.Data.CommandType CommandType = System.Data.CommandType.Text, System.Data.SqlClient.SqlTransaction ts = null)
        {
            return ExecuteScalar(sql, ArrayParamsToDict(paramnames, paramvalues), CommandType, ts);
        }
        public object ExecuteScalar( string sql, Dictionary<string, object> Parameters, System.Data.CommandType CommandType = System.Data.CommandType.Text, System.Data.SqlClient.SqlTransaction ts = null)
        {
            if (ts == null)
            {
                using (System.Data.SqlClient.SqlConnection myConn = new System.Data.SqlClient.SqlConnection(_connectionString))
                {
                    myConn.Open();

                    using (System.Data.SqlClient.SqlCommand myCommand = new System.Data.SqlClient.SqlCommand(sql, myConn))
                    {
                        myCommand.CommandType = CommandType;
                        foreach (KeyValuePair<string, object> param in Parameters)
                        {
                            DbParameter dbParameter = myCommand.CreateParameter();
                            dbParameter.ParameterName = param.Key;
                            dbParameter.Value = param.Value;
                            if (param.Value?.GetType() == typeof(string))
                            {
                                if (param.Value?.ToString().Length > 4000)
                                    dbParameter.Size = -1;
                                else
                                    dbParameter.Size = 4000;
                            }
                            else if (param.Value?.GetType() == typeof(decimal))
                            {
                                dbParameter.Value = param.Value;
                                dbParameter.Precision = 18;
                                dbParameter.Scale = 4;
                            }
                            myCommand.Parameters.Add(dbParameter);
                        }
                        return myCommand.ExecuteScalar();
                    }
                }
            }
            else
            {
                using (System.Data.SqlClient.SqlCommand myCommand = new System.Data.SqlClient.SqlCommand(sql, ts.Connection, ts))
                {
                    myCommand.CommandType = CommandType;
                    foreach (KeyValuePair<string, object> param in Parameters)
                    {
                        DbParameter dbParameter = myCommand.CreateParameter();
                        dbParameter.ParameterName = param.Key;
                        dbParameter.Value = param.Value;
                        if (param.Value?.GetType() == typeof(string))
                        {
                            if (param.Value?.ToString().Length > 4000)
                                dbParameter.Size = -1;
                            else
                                dbParameter.Size = 4000;
                        }
                        else if (param.Value?.GetType() == typeof(decimal))
                        {
                            dbParameter.Value = param.Value;
                            dbParameter.Precision = 18;
                            dbParameter.Scale = 4;
                        }
                        myCommand.Parameters.Add(dbParameter);
                    }
                    return myCommand.ExecuteScalar();
                }
            }
        }

        public long GetSequenceNextNumber(string Name, System.Data.SqlClient.SqlTransaction ts = null)
        {
            if (ts == null)
            {
                using (System.Data.SqlClient.SqlConnection myConn = new System.Data.SqlClient.SqlConnection(_connectionString))
                {
                    myConn.Open();
                    using (System.Data.SqlClient.SqlCommand myCommand = new System.Data.SqlClient.SqlCommand("SELECT NEXT VALUE FOR [" + Name + "]", myConn))
                    {
                        return Convert.ToInt64(myCommand.ExecuteScalar());
                    }
                }
            }
            else
            {
                using (System.Data.SqlClient.SqlCommand myCommand = new System.Data.SqlClient.SqlCommand("SELECT NEXT VALUE FOR [" + Name + "]", ts.Connection, ts))
                {
                    return Convert.ToInt64(myCommand.ExecuteScalar());
                }

            }

        }
        public int GetSequenceNextIntNumber( string Name, System.Data.SqlClient.SqlTransaction ts = null)
        {
            return Convert.ToInt32(GetSequenceNextNumber(Name, ts));
        }
        private Dictionary<string, object> ArrayParamsToDict(string[] paramnames, object[] paramvalues)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            if (paramnames == null)
                return res;
            if (paramnames.Length != paramvalues.Length)
                throw new Exception("Parametrs and Values count not equal.");
            for (int i = 0; i < paramnames.Length; i++)
                res.Add(paramnames[i], (paramvalues[i] == null ? DBNull.Value : paramvalues[i]));
            return res;
        }

    }


}
