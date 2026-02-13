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


        private void Button1_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrEmpty(txtUserid.Text.Trim()) || string.IsNullOrEmpty(txtPwd.Text.Trim()))
            {
                MessageBox.Show("请输入正确的用户密码！");
                return;
            }
           
            string conStr = DbHelper.GetCurrentConnectString();

            using (SqlConnection conn = new SqlConnection(conStr))
            {

                using (SqlCommand cmd = new SqlCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = string.Format(@"select 
                                                Username,
                                                pwd,
                                                LastErrorDateTime,
                                                Errortimes 
                                                from userinfo 
                                                where username='{0}'
                                                and pwd='{1}'", txtUserid.Text, txtPwd.Text);//查找用户输入的id和密码

                    UserInfo userInfo = new UserInfo();
                    

                    #region 获取查询数据
                    using (SqlDataReader reader = cmd.ExecuteReader())//绑定一个reader读取数据
                    {
                        if(reader.Read())
                        {
                            //把数据全部读出来
                            userInfo.UserName = reader["Username"].ToString();
                            userInfo.Password = reader["pwd"].ToString();
                            userInfo.LastErrorDateTime = reader["LastErrorDateTime"] == DBNull.Value
                                 ? DateTime.MinValue  // 或者用 null，但 DateTime 是值类型，要用 DateTime?
                                 : DateTime.Parse(reader["LastErrorDateTime"].ToString());
                            userInfo.Errortimes = int.Parse(reader["Errortimes"].ToString());
                           
                        }
                    }//此时reader一直占用conn操作空间
                    #endregion

                    //如果没找到就要更新数据库
                    if ( userInfo == null )
                    {
                        MessageBox.Show("账号或密码错误!");
                        cmd.CommandText = "update ";
                        cmd.ExecuteNonQuery();
                        return;
                    }
                    

                    //如果错误次数小于3次或者15分钟内登录都可以
                    //如果
                    if (userInfo.Errortimes <= 3 || DateTime.Now.Subtract(userInfo.LastErrorDateTime).Minutes < 15)
                    {
                        MessageBox.Show("登录成功！");
                        cmd.CommandText = @"update Userinfo set Errortimes=0 where username='"+userInfo.UserName+"'";
                        cmd.ExecuteNonQuery();
                    }
                }
             
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
