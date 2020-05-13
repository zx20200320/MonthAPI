using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonthAPI.Models
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Configuration;// 添加程序集引用（在你的项目找到引用，鼠标右键点击打开，查找到Configuration。然后确定添加OK！）
    using System.Reflection;

    #region [ SqlHelper ]
    public static class SqlDbHelper
    {
        #region [ connectionString ]
        // 连接字符串
        //public static string connectionString = ConfigurationManager.ConnectionStrings["ConnectString"].ConnectionString;


        // 连接字符串
        private static string connectionString = "Data Source=.;Initial Catalog=OA;Integrated Security=True";
        #endregion

        #region [ PrepareCommand ]
        // 准备命令
        private static void PrepareCommand(string cmdText, SqlConnection conn, SqlCommand cmd, CommandType cmdType, params SqlParameter[] commandParameters)
        {
            // 判断连接是否打开
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            // 设置连接对象
            cmd.Connection = conn;
            // 设置命令文本
            cmd.CommandText = cmdText;
            // 设置命令类型
            cmd.CommandType = cmdType;
            // 判断参数是否为空
            if (commandParameters != null)
            {
                cmd.Parameters.AddRange(commandParameters);
            }
        }
        #endregion

        #region [ ExecuteNonQuery ]

        /// <summary>
        /// 执行Sql语句--添加删除修改操作
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="cmdType"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        private static bool ExecuteNonQuery(string cmdText, CommandType cmdType, params SqlParameter[] commandParameters)
        {
            int count;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmdText, conn, cmd, cmdType, commandParameters);
                count = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                conn.Close();
                throw new Exception(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return count > 0 ? true : false;
        }

        // 执行Sql语句---添加删除修改操作
        private static bool ExecuteNonQueryByText(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteNonQuery(cmdText, CommandType.Text, commandParameters);
        }


        /// <summary>
        /// 执行存储过程[Proc]，添加删除修改操作
        /// </summary>
        /// <param name="cmdText">要执行的Sql查询语句</param>
        /// <param name="commandParameters">参数</param>
        /// <returns>成功返回true,否则false</returns>
        public static bool ExecuteNonQueryByProc(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteNonQuery(cmdText, CommandType.StoredProcedure, commandParameters);
        }

        /// <summary>
        /// 执行Sql语句，添加删除修改操作
        /// </summary>
        /// <param name="cmdText">要执行的Sql查询语句</param>
        /// <returns>成功返回true,否则false</returns>
        public static bool ExecuteNonQuery(string cmdText)
        {
            return ExecuteNonQuery(cmdText, CommandType.Text, null);
        }

        #endregion

        #region [ ExecuteReader ]

        /// <summary>
        /// 返回SqlDataReader对象
        /// </summary>
        /// <param name="cmdText">要执行的Sql查询语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">命令参数</param>
        /// <returns>SqlDataReader数据读取器</returns>
        private static SqlDataReader ExecuteReader(string cmdText, CommandType cmdType, params SqlParameter[] commandParameters)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmdText, conn, cmd, cmdType, commandParameters);
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();

                return dr;
            }
            catch (Exception ex)
            {
                conn.Close();
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// 执行Sql语句，返回SqlDataReader对象
        /// </summary>
        /// <param name="cmdText">要执行的Sql查询语句</param>
        /// <param name="commandParameters">命令参数</param>
        /// <returns>SqlDataReader数据读取器</returns>
        private static SqlDataReader ExecuteReaderByText(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteReader(cmdText, CommandType.Text, commandParameters);
        }
        // 执行Proc，返回SqlDataReader对象
        #endregion

        #region [ ExecuteReaderByProc ]
        private static SqlDataReader ExecuteReaderByProc(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteReader(cmdText, CommandType.StoredProcedure, commandParameters);
        }

        /// <summary>
        /// 获取第一行，第一列的值
        /// </summary>
        /// <param name="cmdText">要执行的Sql查询语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">命令参数</param>
        /// <returns>Object类型值</returns>
        private static object ExecuteScalar(string cmdText, CommandType cmdType, params SqlParameter[] commandParameters)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmdText, conn, cmd, cmdType, commandParameters);
                object obj = cmd.ExecuteScalar();
                return obj;
            }
            catch (Exception ex)
            {
                conn.Close();
                throw new Exception(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }


        /// <summary>
        /// 获取第一行，第一列的值
        /// </summary>
        /// <param name="cmdText">要执行的Sql查询语句</param>
        /// <param name="commandParameters">命令参数</param>
        /// <returns>Object类型值</returns>
        private static object ExecuteScalarByText(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteScalar(cmdText, CommandType.Text, commandParameters);
        }


        /// <summary>
        /// 获取第一行，第一列的值
        /// </summary>
        /// <param name="cmdText">要执行的Sql查询语句</param>
        /// <param name="commandParameters">命令参数</param>
        /// <returns>Object类型值</returns>
        public static object ExecuteScalarByProc(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteScalar(cmdText, CommandType.StoredProcedure, commandParameters);
        }

        /// <summary>
        /// 获取第一行，第一列的值.
        /// </summary>
        /// <param name="cmdText">要执行的Sql查询语句</param>
        /// <returns>Object类型值</returns>
        public static object ExecuteScalar(string cmdText)
        {
            return ExecuteScalar(cmdText, CommandType.Text, null);
        }

        #endregion

        #region [ ExecuteDataSet ] 


        /// <summary>
        /// 执行Sql语句,获取DataSet.
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="cmdType"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        private static DataSet ExecuteDataSet(string cmdText, CommandType cmdType, params SqlParameter[] commandParameters)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmdText, conn, cmd, cmdType, commandParameters);
                DataSet ds = new DataSet();
                SqlDataAdapter dr = new SqlDataAdapter(cmd);
                dr.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                throw new Exception(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 执行Sql语句,获取DataSet.
        /// </summary>
        /// <param name="cmdText">要执行的Sql语句</param>
        /// <param name="commandParameters">命令参数</param>
        /// <returns>DataSet类型值</returns>
        private static DataSet ExecuteDataSetByText(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteDataSet(cmdText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// 执行Sql语句,获取DataSet.
        /// </summary>
        /// <param name="cmdText">要执行的Sql语句</param>
        /// <param name="commandParameters">命令参数</param>
        /// <returns>DataSet类型值.</returns>
        public static DataSet ExecuteDataSetByProc(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteDataSet(cmdText, CommandType.StoredProcedure, commandParameters);
        }

        /// <summary>
        /// 执行Sql语句,获取DataSet.
        /// </summary>
        /// <param name="cmdText">要执行的Sql语句</param>
        /// <returns></returns>
        private static DataSet ExecuteDataSet(string cmdText)
        {
            return ExecuteDataSet(cmdText, CommandType.Text, null);
        }

        #endregion

        #region [ ExecuteDataTable ]


        /// <summary>
        /// 执行Sql语句,获取DataTable.
        /// </summary>
        /// <param name="cmdText">要执行的Sql语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="commandParameters">命令参数</param>
        /// <returns>DataTable类型值</returns>
        private static DataTable ExecuteDataTable(string cmdText, CommandType cmdType, params SqlParameter[] commandParameters)
        {
            try
            {
                DataSet ds = ExecuteDataSet(cmdText, cmdType, commandParameters);
                DataTable dt = new DataTable();
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 执行Sql语句,获取DataTable.
        /// </summary>
        /// <param name="cmdText">要执行的Sql语句</param>
        /// <param name="commandParameters">命令参数</param>
        /// <returns>DataTable类型值</returns>
        private static DataTable ExecuteDataTableByText(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteDataTable(cmdText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// 执行Sql语句,获取DataTable.
        /// </summary>
        /// <param name="cmdText">要执行的Sql语句</param>
        /// <returns>DataTable类型值</returns>
        public static DataTable ExecuteDataTable(string cmdText)
        {
            return ExecuteDataTable(cmdText, CommandType.Text, null);
        }

        /// <summary>
        /// 执行Sql语句,获取DataTable.
        /// </summary>
        /// <param name="cmdText">要执行的Sql语句</param>
        /// <param name="commandParameters">命令参数</param>
        /// <returns>DataTable类型值</returns>
        public static DataTable ExecuteDataTableByProc(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteDataTable(cmdText, CommandType.StoredProcedure, commandParameters);
        }
        #endregion

        /// <summary>
        /// 获取对象列表集合.
        /// </summary>
        /// <typeparam name="T">对应的实体模型[属性名与列名要一致]</typeparam>
        /// <param name="cmdText">要执行的查询Sql语句[select开头的语句]</param>
        /// <returns></returns>
        public static List<T> GetList<T>(string cmdText) where T : new()
        {
            DataTable dt = ExecuteDataTable(cmdText, CommandType.Text, null);

            return ConvertToEntityList<T>(dt);
        }


        /// <summary>
        /// DataTable转List集合.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        private static List<T> ConvertToEntityList<T>(DataTable table) where T : new()
        {
            // Create a new type of the entity I want
            List<T> result = new List<T>();

            foreach (DataRow tableRow in table.Rows)
            {
                Type t = typeof(T);
                T returnObject = new T();
                foreach (DataColumn col in table.Columns)
                {
                    string colName = col.ColumnName;

                    // Look for the object's property with the columns name, ignore case
                    PropertyInfo pInfo = t.GetProperty(colName.ToLower(),
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    // did we find the property ?
                    if (pInfo != null)
                    {
                        object val = tableRow[colName];

                        // is this a Nullable<> type
                        bool IsNullable = (Nullable.GetUnderlyingType(pInfo.PropertyType) != null);
                        if (IsNullable)
                        {
                            if (val is System.DBNull)
                            {
                                val = null;
                            }
                            else
                            {
                                // Convert the db type into the T we have in our Nullable<T> type
                                val = Convert.ChangeType
                        (val, Nullable.GetUnderlyingType(pInfo.PropertyType));
                            }
                        }
                        else
                        {
                            // Convert the db type into the type of the property in our entity
                            val = Convert.ChangeType(val, pInfo.PropertyType);
                        }
                        // Set the value of the property with the value from the db
                        pInfo.SetValue(returnObject, val, null);
                    }
                }
                result.Add(returnObject);
            }

            // return the entity object with values
            return result;
        }


    }
    #endregion
}
