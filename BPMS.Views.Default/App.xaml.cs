using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using DevExpress.Xpf.Core;

namespace BPMS.Views.Default
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //string ip = ConfigurationManager.AppSettings["IP"];
            //int updatePort = Convert.ToInt32(ConfigurationManager.AppSettings["UpdatePort"]);
            ////检查更新
            ////if (!RunUpdate(ip, updatePort))
            ////{
            ////    this.Shutdown(0);
            ////    return;
            ////}
            ////else
            ////{
            //    Login login = new Login();
            //    login.Show();
            ////}
            base.OnStartup(e);
        }

        private bool RunUpdate(string IP, int updatePort)
        {
            //1-不需要更新, 2-检测到更新, -1应用程序不存在, -2唯一的实例, -3指定的目标服务不存在, -4初始化参数不正确
            int iRlt = CheckUpdater.CheckUpdate(IP, updatePort);
            if (iRlt == 1)
                return true;
            if (iRlt == 2)
            {
                bool yes = false;
                this.Dispatcher.Invoke(new Action(() =>
                {
                    yes = (MessageBox.Show("检测到新版本，是否下载更新？", "提示", MessageBoxButton.YesNo,
                        MessageBoxImage.Question) == MessageBoxResult.Yes);
                }));
                if (yes)
                {
                    CheckUpdater.RunUpdate(IP, updatePort);
                }
                this.Dispatcher.Invoke(new Action(() =>
                {
                    //有新版本则必需更新后才能运行
                    Application.Current.Shutdown();
                    Environment.Exit(0);
                }));
            }
            else
            {
                string msg = "";
                switch (iRlt)
                {
                    case -1:
                        msg = "AppUpdater:应用程序不存在";
                        break;
                    case -2:
                        msg = "AppUpdater:必需唯一的实例";
                        break;
                    case -3:
                        msg = "AppUpdater:指定的目标服务不存在";
                        break;
                    case -4:
                        msg = "AppUpdater:初始化参数不正确";
                        break;
                }
                this.Dispatcher.Invoke(new Action(() =>
                {
                    MessageBox.Show(msg);
                }));
            }
            return false;
        }
    }
}
