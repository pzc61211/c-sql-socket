using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;




namespace WindowsFormsApp5
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

        
        //泛型存储  
        List<string> list = new List<string>();
        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择歌曲";
            ofd.InitialDirectory = @"C:\KwDownload\song";
            ofd.Filter = "音乐文件|*.flac|所有文件|*.*";
            ofd.Multiselect = true;
            ofd.ShowDialog();
            string[] files = ofd.FileNames;
            for (int i = 0; i < files.Length; ++i)
            {
                listBox1.Items.Add(Path.GetFileNameWithoutExtension(files[i]));
                list.Add(files[i]);
            }
            //foreach (string item in ofd.FileNames)
            //{
            //    listBox1.Items.Add(System.IO.Path.GetFileName(item));
            //}
        }
        /// <summary>
        /// 双击实现播放音乐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            Song.play(@"C:\KwDownload\song\" + listBox1.SelectedItem.ToString());
        }

        


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
         
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Song.SetVolume(Convert.ToInt32(txtvolume.Text));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(btplay.Text=="暂停")
            {
                Song.pause();
                btplay.Text="播放";
            }
            else
            {
                Song.resume();
                btplay.Text = "暂停";
            }
        }
                     //上一首
        private void button2_Click(object sender, EventArgs e)
        {
            Song.stop(); 
            int index= listBox1.SelectedIndex;
            if (index == 0)
            {
                index = listBox1.Items.Count - 1;
            }
            else
            {
                index--;
            }
            listBox1.SelectedIndex = index;
           Song.play(@"C:\KwDownload\song\" + listBox1.SelectedItem.ToString());
        }
        /// <summary>
        /// 下一首
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            Song.stop();
            //  player.controls.stop();
            int index = listBox1.SelectedIndex;
            if (index == listBox1.Items.Count - 1)
            {
                index = 0;
            }
            else
            {
                index++;

            }
            listBox1.SelectedIndex = index;
          //  player.URL = @"C:\KwDownload\song\" + listBox1.SelectedItem.ToString();
          //  player.controls.play();
            Song.play(@"C:\KwDownload\song\" + listBox1.SelectedItem.ToString());
        }
    }

   
}
