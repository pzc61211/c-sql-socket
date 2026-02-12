using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace sqlUserDemo
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        //public static bool checklogin(string username, string password)
        //{
        //    return true;
        //}

        private void Button1_Click(object sender, EventArgs e)
        {
            //获取数据，然后校验数据
            if (string.IsNullOrEmpty(txtUid.Text.Trim()) || string.IsNullOrEmpty(txtPwd.Text.Trim()))
            {
                MessageBox.Show("请输入正确的用户密码！");
                return;
            }
            //如果字符串都从同一个地方获取
            //单例模式
            string conStr = dbHelper.GetCurrentConnectString();
            using (SqlConnection conn = new SqlConnection(conStr))
            {

                using (SqlCommand cmd = new SqlCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;
                    string sql = string.Format(@"select count(*) from userinfo where username='{0}'
                    and pwd='{1}'", txtUid.Text, txtPwd.Text);

                    cmd.CommandText = sql;

                    object result = cmd.ExecuteScalar();

                    int rows = int.Parse(result.ToString());

                    if (rows >= 1)
                    {
                        MessageBox.Show("登录成功");
                    }


                    else
                    {
                        MessageBox.Show("账户或密码错误！");
                    }
                }
                //using (SqlCommand cmd = conn.CreateCommand())
                //{
                //    conn.Open();
                //}
            }



        }
        //连接数据库，做查询

        /// <summary>
        /// 注册功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {


        }
    }
}
