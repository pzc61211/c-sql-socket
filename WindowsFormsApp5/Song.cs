using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMPLib;


namespace WindowsFormsApp5
{
    public class Song
    {
        public string Name { get; set; }
        public string Path { get; set; }
        private static WindowsMediaPlayer player = new WindowsMediaPlayer();
        public Song(string name, string path)
        {
            Name = name;
            Path = path;
            player.URL = path;
        }
        public Song()
        {   
        }
        ~Song()
        {
            player.controls.stop();
            player.close();
        }
        public static void play(string url)
        {
            player.URL = url;
            player.controls.play();
        }
        

        public static void pause()
        {
            player.controls.pause();
        }

        public static void stop()
        {
            player.controls.stop();

        }
        public static void resume()
        {
            player.controls.play();
        }
        public static void SetVolume(int volume) // 0-100
        {
            player.settings.volume = volume;
        }
        

    }
}
