using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BPMS.Model;

namespace BPMS.Views.Default
{
    public class CheckUpdater
    {
        static string AppUpdaterPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "AppUpdater.exe");

        static int ThreadNum = 1; //最大线程数
        static long BufferSize = 1024 * 30;//下载缓存大小KB
        static int DelayMillisecond = 0;//下载速度控制(毫秒)
        static string ExecutablePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "BPMS.Views.Default.exe");

        #region 检查更新
        /// <summary>
        /// 检查更新
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="updatePort"></param>
        /// <returns>
        /// 1-不需要更新, 
        /// 2-检测到更新, 
        /// -1应用程序不存在, 
        /// -2唯一的实例, 
        /// -3指定的目标服务不存在, 
        /// -4初始化参数不正确
        /// </returns>
        public static int CheckUpdate(string ip, int updatePort)
        {
            int rlt = 1;
            string endPointAddress = GetEndAddress(ip, updatePort);
            if (!File.Exists(AppUpdaterPath))
                rlt = -1;
            if (rlt == 1)
            {
                try
                {
                    AppExitCode exitCode = (AppExitCode)AppUpdaterHelper.CheckNewFiles(AppUpdaterPath, endPointAddress);
                    switch (exitCode)
                    { 
                        case AppExitCode.OnceInstance:
                            rlt = -2;
                            break;
                        case AppExitCode.NothingService:
                            rlt = -3;
                            break;
                        case AppExitCode.ErrorParams:
                            rlt = -4;
                            break;
                        case AppExitCode.NeedUpdate:
                            rlt = 2;
                            break;
                        case AppExitCode.NoUpdate:
                            rlt = 1;
                            break;
                        default:
                            rlt = -1;
                            break;
                    }
                }
                catch
                {
                    rlt = -1;
                }
            }
            return rlt;
        }
        #endregion

        public static string GetEndAddress(string ip, int updatePort)
        {
            return String.Format("http://{0}:{1}/AppUpdateService/", ip, updatePort);
        }
        /// <summary>
        /// 运行更新程序
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="updatePort"></param>
        public static void RunUpdate(string ip, int updatePort)
        {
            string endPointAddress = GetEndAddress(ip, updatePort);
            AppUpdaterHelper.RunUpdater(AppUpdaterPath, endPointAddress, ThreadNum, BufferSize, DelayMillisecond, ExecutablePath);
        }
    }
}
