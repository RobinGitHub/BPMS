using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppUpdater
{
    public class ConfigSet
    {
        private ConfigSet()
        {
            //初始化参数
            this.DelayMillisecond = 0;
            this.BufferSize = 1024 * 10;
            this.ThreadNum = 3;
            this.TempFolder = "~Temp/";
            this.XmlFileName = "AppUpdater.xml";
        }
        public static ConfigSet _config = null;
        public static ConfigSet Current
        {
            get
            {
                if (_config == null)
                {
                    _config = new ConfigSet();
                }
                return _config;
            }
        }

        /// <summary>
        /// 服务地址
        /// </summary>
        public string ServiceAddress
        {
            get;
            set;
        }
        /// <summary>
        /// 命令
        /// </summary>
        public EUpdaterCommand Command
        {
            get;
            set;
        }
        /// <summary>
        /// 启动的应用程序
        /// </summary>
        public string ExecutablePath
        {
            get;
            set;
        }
        /// <summary>
        /// 限速下载(毫秒)
        /// </summary>
        public int DelayMillisecond
        {
            get;
            set;
        }
        /// <summary>
        /// 缓存大小(字节数)
        /// </summary>
        public long BufferSize
        {
            get;
            set;
        }
        /// <summary>
        /// 启用的线程数
        /// </summary>
        public int ThreadNum
        {
            get;
            set;
        }
        /// <summary>
        /// 临时文件夹
        /// </summary>
        public string TempFolder
        {
            get;
            set;
        }
        /// <summary>
        /// 更新目录XML的文件名
        /// </summary>
        public string XmlFileName
        {
            get;
            set;
        }
    }
}
