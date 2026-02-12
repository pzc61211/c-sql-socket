using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace socket_client
{
    public partial class client : Form
    {
        
        public client()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }
       
        void showMsg(string msg)
        {
            txtLog.AppendText(msg + "\r\n");
        }


        Socket socketSend;//客户端通信的Socket对象
        private void button1_Click(object sender, EventArgs e)//连接功能实现
        {
            //创建Socket对象
            socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(txtSever.Text.ToString());//将字符串形式的IP地址转换为IPAddress类型
            IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(txtPort.Text.ToString()));
            //连接服务器
            socketSend.Connect(point);
            showMsg("连接服务器成功!");
            //启动接收receive线程
            Thread th = new Thread(Receive);
            th.IsBackground = true;
            th.Start();
        }
        void Receive()//接收服务端 数据
        {
            while (true)
            {
                byte[] buffer = new byte[1024 * 1024 * 5];
               
                int r = socketSend.Receive(buffer);//接收数据，并返回数据的长度
                if (r == 0)
                {
                    break;
                }
                string str1 = System.Text.Encoding.UTF8.GetString(buffer, 1, r - 1);
                showMsg(str1);
                showMsg("信息类型："+buffer[0].ToString());
                showMsg("字节长度："+r.ToString());     
                switch (buffer[0])
                {
                    //消息
                    case 0:
                           
                            string str = System.Text.Encoding.UTF8.GetString(buffer, 1, r - 1);//将字节数组转换为字符串
                            showMsg(socketSend.RemoteEndPoint + "文本消息:" + str);
                            break;
                        //文件
                    case 1:

                        showMsg("已进入1");
                        SaveFileDialog sfd = new SaveFileDialog();
                        try
                        {
                            sfd.InitialDirectory = @"C:\Users\27166\Desktop\1231231.txt";
                            sfd.Filter = "所有文件|*.*";
                            sfd.Title = "保存文件";
                            sfd.ShowDialog(this);
                            string path = sfd.FileName;//获取保存文件路径
                            using (FileStream fsWrite = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                            {
                                string str2 = System.Text.Encoding.UTF8.GetString(buffer, 1, r - 1);
                                showMsg(str2);
                                fsWrite.Write(buffer, 1, r - 1);
                                showMsg("已写入");
                                
                            }
                            MessageBox.Show("文件已保存到:" + path);
                            
                        }
                        catch(Exception ex){
                            MessageBox.Show(ex.Message);
                        }
                        break;
                    //震动
                    case 2:
                        break;
                }

                //if (buffer[0]==0
                //{
                //    string str = System.Text.Encoding.UTF8.GetString(buffer, 0, r-1);//将字节数组转换为字符串
                //    showMsg(socketSend.RemoteEndPoint + ":" + str);
                //}
                //else if()

            }
        }
        private void btSend_Click(object sender, EventArgs e)
        {
            string str = txtSend.Text.Trim();
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);//将字符串转换为字节数组
            socketSend.Send(buffer);
            showMsg("客户端已发送:" + str);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        

        private void txtSend_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
