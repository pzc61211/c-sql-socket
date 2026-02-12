using System.Configuration;

namespace sqlUserDemo
{

    //配置文件连接字符串
    internal class dbHelper
    {
        private static string connStr = ConfigurationManager.ConnectionStrings
            ["connectionStrings"].ConnectionString;//获取连接字符串
        public static string GetCurrentConnectString()
        {
            return connStr;
        }

        //private int pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"]);
        //private bool enableLog = bool.Parse(ConfigurationManager.AppSettings["EnableLog"]);
    }
}
