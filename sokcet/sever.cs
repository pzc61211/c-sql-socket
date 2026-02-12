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

namespace sokcet
{
  
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ToolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        #region 启动服务器
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {


                //创建监听Socket
                Socket socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //创建IP对象

                //IPAddress iPAddress = IPAddress.Parse("0.0.0.0");
                IPAddress iPAddress = IPAddress.Any;
                //创建端口对象
                IPEndPoint point = new IPEndPoint(iPAddress, Convert.ToInt32(txtPort.Text));
                socketWatch.Bind(point);
                //通信socket
                showMsg("监听成功");
                socketWatch.Listen(10);

                Thread th = new Thread(Listsen)
                {
                    IsBackground = true
                };
                th.Start(socketWatch);
            }
            catch(Exception ex)
            {
                showMsg("启动失败："+ex.Message);
            }

        }

        List<Socket> sokcetList = new List<Socket>();
        List<int> sokcetIndex = new List<int>();


        //  Socket socketSend;
        #endregion
        #region 1.监听客户端连接
        public void Listsen(object o)
        {
            Socket socketwatch = o as Socket;

            while (true)
           {
                try
                {
                    Socket socketSend = socketwatch.Accept();
                    sokcetList.Add(socketSend);

                    sokcetIndex.Add(sokcetList.Count-1);
                    listBox1.Items.Add(sokcetList.Count - 1); 
                 
                    showMsg(socketSend.RemoteEndPoint.ToString() + ":连接成功");
                    Thread th = new Thread(Receive)
                    {
                        IsBackground = true
                    };
                    th.Start(socketSend);

                }
                catch (Exception ex)
                {
                    showMsg("监听异常：" + ex.Message);
                }

            }
        }
        #endregion
        #region 2.接收客户端发送的数据
        void Receive(object o)
        {
            Socket socketSend = o as Socket;   //因为线程启动时传递过来的是object类型的参数，所以要进行类型转换

            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024 * 1024 * 2];//2M缓冲区
                    int r = socketSend.Receive(buffer);//接收客户端发送的数据
                    if (r > 0)
                    {
                        string str = Encoding.UTF8.GetString(buffer, 0, r);   //将接受到的字节数据转换为字符串
                        showMsg(socketSend.RemoteEndPoint.ToString() + ":" + str);//在文本框中显示接收到的数据
                    }
                    else
                    {
                        showMsg(socketSend.RemoteEndPoint.ToString() + ":断开连接1");
                        //socketSend.Close();
                        break;
                    }
                }
                //catch (SocketException se)
                //{
                //    showMsg(socketSend.RemoteEndPoint.ToString() + ":断开连接2" + se.Message);
                //    // socketSend.Close();
                //    break;
                //}
                catch (Exception ex)
                {
                    showMsg(socketSend.RemoteEndPoint.ToString() + ":断开连接" + ex.Message);
                   // socketSend.Close();
                    break;
                }
            }
        }
        #endregion
        #region 显示日志
        void showMsg(string msg)
        {
            txtLog.AppendText(msg + "\r\n");
        }

        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;//取消跨线程访问检查
        }


        #region 发送指定对象
        private void button4_Click(object sender, EventArgs e)//服务器发送功能实现
        {
            string msg = txtPath.Text.Trim();
            byte[] buffer = Encoding.UTF8.GetBytes(msg);
            
            sokcetList[listBox1.SelectedIndex].Send(buffer);
        }
        #endregion
        #region 实现服务器群发数据功能
        private void button6_Click(object sender, EventArgs e)
        {
            string msg = txtPath.Text.Trim();
            byte[] buffer = Encoding.UTF8.GetBytes(msg);
            List<byte> list = new List<byte> {0};
       
            list.AddRange(buffer);//添加消息内容
            byte[] newbuffer = list.ToArray();
            foreach (var item in sokcetList)
            {
                item.Send(newbuffer);
            }
        }
        #endregion
        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择要发送的文件";
            ofd.InitialDirectory = @"C:\KwDownload\song";
            ofd.Filter = "所有文件|*.*|文本文件|*.txt";
            //ofd.Multiselect = true;
            ofd.ShowDialog();
            txtPath.Text= ofd.FileName;//显示选择的文件路径
        }

        /// <summary>
        /// 发送文件按钮功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            string path = txtPath.Text.Trim();//获取要发送的文件路径
            using (FileStream fsRead = new FileStream(path,FileMode.Open,FileAccess.Read))
                
            {
                byte[] buffer = new byte[1024 * 1024 * 2];
                fsRead.Read(buffer, 0, buffer.Length);//读取文件内容存储到缓冲区，并返回实际读取的字节数
                showMsg("文件读取完成，正在发送文件...:");
                //加头文件类型
                List<byte> list = new List<byte>() { 1 };
               
                list.AddRange(buffer);//添加消息内容
                byte[] newbuffer = list.ToArray();
                showMsg("正在发送文件到："+ sokcetList[listBox1.SelectedIndex].RemoteEndPoint.ToString());
                sokcetList[listBox1.SelectedIndex].Send(newbuffer);//给制定客户端发送文件
                showMsg("文件发送完成");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
