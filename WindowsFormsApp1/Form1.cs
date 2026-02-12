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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
              SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();
              scsb.UserID = "sa";
              scsb.DataSource = ".\\sql2019";
              scsb.Password = "domino123";
              propGrid4ConString.SelectedObject= scsb;//可以全局访问
           // SqlConnectionStringBuilder scsb  = propGrid4ConString.SelectedObject as SqlConnectionStringBuilder;不这样写是因为不能全局访问
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = propGrid4ConString.SelectedObject.ToString();
            Clipboard.Clear();
            Clipboard.SetText(str);
            MessageBox.Show(str);
            
        }
    }
}
