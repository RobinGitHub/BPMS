using BPMS.Common;
using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace BPMS.WinService
{
    public class Config
    {
        private Config() { }

        static string ConfigPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "ServerConfig.xml");

        static Config _config = null;
        static XElement _objRootElem = null;

        public static Config Current
        {
            get
            {
                if (_config == null)
                    _config = new Config();
                return _config;
            }
        }

        public void LoadXml()
        {
            string fileContent = File.ReadAllText(ConfigPath, Encoding.UTF8);
            _objRootElem = XElement.Parse(fileContent);
            XElement dbConfigElem = _objRootElem.Element("DBConfig");
            Server = dbConfigElem.Element("Server").Value;
            Database = dbConfigElem.Element("Database").Value;
            UID = dbConfigElem.Element("UID").Value;
            PWD = dbConfigElem.Element("PWD").Value;

            XElement servicesConfigElem = _objRootElem.Element("ServicesConfig");
            Binding = (EBinding)Enum.Parse(typeof(EBinding), servicesConfigElem.Element("Binding").Value);
            IP = servicesConfigElem.Element("IP").Value;
            Port = Convert.ToInt32(servicesConfigElem.Element("Port").Value);
            UpdatePort = Convert.ToInt32(servicesConfigElem.Element("UpdatePort").Value);

            XElement accountConfigElem = _objRootElem.Element("SuperAccount");
            SuperAccount = accountConfigElem.Element("Account").Value;
            SuperPassword = accountConfigElem.Element("Password").Value;
        }

        public string GetConnectionString()
        {
            return String.Format("metadata=res://*/Model.csdl|res://*/Model.ssdl|res://*/Model.msl;provider=System.Data.SqlClient;provider connection string=\"data source={0};initial catalog={1};persist security info=True;user id={2};password={3};multipleactiveresultsets=True;App=EntityFramework\"",
                Server, Database, UID, PWD);
        }    

        /// <summary>
        /// 数据库IP
        /// </summary>
        public string Server
        {
            get;
            set;
        }
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string Database
        {
            get;
            set;
        }
        /// <summary>
        /// 数据库用户ID
        /// </summary>
        public string UID
        {
            get;
            set;
        }
        /// <summary>
        /// 数据库用户密码
        /// </summary>
        public string PWD
        {
            get;
            set;
        }


        /// <summary>
        /// WCF服务的绑定类型
        /// </summary>
        public EBinding Binding
        {
            get;
            set;
        }
        /// <summary>
        /// WCF服务的IP
        /// </summary>
        public string IP
        {
            get;
            set;
        }
        /// <summary>
        /// WCF服务的端品
        /// </summary>
        public int Port
        {
            get;
            set;
        }
        /// <summary>
        /// WCF服务的更新端品
        /// </summary>
        public int UpdatePort
        {
            get;
            set;
        }

        /// <summary>
        /// 超级帐号名
        /// </summary>
        public string SuperAccount
        {
            get;
            set;
        }

        /// <summary>
        /// 超级帐号密码
        /// </summary>
        public string SuperPassword
        {
            get;
            set;
        }
    }
}
