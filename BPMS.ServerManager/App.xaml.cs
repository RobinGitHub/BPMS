using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;

namespace BPMS.ServerManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex MyMutex = null;
        public App()
        {
            /******************检查实例是否唯一******************/
            bool createNew = false;
            MyMutex = new Mutex(true, "BPMS.ServerManager", out createNew);
            if (!createNew)
            {
                Environment.Exit(0);
                return;
            }
            /******************************************************/
            //如果把IP地址比作一间房子 ，端口就是出入这间房子的门。真正的房子只有几个门，
            //            但是一个IP地址的端口 可以有65536（即：2^16）个之多！
            //            端口是通过端口号来标记的，端口号只有整数，范围是从0 到65535（2^16-1）。 
        }
    }
}
