using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace AppUpdater
{
    public class Config
    {
        private Config() { }

        #region 属性
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
        /// 是否已登录
        /// </summary>
        public bool IsLogin
        {
            get;
            set;
        }
        #endregion

        static string ConfigPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "ClientConfig.xml");

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

            Binding = (EBinding)Enum.Parse(typeof(EBinding), _objRootElem.Element("Binding").Value);
            IP = _objRootElem.Element("IP").Value;
            Port = Convert.ToInt32(_objRootElem.Element("Port").Value);
            UpdatePort = Convert.ToInt32(_objRootElem.Element("UpdatePort").Value);
            IsLogin = _objRootElem.Element("IsLogin").Value == "1";
        }

        public void Save()
        {
            _objRootElem.Element("Binding").Value = Binding.ToString();
            _objRootElem.Element("IP").Value = IP;
            _objRootElem.Element("Port").Value = Port.ToString();
            _objRootElem.Element("UpdatePort").Value = UpdatePort.ToString();
            _objRootElem.Element("IsLogin").Value = IsLogin ? "1" : "0";

            using (StreamWriter sw = new StreamWriter(ConfigPath, false, Encoding.UTF8))
            {
                _objRootElem.Save(sw);
                sw.Flush();
                sw.Close();
            }
        }
    }
}
