using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Xml.Linq;
using System.IO;
using AppUpdaterContracts;

namespace AppUpdater
{
    public class ServiceProxy
    {
        private static ServiceProxy _serviceProxy = null;
        private static IAppUpdateService IProxy = null;

        public static ServiceProxy Current
        {
            get
            {
                if (_serviceProxy == null)
                {
                    _serviceProxy = new ServiceProxy();
                    BasicHttpBinding binding = new BasicHttpBinding();
                    binding.TransferMode = TransferMode.Streamed;
                    binding.SendTimeout = new TimeSpan(0, 30, 0);
                    binding.MaxReceivedMessageSize = 1024 * 1024 * 1024; //1G
                    try
                    {
                        EndpointAddress address = new EndpointAddress(ConfigSet.Current.ServiceAddress);
                        ChannelFactory<IAppUpdateService> channelFactory2 = new ChannelFactory<IAppUpdateService>(binding, address);
                        IProxy = channelFactory2.CreateChannel();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("编码：channelFactory2.CreateChannel\r\n描述:" + ex.Message);
                    }
                }
                return _serviceProxy;
            }
        }

        public string TestConn(string msg)
        {
            return IProxy.TestConn(msg);
        }

        public Stream CreateUpdateFileStream(string path)
        {
            return IProxy.CreateUpdateFileStream(path);
        }

        public Stream CreateUpdateXmlStream()
        {
            return IProxy.CreateUpdateXmlStream();
        }

        #region 获取需要更新的文件列件信息
        /// <summary>
        /// 获取需要更新的文件列件信息
        /// </summary>
        /// <returns></returns>
        public List<UpFileInfo> GetUpFileList()
        {
            List<UpFileInfo> lstUpFile = new List<UpFileInfo>();

            List<UpFileInfo> lstLocalUpFile = GetLocalUpFileList();
            List<UpFileInfo> lstSourceUpFile = IProxy.GetUpdateFiles();

            foreach (UpFileInfo objSourceUpFile in lstSourceUpFile)
            {
                UpFileInfo objLocalUpFile = lstLocalUpFile.Where(t => t.FileName == objSourceUpFile.FileName).FirstOrDefault();
                if (objLocalUpFile != null)
                {   //若本地有，但版本不同
                    if (objLocalUpFile.Version != objSourceUpFile.Version)
                    {
                        lstUpFile.Add(objSourceUpFile);
                    }
                }
                else //若本地没有
                    lstUpFile.Add(objSourceUpFile);
            }
            //若服务器配置的文件不存在，则报错
            if (lstUpFile.Where(t => t.FileLength < 0).Count() > 0)
            {
                throw new Exception("服务器配置的文件不存在");
            }

            return lstUpFile;
        }
        #endregion

        #region 获取本地Xml目录
        /// <summary>
        /// 获取本地Xml目录
        /// </summary>
        /// <returns></returns>
        private List<UpFileInfo> GetLocalUpFileList()
        {
            List<UpFileInfo> lstUpFile = new List<UpFileInfo>();
            if (!File.Exists(ConfigSet.Current.XmlFileName))
            {
                return new List<UpFileInfo>();
            }
            XDocument objDoc = XDocument.Load(ConfigSet.Current.XmlFileName);
            foreach (XElement objElem in objDoc.Document.Element("update").Element("files").Elements("file"))
            {
                UpFileInfo objUpFile = new UpFileInfo();
                objUpFile.FileName = objElem.Attribute("path").Value;
                objUpFile.Version = objElem.Attribute("ver").Value;
                FileInfo objFile = new FileInfo(objElem.Attribute("path").Value);
                if (objFile.Exists)
                    objUpFile.FileLength = objFile.Length;
                else
                    objUpFile.FileLength = -1;
                lstUpFile.Add(objUpFile);
            }
            return lstUpFile;
        }
        #endregion
    }
}
