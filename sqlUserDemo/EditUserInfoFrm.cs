using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Transactions;

namespace sqlUserDemo
{
    public partial class EditUserInfoFrm : Form
    {
        public UserInfo UserInfo { get; set; }
        public EditUserInfoFrm(UserInfo userinfo)
        {
            InitializeComponent();
            UserInfo = userinfo;
        }

        #region 通过userid获取用户信息并显示在界面上
        private void EditUserInfoFrm_Load(object sender, EventArgs e)
        {
            try
            {


                using (SqlConnection conn = new SqlConnection(DbHelper.GetCurrentConnectString()))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "select * from UserInfo where Id=@Id";
                        cmd.Parameters.AddWithValue("@Id", UserInfo.ID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                this.txtName.Text = reader["Username"].ToString();
                                txtpwd.Text = reader["pwd"].ToString();//注意：实际开发中不建议将密码以明文的形式存储在数据库中，这里只是为了演示方便。
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message);
            }
        }
        #endregion
        #region 保存按钮的点击事件方法
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    MessageBox.Show("用户名不能为空！");
                    return;
                }
                if (string.IsNullOrEmpty(txtpwd.Text))
                {
                    MessageBox.Show("密码不能为空！");
                    return;
                }
                string sql = "update UserInfo set username=@username,pwd=@Password where ID=@ID";
                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@username",txtName.Text),
                new SqlParameter("@Password",txtpwd.Text),
                new SqlParameter("@ID",UserInfo.ID)
                };


                DbHelper.RunSqlWithParameters(sql, parameters);

                MessageBox.Show("保存成功！");

            }

            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message);
                return;

            }
        }
        #endregion
        #region 注册保存按钮的点击事件方法
        public void RegistBtnSaveClickEventMethod(EventHandler btnSaveClickMethod)
        {
            if (btnSave!=null)
            {
                btnSave.Click += btnSaveClickMethod;
            }

        }
        #endregion
    }
}
