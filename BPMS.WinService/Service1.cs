using AppUpdaterContracts;
using BPMS.Common;
using BPMS.Contracts;
using BPMS.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceProcess;
using System.Text;

namespace BPMS.WinService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
            //OnStart(null);
        }

        ServiceHost serverHost = null;
        ServiceHost updateHost = null;

        protected override void OnStart(string[] args)
        {
            //读取本置xml文件
            Config.Current.LoadXml();

            //设置当前连接字符串
            BPMS.Common.AppSettings.ConnectionStrings.ModelContainer = Config.Current.GetConnectionString();
            BPMS.Common.AppSettings.ConfigSettings.StaticAdminAccount = Config.Current.SuperAccount;
            BPMS.Common.AppSettings.ConfigSettings.StaticAdminPassword = Config.Current.SuperPassword;

            if (Config.Current.Binding == EBinding.WsHttpBinding)
            {
                WSHttpBinding wsHttpBinding = new WSHttpBinding();
                wsHttpBinding.SendTimeout = new TimeSpan(0, 30, 0);
                wsHttpBinding.MaxReceivedMessageSize = 1024 * 1024 * 1024;//1G
                wsHttpBinding.MaxBufferPoolSize = 1024 * 1024 * 1024;//1G
                wsHttpBinding.Security.Mode = SecurityMode.None;

                wsHttpBinding.ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas();
                wsHttpBinding.ReaderQuotas.MaxDepth = 1024 * 1024 * 1024;//1G
                wsHttpBinding.ReaderQuotas.MaxStringContentLength = 1024 * 1024 * 1024;//1G
                wsHttpBinding.ReaderQuotas.MaxArrayLength = 1024 * 1024 * 1024;//1G
                wsHttpBinding.ReaderQuotas.MaxBytesPerRead = 1024 * 1024 * 1024;//1G
                wsHttpBinding.ReaderQuotas.MaxNameTableCharCount = 1024 * 1024 * 1024;//1G
                /******************************************/
                serverHost = new ServiceHost(typeof(Service));
                serverHost.Description.Endpoints.Clear();
                serverHost.AddServiceEndpoint(typeof(IService), wsHttpBinding,
                    String.Format("http://{0}:{1}/Service/", Config.Current.IP, Config.Current.Port));
            }
            else if (Config.Current.Binding == EBinding.NetTcpBinding)
            {
                NetTcpBinding netTcpBinding = new NetTcpBinding();
                netTcpBinding.TransferMode = TransferMode.Buffered;
                netTcpBinding.SendTimeout = new TimeSpan(0, 30, 0);
                netTcpBinding.MaxReceivedMessageSize = 1024 * 1024 * 1024;//1G
                netTcpBinding.MaxBufferSize = 1024 * 1024 * 1024;//1G
                netTcpBinding.MaxBufferPoolSize = 1024 * 1024 * 1024;//1G                
                netTcpBinding.Security.Mode = SecurityMode.None;

                netTcpBinding.ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas();
                netTcpBinding.ReaderQuotas.MaxDepth = 1024 * 1024 * 1024;//1G
                netTcpBinding.ReaderQuotas.MaxStringContentLength = 1024 * 1024 * 1024;//1G
                netTcpBinding.ReaderQuotas.MaxArrayLength = 1024 * 1024 * 1024;//1G
                netTcpBinding.ReaderQuotas.MaxBytesPerRead = 1024 * 1024 * 1024;//1G
                netTcpBinding.ReaderQuotas.MaxNameTableCharCount = 1024 * 1024 * 1024;//1G
                /******************************************/
                serverHost = new ServiceHost(typeof(Service));
                serverHost.Description.Endpoints.Clear();
                serverHost.AddServiceEndpoint(typeof(IService), netTcpBinding,
                    String.Format("net.tcp://{0}:{1}/Service/", Config.Current.IP, Config.Current.Port));
            }

            if (serverHost.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
            {
                ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
                behavior.HttpGetEnabled = false; //禁用浏览器查看元数据
                serverHost.Description.Behaviors.Add(behavior);
                //添加元数据
                //serverHost.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "http://localhost:8732/WCFService/mex");
                //serverHost.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "net.tcp://localhost:8733/WCFService/mex");
            }
            else
            {
                serverHost.Description.Behaviors.Find<ServiceMetadataBehavior>().HttpGetEnabled = false;
            }
            //必需在HttpGetEnabled=fakse后添加元数据设置
            if (Config.Current.Binding == EBinding.WsHttpBinding)
                serverHost.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(),
                    String.Format("http://{0}:{1}/Service/mex", Config.Current.IP, Config.Current.Port));
            else if (Config.Current.Binding == EBinding.NetTcpBinding)
                serverHost.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(),
                    String.Format("net.tcp://{0}:{1}/Service/mex", Config.Current.IP, Config.Current.Port));
            serverHost.Opened += delegate
            {
                //Console.WriteLine("WCFService已经启动，按任意键终止服务！");
            };
            serverHost.Open();


            ///**************************开启更新下载服务***********************************************/
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.TransferMode = TransferMode.Streamed;
            binding.SendTimeout = new TimeSpan(0, 30, 0);
            binding.MaxReceivedMessageSize = 1024 * 1024 * 1024;//1G
            binding.MaxBufferPoolSize = 1024 * 1024 * 1024;//1G
            binding.MaxBufferSize = 1024 * 1024 * 1024;//1G            
            binding.Security.Mode = BasicHttpSecurityMode.None;

            updateHost = new ServiceHost(typeof(AppUpdateService));
            updateHost.Description.Endpoints.Clear();
            updateHost.AddServiceEndpoint(typeof(IAppUpdateService), binding,
                String.Format("http://{0}:{1}/AppUpdateService/", Config.Current.IP, Config.Current.UpdatePort));
            if (updateHost.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
            {
                ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
                behavior.HttpGetEnabled = false;
                updateHost.Description.Behaviors.Add(behavior);
            }
            else
            {
                updateHost.Description.Behaviors.Find<ServiceMetadataBehavior>().HttpGetEnabled = false;
            }
            updateHost.Opened += delegate
            {
                //Console.WriteLine("AppUpdateService已经启动，按任意键终止服务！");
            };
            updateHost.Open();

            //Console.Read();
        }

        protected override void OnStop()
        {
            try
            {
                if (serverHost != null)
                    serverHost.Close();
                if (updateHost != null)
                    updateHost.Close();
            }
            catch
            {

            }
        }
    }
}
