using AppUpdaterContracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

namespace AppUpdater
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private string GetEndAddress(string IP, int updatePort)
        {
            return String.Format("http://{0}:{1}/AppUpdateService/", IP, updatePort);
        }

        private Mutex MyMutex = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            #region 参数初始化
            string[] args;
            if (e.Args.Length == 0)
            {
                args = new string[6];
                Config.Current.LoadXml();
                string endPointAddress = GetEndAddress(Config.Current.IP, Config.Current.UpdatePort);
                int threadNum = 3; //最大线程数
                long bufferSize = 1024 * 30;//下载缓存大小KB
                int delayMillisecond = 0;//下载速度控制(毫秒)
                string executablePath = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "BPMS.Views.Default.exe");
                args[0] = "0";
                args[1] = endPointAddress;
                args[2] = executablePath;
                args[3] = threadNum.ToString();
                args[4] = bufferSize.ToString();
                args[5] = delayMillisecond.ToString();
            }
            else
                args = e.Args;
            #endregion

            #region 验证程序唯一性
            bool createNew = false;
            MyMutex = new Mutex(true, "AppUpdater", out createNew);
            if (!createNew)
            {
                Console.WriteLine("应用程序只能创建一个实例");
                Environment.Exit((int)EAppExitCode.OnceInstance);
                return;
            }
            #endregion

            #region 初始化参数
            bool isInit = InitParams(args);
            if (!isInit)
            {
                Console.WriteLine("初始化参数不正确");
                MyMutex.ReleaseMutex();
                Environment.Exit((int)EAppExitCode.ErrorParams);
            }
            #endregion

            #region 检查服务器是否存在
            try
            {
                ServiceProxy.Current.TestConn("test");
            }
            catch
            {
                //指定的目标服务不存在
                Console.WriteLine("指定的目标服务不存在");
                MyMutex.ReleaseMutex();
                Environment.Exit((int)EAppExitCode.NothingService);
            }
            #endregion

            #region 检测更新
            if (ConfigSet.Current.Command == EUpdaterCommand.CheckUpdate)
            {
                List<UpFileInfo> lstUpFile = null;
                try
                {
                    lstUpFile = ServiceProxy.Current.GetUpFileList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("编码:ServiceProxy.GetUpFileList\r\n描述:" + ex.Message);
                    MyMutex.ReleaseMutex();
                    Environment.Exit((int)EAppExitCode.NothingService);
                }

                if (lstUpFile.Count > 0)
                {
                    //需要更新
                    Console.WriteLine(EAppExitCode.NeedUpdate.ToString());
                    MyMutex.ReleaseMutex();
                    Environment.Exit((int)EAppExitCode.NeedUpdate);
                }
                else
                {
                    Console.WriteLine(EAppExitCode.NoUpdate.ToString());
                    MyMutex.ReleaseMutex();
                    Environment.Exit((int)EAppExitCode.NoUpdate);
                }
            }
            #endregion

            else if (ConfigSet.Current.Command == EUpdaterCommand.DownloadUpdate)
            {
                base.OnStartup(e);
            }

            MyMutex.ReleaseMutex();
        }

        private bool InitParams(string[] args)
        {
            bool isFlag = true;
            try
            {
                Uri uri = new Uri(args[1]);
                ConfigSet.Current.Command = (EUpdaterCommand)Convert.ToInt32(args[0]);
                ConfigSet.Current.ServiceAddress = args[1];

                if (ConfigSet.Current.Command == EUpdaterCommand.DownloadUpdate)
                {
                    if (args.Count() < 3 || !File.Exists(args[2]))
                        isFlag = false;
                    if (isFlag)
                    { 
                        ConfigSet.Current.ExecutablePath = args[2];
                        if (args.Count() >= 4)
                            ConfigSet.Current.ThreadNum = Convert.ToInt32(args[3]);
                        if (args.Count() >= 5)
                            ConfigSet.Current.BufferSize = Convert.ToInt64(args[4]);
                        if (args.Count() >= 6)
                            ConfigSet.Current.DelayMillisecond = Convert.ToInt32(args[5]);
                    }
                }
            }
            catch
            {
                isFlag = false;
            }
            return isFlag;
        }
    }
}
