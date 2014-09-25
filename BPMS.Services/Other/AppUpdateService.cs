using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Configuration;
using System.IO;
using System.Xml.Linq;
using AppUpdaterContracts;

namespace BPMS.Services
{
    public class AppUpdateService : IAppUpdateService
    {
        #region 实现接口
        Stream IAppUpdateService.CreateUpdateFileStream(string path)
        {
            return CreateUpdateFileStream(path);
        }
        Stream IAppUpdateService.CreateUpdateXmlStream()
        {
            return CreateUpdateXmlStream();
        }
        List<UpFileInfo> IAppUpdateService.GetUpdateFiles()
        {
            return this.GetUpdateFiles();
        }
        string IAppUpdateService.TestConn(string msg)
        {
            return msg;
        }
        #endregion

        const string AppUpdaterXMLFileName = "AppUpdater.xml";
        static string ClientUpFolder = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Client/");

        #region 获取更新的文件列表
        List<UpFileInfo> GetUpdateFiles()
        {
            List<UpFileInfo> lstUpFile = new List<UpFileInfo>();
            string xmlPath = Path.Combine(ClientUpFolder + AppUpdaterXMLFileName);
            if (!File.Exists(xmlPath))
            {
                throw new Exception("更新目录Xml文件不存在");
            }
            XDocument objDoc = XDocument.Load(xmlPath);
            foreach (XElement objElem in objDoc.Document.Element("update").Element("files").Elements("file"))
            {
                UpFileInfo objUpFile = new UpFileInfo();
                objUpFile.FileName = objElem.Attribute("path").Value;
                objUpFile.Version = objElem.Attribute("ver").Value;
                FileInfo objFile = new FileInfo(Path.Combine(ClientUpFolder, objElem.Attribute("path").Value));
                if (objFile.Exists)
                    objUpFile.FileLength = objFile.Length;
                else
                    objUpFile.FileLength = -1;
                lstUpFile.Add(objUpFile);
            }
            return lstUpFile;
        }
        #endregion

        #region 创建指定更新文件的文件流
        Stream CreateUpdateFileStream(string path)
        {
            if (!CheckUpdateFile(path))
            {
                throw new Exception("指定更新的文件不存在");
            }
            string filePath = Path.Combine(ClientUpFolder + path);
            return new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
        #endregion

        #region 创建更新目录Xml的文件流
        Stream CreateUpdateXmlStream()
        {
            string xmlPath = Path.Combine(ClientUpFolder, AppUpdaterXMLFileName);
            return new FileStream(xmlPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
        #endregion

        #region 检查所更新的文件是否存在合法
        bool CheckUpdateFile(string path)
        {
            bool hasUpdate = true;
            List<UpFileInfo> lstUpFile = GetUpdateFiles();
            hasUpdate = lstUpFile.Where(t => t.FileName == path).Count() > 0;

            if (hasUpdate)
            {
                hasUpdate = File.Exists(Path.Combine(ClientUpFolder, path));
            }
            return hasUpdate;
        }
        #endregion
    }
}
