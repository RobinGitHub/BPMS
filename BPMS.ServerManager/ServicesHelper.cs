using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Configuration.Install;
using System.ServiceProcess;
using System.IO;
using System.Threading;

namespace BPMS.ServerManager
{
    public class ServicesHelper
    {
        private ServicesHelper()
        {

        }
        private static ServicesHelper _cmdHelper = null;
        public static ServicesHelper Current
        {
            get
            {
                if (_cmdHelper == null)
                    _cmdHelper = new ServicesHelper();
                return _cmdHelper;
            }
        }

        private static string FilePath = @"E:\Robin\Desktop\BPMS\BPMS.WinService\bin\Debug\BPMS.WinService.exe";
        //Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "BPMS.WinService.exe");
        private const string ServicesName = "BPMS.WinService";

        public void InstallService()
        {
            this.InstallService(null, FilePath, ServicesName);
        }
        public void UnInstallService()
        {
            this.UnInstallService(FilePath, ServicesName);
        }
        public bool ServiceIsExisted()
        {
            return this.ServiceIsExisted(ServicesName);
        }
        public void StartService()
        {
            this.StartService(ServicesName);
        }
        public void StopService()
        {
            this.StopService(ServicesName);
        }
        public ServiceControllerStatus ServiceStatus()
        {
            return this.ServiceStatus(ServicesName);
        }

        #region 私有方法
        private ServiceControllerStatus ServiceStatus(string serviceName)
        {
            try
            {
                ServiceController service = new ServiceController(serviceName);
                return service.Status;
            }
            catch (Exception ex)
            {
                throw new Exception("ServiceIsStart\r\n" + ex.Message);
            }
        }

        /// <summary>
        /// 安装windows服务
        /// </summary>
        private void InstallService(IDictionary stateSaver, string filepath, string serviceName)
        {
            try
            {
                ServiceController service = new ServiceController(serviceName);
                if (!ServiceIsExisted(serviceName))
                {
                    AssemblyInstaller myAssemblyInstaller = new AssemblyInstaller();
                    myAssemblyInstaller.UseNewContext = true;
                    myAssemblyInstaller.Path = filepath;
                    myAssemblyInstaller.Install(stateSaver);
                    myAssemblyInstaller.Commit(stateSaver);
                    myAssemblyInstaller.Dispose();
                    service.Start();
                }
                else
                {
                    if (service.Status != ServiceControllerStatus.Running && service.Status != ServiceControllerStatus.StartPending)
                        service.Start();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("InstallServiceError\r\n" + ex.Message);
            }
        }

        /// <summary>
        /// 卸载windows服务
        /// </summary>
        private void UnInstallService(string filepath, string serviceName)
        {
            try
            {
                if (ServiceIsExisted(serviceName))
                {
                    //UnInstall Service
                    AssemblyInstaller myAssemblyInstaller = new AssemblyInstaller();
                    myAssemblyInstaller.UseNewContext = true;
                    myAssemblyInstaller.Path = filepath;
                    myAssemblyInstaller.Uninstall(null);
                    myAssemblyInstaller.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("UnInstallServiceError\r\n" + ex.Message);
            }
        }

        /// <summary>
        /// 判断window服务是否存在
        /// </summary>
        private bool ServiceIsExisted(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController s in services)
            {
                if (s.ServiceName == serviceName)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 启动windows服务
        /// </summary>
        private void StartService(string serviceName)
        {
            if (ServiceIsExisted(serviceName))
            {
                ServiceController service = new ServiceController(serviceName);
                if (service.Status != ServiceControllerStatus.Running && service.Status != ServiceControllerStatus.StartPending)
                {
                    service.Start();
                    for (int i = 0; i < 60; i++)
                    {
                        service.Refresh();
                        Thread.Sleep(1000);
                        if (service.Status == ServiceControllerStatus.Running)
                        {
                            break;
                        }
                        if (i == 59)
                        {
                            throw new Exception(String.Format("StartServiceError\r\n", serviceName));
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 停止windows服务
        /// </summary>
        private void StopService(string serviceName)
        {
            if (ServiceIsExisted(serviceName))
            {
                ServiceController service = new ServiceController();
                if (service.Status == ServiceControllerStatus.Running)
                {
                    service.Stop();
                    for (int i = 0; i < 60; i++)
                    {
                        service.Refresh();
                        Thread.Sleep(1000);
                        if (service.Status == ServiceControllerStatus.Stopped)
                            break;
                        if (i == 59)
                        {
                            throw new Exception(String.Format("StopServiceError\r\n", serviceName));
                        }
                    }
                }
            }
        }
        #endregion



    }
}
