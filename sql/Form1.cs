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

namespace sql
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        void showMsg(string msg)
        {
            txtLog.AppendText(msg + "\r\n");
        }

        const string constr = "server=.\\sql2019;uid=sa;pwd=domino123;database=phpdb2";
        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                //登录配置文件
                //
                
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    showMsg("已打开数据库");
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        
                        // cmd.CommandText = "select * from test2";
                        cmd.CommandText = @"insert into test2(Name,birthday)values(N'西红柿',getdate())";
                        //返回影响的行数
                        int r = cmd.ExecuteNonQuery();
                        showMsg("影响行数"+r.ToString());
                        SqlDataReader rdr = cmd.ExecuteReader();
                        DataSet dataSet = new DataSet();
                    }
                    
                }
            }
           catch(Exception ex)
            {
                showMsg(ex.Message);
            }
         
        }

        private void button2_Click(object sender, EventArgs e)
        {

            using (SqlConnection sql =new SqlConnection(constr))
            {

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
