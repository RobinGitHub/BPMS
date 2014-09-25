using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AppUpdaterContracts
{
    [ServiceContract]
    public interface IAppUpdateService
    {
        /// <summary>
        /// 下载更新文件
        /// </summary>
        /// <param name="fileName">相对路径</param>
        /// <returns></returns>
        [OperationContract]
        Stream CreateUpdateFileStream(string path);

        /// <summary>
        /// 创建更新目录文件流
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Stream CreateUpdateXmlStream();

        /// <summary>
        /// 获取更新目录
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<UpFileInfo> GetUpdateFiles();

        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [OperationContract]
        string TestConn(string msg);
    }
}
