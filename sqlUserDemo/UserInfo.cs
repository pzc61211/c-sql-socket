using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sqlUserDemo
{
   
    internal class UserInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime LastErrorDateTime { get; set; }  // DateTime? 表示可空
        public int Errortimes { get; set; }
    }
}
