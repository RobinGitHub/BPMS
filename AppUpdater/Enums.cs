using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppUpdater
{
    public enum EFileStatus
    { 
        None = 0,
        /// <summary>
        /// 连接服务器
        /// </summary>
        ConnServer = 1,
        /// <summary>
        /// 下载需更新的文件列表
        /// </summary>
        DownloadFileList = 2,
        /// <summary>
        /// 等待下载文件
        /// </summary>
        WaitDownLoadFile = 3,
        /// <summary>
        /// 下载需更新的文件
        /// </summary>
        DownloadFile = 4,
        /// <summary>
        /// 从临时目录中的下载文件 替换至 目标文件
        /// </summary>
        ReplaceUpdateTargetFile = 5,
        /// <summary>
        /// 更新完成
        /// </summary>
        UpdateComplete = 6,
        /// <summary>
        /// 错误
        /// </summary>
        Error = 7,
    }

    /// <summary>
    /// WCF的绑定协议
    /// </summary>
    public enum EBinding
    {
        BasicHttpBinding = 0,
        WsHttpBinding = 1,
        NetTcpBinding = 2,
    }

    public enum EUpdaterCommand
    {
        /// <summary>
        /// 下载更新
        /// </summary>
        DownloadUpdate = 0,
        /// <summary>
        /// 检查更新
        /// </summary>
        CheckUpdate = 1,
    }

    public enum EAppExitCode
    {
        /// <summary>
        /// 唯一实例
        /// </summary>
        OnceInstance = 1,
        /// <summary>
        /// 错误参数
        /// </summary>
        ErrorParams = 2,
        /// <summary>
        /// 服务不存在
        /// </summary>
        NothingService = 3,
        /// <summary>
        /// 不需更新
        /// </summary>
        NoUpdate = 4,
        /// <summary>
        /// 需要更新
        /// </summary>
        NeedUpdate = 5,
    }
}
