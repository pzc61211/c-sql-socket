using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace sqlUserDemo
{

    //配置文件连接字符串
    internal class DbHelper
    {
        private static string connStr = ConfigurationManager.ConnectionStrings
            ["connectionStrings"].ConnectionString;//获取连接字符串


        
        //querry方法，返回一个dataset对象，便于填充datagridview
        static public DataSet QuerryWithParameters(string sql, params SqlParameter[] parameters)
        {
            DataSet ds = new DataSet();
            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, connStr))
            {
                adapter.SelectCommand.Parameters.AddRange(parameters.ToArray());
                adapter.Fill(ds);
                return ds;//返回一个dataset对象，便于填充datagridview
            }

        }
        static public DataSet QuerryWithoutParameters(string sql)
        {
            DataSet ds = new DataSet();
            using (SqlDataAdapter da = new SqlDataAdapter(sql, connStr))
            {
                    da.Fill(ds);
                    return ds;//返回一个dataset对象，便于填充datagridview
            }

        }
        //runsql方法，仅仅执行sql语句，不返回结果，适用于insert、update、delete等操作
        public static void RunSqlWithParameters(string sql,params SqlParameter[] parameters)//带有防注入的sql语句执行方法
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd=new SqlCommand(sql,conn))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    conn.Open();
                    cmd.ExecuteNonQuery(); 
                }
            }
        }
        static public void RunSqlWithoutParameters(string sql)
        {

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                //using (SqlCommand cmd = conn.CreateCommand())
                //{
                //    conn.Open();
                //    cmd.CommandText = sql;
                //    cmd.ExecuteNonQuery();
                //}
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        //
        static public DataTable RunStoredProcedure(string sql,CommandType commandType,params SqlParameter[] parameters)
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter(sql,connStr))
            {
                DataTable dt = new DataTable();
                if(parameters != null && parameters.Length > 0)
                {
                    adapter.SelectCommand.Parameters.AddRange(parameters);
                }
                adapter.SelectCommand.CommandType = commandType;
                adapter.Fill(dt);
                return dt;

            }
        }

        //返回第一行第一列的值，类型为object
        public static object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }
        //返回第一行第一列的值，并且转换为指定类型
        public static T ExecuteScalar<T>(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql,conn))
                {
                    
                    if (parameters != null && parameters.Length > 0)
                    {   
                        cmd.Parameters.AddRange(parameters);
                    }
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                    {
                        return default(T);
                    }
                  //  return (T)cmd.ExecuteScalar();
                    return (T)Convert.ChangeType(result, typeof(T));
                }
            }
        }
       
        public static SqlDataReader ExcuteReader(string sql,params SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand(sql, conn);
            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
            }
            conn.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);//注意：调用者需要负责关闭连接
        }
        public static string GetCurrentConnectString()
        {
            return connStr;
        }


        //private int pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"]);
        //private bool enableLog = bool.Parse(ConfigurationManager.AppSettings["EnableLog"]);
    }
}







