using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPMS.Common
{
    /// <summary>
    /// Config 配置项
    /// </summary>
    public class AppSettings
    {
        #region 系统配置
        /// <summary>
        /// 系统配置
        /// </summary>
        public class ConfigSettings
        {

            /// <summary>
            /// 站点域名
            /// </summary>
            public static string HostNameToCheck
            {
                get
                {
                    return System.Configuration.ConfigurationManager.AppSettings[Consts.ConfigKey.HostNameToCheck];
                }
            }
            /// <summary>
            /// 超级管理员帐号
            /// </summary>
            public static string StaticAdminAccount
            {
                get;
                set;
            }
            /// <summary>
            /// 超级管理员密码
            /// </summary>
            public static string StaticAdminPassword
            {
                get;
                set;
            }
        }
        #endregion

        #region 连接字符串
        public class ConnectionStrings
        {
            public static string ModelContainer
            {
                get;
                set;
            }
        }
        #endregion

        #region 服务地址
        /// <summary>
        /// 服务地址
        /// </summary>
        public class ServiceUris
        {

        }
        #endregion
    }
}
