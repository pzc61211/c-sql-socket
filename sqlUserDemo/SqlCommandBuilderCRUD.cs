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
    public partial class SqlCommandBuilderCRUD : Form
    {
        
        public SqlCommandBuilderCRUD()
        {
            InitializeComponent();
        }

        private void SqlCommandBuilderCRUD_Load(object sender, EventArgs e)
        {
            string sql = @"SELECT ID,Username,pwd,CreateTime FROM UserInfo ";
            DataSet ds=DbHelper.QuerryWithoutParameters(sql);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void btnSave_Click(object sender, EventArgs e)
        {


            string connstr = DbHelper.GetCurrentConnectString();

            //拿到修改完毕的数
           
            string sql = @"SELECT ID,Username,pwd,CreateTime FROM UserInfo ";

            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, connstr))//创建一个adapter
            {
                
                DataTable dt = this.dataGridView1.DataSource as DataTable;
                //SqlCommandBuilder 的作用是：根据你最初查询数据用的 SELECT 语句，自动反推出对应的 INSERT、UPDATE、DELETE 语句
                using (SqlCommandBuilder cmdbuilder =new SqlCommandBuilder(adapter))
                {
                    adapter.Update(dt);//更新数据
                }
            }
            
            MessageBox.Show("保存成功！");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
