using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;

namespace 文件导入
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //打开文件
            using (OpenFileDialog ofd =new OpenFileDialog())
            {

                ofd.Multiselect = false;
                ofd.Filter = "文本文件|*.txt";
                if (ofd.ShowDialog()==DialogResult.OK)
                {
                    this.txtfilepath.Text= ofd.FileName;
                    ImportData(ofd.FileName);
                    
                }
            }
        }
        
        private void ImportData(string filename)
        {
            string temp = string.Empty;//临时值

            //streamreader类
            using (StreamReader reader = new StreamReader(filename,
                Encoding.Default))
            {
                
                reader.ReadLine();//把第一行读掉

                string connstr = ConfigurationManager.ConnectionStrings["Sql"].ConnectionString;


                using (SqlConnection conn = new SqlConnection(connstr))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        while (!string.IsNullOrEmpty(temp = reader.ReadLine()))//如果非空
                        {
                            var strs = temp.Split(',');//按照逗号分，返回字符串

                             string sql = @"insert into";
                             cmd.CommandText = sql;
                             cmd.ExecuteNonQuery();
                            
                        }
                    }
                        

                }
            }
        }
    }
}
