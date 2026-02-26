using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sqlUserDemo
{
    public partial class DataGridView : Form
    {
        public DataGridView()
        {
            InitializeComponent();
        }

        
        private void DataGridView_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“phpdb2DataSet.UserInfo”中。您可以根据需要移动或移除它。
            this.userInfoTableAdapter.Fill(this.phpdb2DataSet.UserInfo);//c#自带的加载1
            dataGridView1.AllowUserToAddRows = false;  // 禁用新行
            //string connstr = DbHelper.GetCurrentConnectString();
            //string sql = @"SELECT Username,pwd,CreateTime FROM UserInfo ";
            //DataSet ds=DbHelper.Querry(sql);
            //this.dataGridView2.DataSource = ds.Tables[0];

            querrydata2();//自己写的加载

        }
        public void querrydata1()
        {
            string sql = @"SELECT * FROM UserInfo ";
            DataSet ds = DbHelper.QuerryWithoutParameters(sql);
            this.dataGridView1.DataSource = ds.Tables[0];
        }
        public void querrydata2()
        {
            string sql = @"SELECT Username,pwd,CreateTime FROM UserInfo ";
            DataSet ds = DbHelper.QuerryWithoutParameters(sql);
            this.dataGridView2.DataSource = ds.Tables[0];
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)  // 只处理数据行，忽略列头点击
            {
                // 关键：检查是否是新行
                if (dataGridView1.Rows[e.RowIndex].IsNewRow)
                {
                    // 空行：清空文本框或设置默认值
                    txtName.Text = "";
                    txtpwd.Text = "";
                    txtID.Text = "";  // 或 "新记录"
                    return;  // 直接返回，不继续执行
                }

                // 正常行的处理
                try
                {
                    txtName.Text = dataGridView1.Rows[e.RowIndex].Cells["colUsername"].Value?.ToString() ?? "";
                    txtpwd.Text = dataGridView1.Rows[e.RowIndex].Cells["colpwd"].Value?.ToString() ?? "";
                    txtID.Text = dataGridView1.Rows[e.RowIndex].Cells["colID"].Value?.ToString() ?? "";
                }
                catch (Exception ex)
                {
                    // 处理可能的异常
                    MessageBox.Show($"获取数据出错：{ex.Message}");
                }
            }
        }

        private void dataGridView1_MouseEnter(object sender, EventArgs e)
        {
            
        }

        private string GetTipText(DataGridViewCell cell)
        {
            string columnName = dataGridView1.Columns[cell.ColumnIndex].Name;
            string cellValue = cell.Value?.ToString() ?? "空";

            switch (columnName)
            {
                case "UserId":
                    return $"用户ID: {cellValue}";
                case "colUsername":
                    return $"姓名: {cellValue}";
                case "Email":
                    return $"邮箱地址: {cellValue}";
                case "Remark":
                    // 长文本可以显示完整内容
                    return cellValue.Length > 10 ? cellValue : null;
                default:
                    return null;
            }

        }


        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // 根据单元格内容设置提示
                string tipText = GetTipText(cell);

                if (!string.IsNullOrEmpty(tipText))
                {
                    dataGridView1.ShowCellToolTips = true;

                    cell.ToolTipText = tipText;
                   
                }
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        
        private void btnUpdate_Click(object sender, EventArgs e)//更新数据
        {
            string SQL = "update userinfo set pwd=@pwd,username=@username where id=@id";
            SqlParameter[] parameters= new SqlParameter[]
            {
                new SqlParameter("@pwd",txtpwd.Text),
                new SqlParameter("@username",txtName.Text),
                new SqlParameter("@id",txtID.Text)
            };
            DbHelper.RunSqlWithParameters(SQL, parameters);
            //string sql = string.Format("update userinfo set pwd='{0}' ,username='{1}'where ID={2}", 
            //    txtpwd.Text, txtName.Text, txtID.Text);
            //DbHelper.runsql(sql);
            querrydata1();
            querrydata2();
            //MessageBox.Show("修改成功");
        }
         
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {


                if (this.dataGridView1.SelectedRows.Count <= 0)//判断是否选中行
                {
                    MessageBox.Show("请先选择要删除的行");
                    return;
                }
                if (MessageBox.Show("确认要删除吗", "注意消息", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)//确认删除
                {
                    return;
                }
                MessageBox.Show("当前选中行ID" + this.dataGridView1.SelectedRows[0].Cells["colID"].Value.ToString());
                int deleteID = Convert.ToInt32(this.dataGridView1.SelectedRows[0].Cells["colID"].Value);
                string sql = string.Format("delete from userinfo where ID={0}", deleteID);
                DbHelper.RunSqlWithoutParameters(sql);
                MessageBox.Show("删除成功");
            }
            catch(Exception ex)
            {
                MessageBox.Show("删除失败，错误信息：" + ex.Message);
            }
            finally
            {
                querrydata1();
                querrydata2();
            }
            
        }


        #region 双击弹出修改窗口
        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0)
            {
                return;
            }
            int id = int.Parse(dataGridView1.SelectedRows[0].Cells["colID"].Value.ToString());

            EditUserInfoFrm EditForm = new EditUserInfoFrm(new UserInfo() { ID= id,});
            
            EditForm.FormClosing+=EditUserInfoFrm_FormClosing;

            EditForm.Show();
        }

        private void EditUserInfoFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.userInfoTableAdapter.Fill(this.phpdb2DataSet.UserInfo);//关闭修改窗口后刷新数据
        }


        #endregion

        private void btnSelect_Click(object sender, EventArgs e)
        {
            string connstr = DbHelper.GetCurrentConnectString();    
            string sqlText="select * from userinfo";
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<string> wherelist = new List<string>();
            if(!string.IsNullOrEmpty(txtSearchID.Text.Trim()))
            {
                wherelist.Add(" ID like @ID ");
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@ID";
                parameter.Value = "%" + txtSearchID.Text.Trim() + "%";
                parameters.Add(parameter);
            }
            if (!string.IsNullOrEmpty(txtSearchUsername.Text.Trim()))
            {
                wherelist.Add(" Username like @Username ");
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@Username";
                parameter.Value = "%" + txtSearchUsername.Text.Trim() + "%";
                parameters.Add(parameter);
            }
            if(wherelist.Count>0)
            {
                sqlText += " where " + string.Join(" and ", wherelist);//join方法自动将list中的元素用指定的字符串连接起来
            }
            DataSet ds = DbHelper.QuerryWithParameters(sqlText, parameters.ToArray());
            this.dataGridView1.DataSource = ds.Tables[0];
        }
    }
}
